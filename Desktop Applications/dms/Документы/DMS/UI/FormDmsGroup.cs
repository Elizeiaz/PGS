using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using PGS.DMS.Models;
using PGS.UI.Groups;

namespace PGS.DMS.UI
{
    /// <summary>
    /// Форма выбора/редактирования групп документов.
    /// </summary>
    public partial class FormDmsGroup : Form
    {
        private readonly bool _isEditMode;

        /// <summary>
        /// Таблица групп для редактирования
        /// </summary>
        public string GroupTable = "dms.doc_groups";

        private readonly NetworkFolder _networkFolder;

        private readonly List<Group> _removedGroup = new List<Group>();

        public string SelectedGroupName;
        public int? SelectedGroupId;

        /// <summary>
        /// Режим выбора группы.
        /// </summary>
        public FormDmsGroup()
        {
            InitializeComponent();

            BtnAdd.Visible = _isEditMode;
            BtnEdit.Visible = _isEditMode;
            BtnRemove.Visible = _isEditMode;
        }

        /// <summary>
        /// Режим редактирования.
        /// </summary>
        public FormDmsGroup(NetworkFolder networkFolder)
        {
            InitializeComponent();
            _isEditMode = true;
            _networkFolder = networkFolder;
        }

        private void FormGroupEdit_Shown(object sender, EventArgs e)
        {
            Group.LoadGroupTree(tv, GroupTable, false);
        }

        #region TreeView

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
            BtnRemove.Enabled = true;
            BtnEdit.Enabled = true;
        }

        private void tv_ItemDrag(object sender, ItemDragEventArgs e)
        {
        }

        private bool ContainsNode(TreeNode node1, TreeNode node2)
        {
            if (node2.Parent == null)
                return false;
            return node2.Parent.Equals(node1) || ContainsNode(node1, node2.Parent);
        }

        private void tv_MouseDown(object sender, MouseEventArgs e)
        {
            if (tv.HitTest(e.X, e.Y).Node != null)
                return;
            tv.SelectedNode = null;
        }

        /// <summary>
        /// Рекурсивное наполнение переданного листа childNodes дочерними узлами node.
        /// </summary>
        /// <param name="childNodes">Лист в который будет идти добавление</param>
        /// <param name="parentNode"></param>
        private void GetAllChilds(List<TreeNode> childNodes, TreeNode parentNode)
        {
            foreach (TreeNode childNode in parentNode.Nodes)
            {
                childNodes.Add(childNode);
                GetAllChilds(childNodes, childNode);
            }
        }

        #endregion

        #region DB Insert/Update

        /// <summary>
        /// Формирует путь к сетевой папке по названиям групп.
        /// </summary>
        private string GetPathFromGroup(Group group)
        {
            var pathList = new List<string>();

            while (group != null)
            {
                pathList.Add(group.Name);

                group = group.Parent;
            }

            pathList.Reverse();

            var path = _networkFolder.CombinePath(pathList.ToArray());

            return path;
        }

        #endregion

        #region Buttons

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!_isEditMode)
            {
                if (tv.SelectedNode == null)
                {
                    MessageBox.Show("Выберите группу", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var selectedNode = tv.SelectedNode;

                if (selectedNode.Tag == null)
                {
                    MessageBox.Show("Ошибка!\nНе удалось выбрать группу", "", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var group = selectedNode.Tag as Group;
                SelectedGroupId = (int)group.ID;
                SelectedGroupName = group.Name;

                Close();
                return;
            }


            var con = DB.Connect();
            try
            {
                var transaction = con.BeginTransaction();

                try
                {
                    foreach (var gr in _removedGroup)
                    {
                        if (!gr.ID.HasValue)
                            continue;
                        var documentGroupId = (int)gr.ID;

                        // Удаление записей из бд
                        DocumentGroup.DeleteDocumentGroups(documentGroupId);

                        // Удаление с диска
                        _networkFolder.DeleteDirectoryByAbsolute(GetPathFromGroup(gr));
                    }

                    foreach (TreeNode node in tv.Nodes) Save(con, transaction, node);


                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Не удалось применить измененения\n{ex}",
                        "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error
                    );
                    transaction.Rollback();
                }
            }
            finally
            {
                con.Disconnect();
            }

            DialogResult = DialogResult.OK;
        }

        private void tv_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            BtnOk_Click(sender, null);
        }

        private void Save(OdbcConnection con, OdbcTransaction tr, TreeNode treeNode)
        {
            if (treeNode.Tag.GetType() == typeof(Group))
            {
                var tag1 = (Group)treeNode.Tag;
                var nullable = treeNode.Parent?.Tag.GetType() == typeof(Group)
                    ? ((Group)treeNode.Parent.Tag).ID
                    : new int?();
                if (!tag1.ID.HasValue)
                {
                    var odbcCommand = new OdbcCommand("", con, tr);
                    odbcCommand.CommandText =
                        $"INSERT INTO {GroupTable} (parentid, name) VALUES ({nullable.ToDBString()}, {treeNode.Text.ToDBString()}) RETURNING ID";
                    tag1.ID = new int?((int)odbcCommand.ExecuteScalar());

                    // Создаём директорию
                    var di = new DirectoryInfo(_networkFolder.CombinePath(treeNode.FullPath));
                    if (!di.Exists)
                        di.Create();
                    else
                        throw new Exception($"Директория уже существует:{di}");
                }
                else
                {
                    var odbcCommand = new OdbcCommand("", con, tr);
                    odbcCommand.CommandText = string.Format("UPDATE {0} SET parentid = {2}, name = {3} WHERE ID = {1}",
                        GroupTable, tag1.ID, nullable.ToDBString(),
                        treeNode.Text.ToDBString());
                    odbcCommand.ExecuteNonQuery();
                }
            }

            for (var index = 0; index < treeNode.Nodes.Count; ++index)
                Save(con, tr, treeNode.Nodes[index]);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var node = new TreeNode("Новая группа")
            {
                Tag = new Group(),
                ImageIndex = 0
            };
            if (tv.SelectedNode != null)
                tv.SelectedNode.Nodes.Add(node);
            else
                tv.Nodes.Add(node);
            node.EnsureVisible();
            tv.SelectedNode = node;
            tv.Focus();
            node.BeginEdit();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null)
                return;
            tv.SelectedNode.BeginEdit();
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null)
                return;

            var selectedNode = tv.SelectedNode;
            selectedNode.Remove();

            if (selectedNode.Tag == null)
                return;

            _removedGroup.Add(selectedNode.Tag as Group);
        }

        #endregion
    }
}