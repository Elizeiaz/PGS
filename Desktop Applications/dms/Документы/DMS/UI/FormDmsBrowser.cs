using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using PGS.DMS.Controllers;
using PGS.DMS.Models;
using PGS.DMS.Properties;

namespace PGS.DMS.UI
{
    public partial class FormDmsBrowser : Form
    {
        #region EditMode

        private bool _editMode = false;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                if (_editMode != value)
                {
                    if (value && !DB.doCheckPermissionLevel("app_dms_rw"))
                        return;

                    _editMode = value;

                    ConfigureEditMode(value);

                    // Подключаемся к SuperUser сетевому соединению
                    if (value && _superUserFolder == null)
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        Application.DoEvents();
                        _superUserFolder = ConnectToNetworkFolder(UncSuperUserPath, SuperUserCredential, true);
                        Cursor.Current = Cursors.Default;
                    }
                }
            }
        }

        private void ConfigureEditMode(bool editMode)
        {
            // Кнопки на верхнем ToolStrip
            btnShowGroupToolStrip.Visible = editMode;
            toolStripSeparator6.Visible = editMode;
            btnDocumentAdd.Visible = editMode;
            btnEdit.Visible = editMode;
            btnRemove.Visible = editMode;
            toolStripSeparator8.Visible = editMode;

            // Кнопки ContextMenu
            tsSoloGroupCreate.Enabled = editMode;

            tsSoloBtnAdd.Enabled = editMode;

            tsGroupCreate.Enabled = editMode;
            tsGroupRename.Enabled = editMode;
            tsGroupDelete.Enabled = editMode;

            tsDocRename.Enabled = editMode;
            tsDocDelete.Enabled = editMode;

            tsGroupListViewRename.Enabled = editMode;
            tsGroupListViewDelete.Enabled = editMode;
        }

        #endregion

        #region Network

        // Подключение для чтения
        public static string UncReadOnlyPath = "";

        public static NetworkCredential ReadOnlyCredential =
            new NetworkCredential("", "", "");

        // Подключение для редактирования
        public static string UncSuperUserPath = "";

        public static NetworkCredential SuperUserCredential =
            new NetworkCredential(@"", "", "");

        private NetworkFolder _readOnlyFolder;

        /// <summary>
        /// Директория на сетевом диске с правами чтения.
        /// </summary>
        internal NetworkFolder ReadOnlyFolder
        {
            get { return _readOnlyFolder; }
            set { _readOnlyFolder = value; }
        }

        private NetworkFolder _superUserFolder;

        /// <summary>
        /// Директория на сетевом диске с правами записи.
        /// </summary>
        internal NetworkFolder SuperUserFolder
        {
            get { return _superUserFolder; }
            set { _superUserFolder = value; }
        }

        private void LoadNetworkConfig()
        {
            var con = DB.Connect();
            try
            {
                var com = con.CreateCommand();
                com.CommandText = "SELECT name, value FROM dms.dms_config";

                var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    var name = (string)reader["name"];
                    var value = reader["value"] == DBNull.Value ? "" : (string)reader["value"];

                    switch (name)
                    {
                        case "Server_Fetch":
                            //if (!Unc.IsUncPath(value))
                            //    value = @"\\" + value;
                            UncReadOnlyPath = value;
                            break;
                        case "User":
                            ReadOnlyCredential.UserName = value;
                            break;
                        case "User_Password":
                            ReadOnlyCredential.Password = value;
                            break;
                        case "User_Domain":
                            ReadOnlyCredential.Domain = value;
                            break;
                        case "Admin":
                            SuperUserCredential.UserName = value;
                            break;
                        case "Domain":
                            SuperUserCredential.Domain = value;
                            break;
                        case "Password":
                            SuperUserCredential.Password = value;
                            break;
                        case "Server_Push":
                            //if (!Unc.IsUncPath(value))
                            //    value = @"\\" + value;
                            UncSuperUserPath = value;
                            break;
                        default:
                            continue;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Не удалось загрузить сетевые настройки!\n{e}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Disconnect();
            }
        }

        #endregion

        public bool IsRevisionHighlight;
        private bool _isRevisionMode;
        public int RevisionModeDay = 30;

        private Color REVISION_MODE_YELLOW = Color.FromArgb(255, 255, 203);
        private Color REVISION_MODE_RED = Color.FromArgb(255, 203, 203);

        private ImageList _smallImageListIcon;
        private UserControlPathNavigator _controlPathNavigator;

        public FormDmsBrowser()
        {
            InitializeComponent();
            AddPathNavigator();

            LoadNetworkConfig();

            UpdateTreeView();

            LoadProperties();

            ConfigureEditMode(EditMode);
        }

        private void FormDocumentBrowser_Load(object sender, EventArgs e)
        {
            TreeViewFix();

            // Подключаемся к ReadOnly сетевому соединению
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            _readOnlyFolder = ConnectToNetworkFolder(UncReadOnlyPath, ReadOnlyCredential, true);
            Cursor.Current = Cursors.Default;

            FillSmallImageListIcon();
            documentListView.SmallImageList = _smallImageListIcon;

            OptimizeForm();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SavePropertirs();
        }

        /// <summary>
        /// Копирует иконки из imageListIcon (48x48), делая их (13x13).
        /// </summary>
        private void FillSmallImageListIcon()
        {
            _smallImageListIcon = new ImageList()
            {
                ColorDepth = (ColorDepth)24,
                ImageSize = new Size(13, 13)
            };

            foreach (Image image in imageListIcons.Images) _smallImageListIcon.Images.Add(image);
        }

        /// <summary>
        /// Добавление в toolStripPath элемента UserControlPathNavigator и его настройка.
        /// </summary>
        private void AddPathNavigator()
        {
            var pathNavigator = new UserControlPathNavigator(toolStripPath);

            // Функция перехода по нажатию кнопки.
            pathNavigator.MouseClickAction =
                delegate(object sender, EventArgs args) { SetSelectedByGroupId((int)((Button)sender).Tag); };

            pathNavigator.FontChanged += delegate(object sender, EventArgs args)
            {
                pathNavigator.Update(_currentDocumentGroup);
            };

            _controlPathNavigator = pathNavigator;

            var host = new ToolStripControlHost(pathNavigator);

            toolStripPath.Items.Add(host);
        }

        #region Оптимизация/фикс ошибок элементов формы

        private const int TVM_SETEXTENDEDSTYLE = 0x1100 + 44;
        private const int TVS_EX_DOUBLEBUFFER = 0x0004;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SendMessage(IntPtr HWnd, int Msg, IntPtr Wp, IntPtr Lp);


        /// <summary>
        /// Оптимизация control'ов формы
        /// </summary>
        private void OptimizeForm()
        {
            SendMessage(treeView1.Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER,
                (IntPtr)TVS_EX_DOUBLEBUFFER);

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelInformation, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, panelUserControl, new object[] { true });

            typeof(TreeView).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, treeView1, new object[] { true });

            typeof(ListView).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, documentListView, new object[] { true });
        }

        /// <summary>
        /// Исправление ошибки. Когда TreeView скрыт, документы не показываются  
        /// </summary>
        private void TreeViewFix()
        {
            if (!BtnShowGroup.Checked)
            {
                BtnShowGroup.Checked = true;
                if (treeView1.Nodes.Count > 0)
                {
                    if (_currentDocumentGroup == null)
                        treeView1.SelectedNode = treeView1.Nodes[0];
                    else
                        SetSelectedByGroupId((int)_currentDocumentGroup.Id);
                }

                BtnShowGroup.Checked = false;
            }
        }

        #endregion

        #region Работа с реестром

        /// <summary>
        /// Сохранение состояния полей формы в регистр.
        /// </summary>
        private void SavePropertirs()
        {
            // Состояние формы
            var rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PGS\\DMS");

            if (rk == null) return;

            rk.SetValue("WindowState", WindowState.ToString());
            if (WindowState != FormWindowState.Maximized)
            {
                rk.SetValue("WindowLeft", Left);
                rk.SetValue("WindowTop", Top);
                rk.SetValue("WindowWidth", Width);
                rk.SetValue("WindowHeight", Height);
            }

            var converter = TypeDescriptor.GetConverter(typeof(Font));
            var fontString = converter.ConvertToString(Font);
            rk.SetValue("font", fontString);
            rk.SetValue("tsImageSize", toolStrip1.ImageScalingSize.Height);

            rk.SetValue("mainSplitWidth", mainSplitContainer.SplitterDistance);
            rk.SetValue("documentSplitWidth", documentSplitContainer.SplitterDistance);
            rk.SetValue("btnShowGroup", BtnShowGroup.Checked);
            rk.SetValue("btnShowDocInfo", btnShowDocInfo.Checked);
            rk.SetValue("viewStyle", (int)documentListView.View);

            rk.SetValue("isRevisionMode", btnRevisionMode.Checked);
            rk.SetValue("isRevisionHighlight", IsRevisionHighlight);
            rk.SetValue("revisionModeDay", RevisionModeDay);

            rk.SetValue("editMode", EditMode);

            // Сохранение состояние TreeView
            var rkTreeView = rk.CreateSubKey("TreeView");

            var savedExpand = new List<int>();
            savedExpand = GetExpandedGroupId(savedExpand, treeView1.Nodes);

            // Хранение в регистре поддерживает только массивы строк
            var savedExpandAsString = savedExpand.Select(x => x.ToString()).ToArray();

            rkTreeView.SetValue("expandedArray", savedExpandAsString);

            var selectedNode = ((DocumentGroup)treeView1.SelectedNode?.Tag)?.Id ?? -1;
            rkTreeView.SetValue("selectedNode", selectedNode);

            //todo: Добавить хранение состояния для Вид (ListView)
        }

        /// <summary>
        /// Загрузка состояния полей формы из регистра.
        /// </summary>
        private void LoadProperties()
        {
            try
            {
                var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\DMS");
                if (rk != null)
                {
                    // Загрузка состояния формы
                    WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState),
                        (string)rk.GetValue("WindowState", WindowState.ToString()));

                    Left = (int)rk.GetValue("WindowLeft", Left);
                    Top = (int)rk.GetValue("WindowTop", Top);
                    Width = (int)rk.GetValue("WindowWidth", Width);
                    Height = (int)rk.GetValue("WindowHeight", Height);

                    mainSplitContainer.SplitterDistance =
                        (int)rk.GetValue("mainSplitWidth", mainSplitContainer.SplitterDistance);
                    documentSplitContainer.SplitterDistance = (int)rk.GetValue("documentSplitWidth",
                        documentSplitContainer.SplitterDistance);
                    BtnShowGroup.Checked = bool.Parse((string)rk.GetValue("btnShowGroup", BtnShowGroup.Checked));
                    btnShowDocInfo.Checked = bool.Parse((string)rk.GetValue("btnShowDocInfo", btnShowDocInfo.Checked));
                    documentListView.View = (View)(int)rk.GetValue("viewStyle", documentListView.View);
                    if (documentListView.View == View.Details)
                    {
                        tsViewIcon.Checked = false;
                        tsViewTable.Checked = true;
                    }

                    btnRevisionMode.Checked = bool.Parse((string)rk.GetValue("isRevisionMode", btnRevisionMode.Checked));
                    IsRevisionHighlight = bool.Parse((string)rk.GetValue("isRevisionHighlight", IsRevisionHighlight));
                    RevisionModeDay = (int)rk.GetValue("revisionModeDay", RevisionModeDay);

                    var fontString = (string)rk.GetValue("Font", Font);
                    var converter = TypeDescriptor.GetConverter(typeof(Font));
                    SetFont((Font)converter.ConvertFromString(fontString));

                    SetToolStripImageSize((int)rk.GetValue("tsImageSize", toolStrip1.ImageScalingSize.Height));

                    var rkTreeView = rk.OpenSubKey("TreeView");
                    if (rkTreeView != null)
                    {
                        var expandedStringArray = (string[])rkTreeView.GetValue("expandedArray", new string[0]);
                        var expandedList = expandedStringArray.Select(x => int.Parse(x)).ToList();
                        SetExpandedByGroupId(expandedList, treeView1.Nodes);

                        var selectedNode = (int)rkTreeView.GetValue("selectedNode", -1);
                        SetSelectedByGroupId(selectedNode, treeView1.Nodes);
                    }

                    // Должен быть после обновления данных о сетевом соединении
                    EditMode = bool.Parse((string)rk.GetValue("editMode", EditMode));
                }
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить конфигурацию\nдля некоторых элементов.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void SetFont(Font font)
        {
            Font = font;

            foreach (Control control in Controls) RecursiveSetFont(control);

            labelDocumentName.Font = new Font(Font.FontFamily, Font.Size + 2, Font.Style);
            RefreshForm();
        }

        private void RecursiveSetFont(Control control)
        {
            control.Font = Font;

            if (control.Controls.Count > 0)
                foreach (Control subcontrol in control.Controls)
                    RecursiveSetFont(subcontrol);
        }

        public void SetToolStripImageSize(int size)
        {
            toolStrip1.AutoSize = false;
            toolStrip1.ImageScalingSize = new Size(size, size);
            toolStrip1.AutoSize = true;
        }

        #endregion

        #region Подключение к сетевым директориям

        /// <summary>
        /// Создание подключение к сетевому диску.
        /// </summary>
        /// <remarks>Если установлен флаг checkExistConnection, то сначала будет проверка на существование соединения (чтения)
        /// и только потом попытка подключения через "net use"
        /// </remarks>
        private NetworkFolder ConnectToNetworkFolder(string uncPath, NetworkCredential credential,
            bool checkExistConnection = false)
        {
            if (checkExistConnection)
                if (Directory.Exists(uncPath))
                    return new NetworkFolder(uncPath);

            try
            {
                var code = NetworkDriver.Connect(uncPath, credential);
                if (code != 0) throw new Win32Exception(code);

                return new NetworkFolder(uncPath);
            }
            catch (Win32Exception exception)
            {
                MessageBox.Show($"Не удалось подключиться к сетевому диску!\n{uncPath}\n{exception.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool CheckReadOnlyFolder()
        {
            if (_readOnlyFolder != null) return true;

            MessageBox.Show("Отсутствует подключение к сетевому диску!",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return false;
        }

        private bool CheckSuperUserFolder()
        {
            if (_superUserFolder != null) return true;

            MessageBox.Show("Отсутствует возможность редактирования на сетевом диске!\n" +
                            "(Проверьте параметры подключения)",
                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }

        #endregion

        #region Получения данных о users.users из бд

        private string GetUserById(int id)
        {
            var con = DB.Connect();
            var com = con.CreateCommand();
            com.CommandText = $"SELECT username FROM users.users WHERE id = {id}";
            try
            {
                var username = (string)com.ExecuteScalar();
                return username;
            }
            finally
            {
                con.Disconnect();
            }
        }

        private int GetUserByUsername(string username)
        {
            var con = DB.Connect();
            var com = con.CreateCommand();
            com.CommandText = $"SELECT id FROM users.users WHERE username = {username.ToDBString()}";
            try
            {
                var id = (int)com.ExecuteScalar();
                return id;
            }
            finally
            {
                con.Disconnect();
            }
        }

        #endregion

        #region TreeView (Группы документов)

        /// <summary>
        /// Сохраняет состояние дерева TreeView.
        /// </summary>
        /// <param name="listWithExpandedIds">Лист в который наполняем DocumentGroup.Id</param>
        /// <param name="nodes">Для рекурсии.</param>
        private List<int> GetExpandedGroupId(List<int> listWithExpandedIds, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.IsExpanded)
                    if ((node.Tag as DocumentGroup)?.Id != null)
                        listWithExpandedIds.Add((int)(node.Tag as DocumentGroup)?.Id);

                if (node.Nodes.Count > 0)
                    GetExpandedGroupId(listWithExpandedIds, node.Nodes);
            }

            return listWithExpandedIds;
        }

        /// <summary>
        /// Восстанавливает состояние дерева TreeView
        /// </summary>
        /// <param name="listWithExpandedIds">Лист с DocumentGroup.Id</param>
        /// <param name="nodes">Для рекурсии.</param>
        private void SetExpandedByGroupId(List<int> listWithExpandedIds, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                var groupId = (node.Tag as DocumentGroup)?.Id;
                if (groupId != null && listWithExpandedIds.Contains((int)groupId)) node.Expand();

                if (node.Nodes.Count > 0)
                    SetExpandedByGroupId(listWithExpandedIds, node.Nodes);
            }
        }

        /// <summary>
        /// Выбирает объект TreeNode по DocumentGroup.Id
        /// </summary>
        /// <param name="id">DocumentGroup.Id</param>
        /// <param name="nodes">Для рекурсии.</param>
        private void SetSelectedByGroupId(int id, TreeNodeCollection nodes = null)
        {
            if (nodes == null)
                nodes = treeView1.Nodes;
            foreach (TreeNode node in nodes)
            {
                if ((node.Tag as DocumentGroup)?.Id == id)
                {
                    treeView1.SelectedNode = node;
                    _currentDocumentGroup = node.Tag as DocumentGroup;
                    node.Expand();

                    return;
                }

                if (node.Nodes.Count > 0)
                    SetSelectedByGroupId(id, node.Nodes);
            }
        }

        /// <summary>
        /// Заполняет TreeView.
        /// </summary>
        private void UpdateTreeView()
        {
            treeView1.Nodes.Clear();

            List<DocumentGroup> documentGroups;

            // Пытаемся загрузить группы из бд
            try
            {
                documentGroups = DocumentGroup.LoadDocumentGroups(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить документы!\n{ex}",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (var group in documentGroups) treeView1.Nodes.Add(CreateNodes(group));

            if (_currentDocumentGroup?.Id != null)
                SetSelectedByGroupId((int)_currentDocumentGroup.Id, treeView1.Nodes);
        }

        private void UpdateTreeView(List<Document> documents)
        {
            var savedExpand = new List<int>();
            GetExpandedGroupId(savedExpand, treeView1.Nodes);
            treeView1.Nodes.Clear();

            foreach (var document in documents)
            {
                // Создаём цепочку групп
                var groups = new List<DocumentGroup>();
                var headDocumentGroup = document.Group;
                while (headDocumentGroup != null)
                {
                    groups.Add(headDocumentGroup);
                    headDocumentGroup = headDocumentGroup.Parent;
                }

                groups.Reverse();

                var headNodes = treeView1.Nodes;
                // Проходимся по цепочке и создаём недостающие узлы
                foreach (var group in groups)
                {
                    // Если нету - создаём
                    if (headNodes.OfType<TreeNode>().All(x => (x.Tag as DocumentGroup)?.Id != group.Id))
                        headNodes.Add(CreateNode(group));
                    // Меняем указатель на уровень ниже
                    headNodes = headNodes.OfType<TreeNode>().First(x => (x.Tag as DocumentGroup)?.Id == group.Id).Nodes;
                }
            }

            SetExpandedByGroupId(savedExpand, treeView1.Nodes);
        }

        /// <summary>
        /// Воссоздаёт дерево групп.
        /// </summary>
        private TreeNode CreateNodes(DocumentGroup documentGroup)
        {
            var tn = CreateNode(documentGroup);

            foreach (var subgroup in documentGroup.DocumentSubgroups)
                tn.Nodes.Add(CreateNodes(subgroup));

            return tn;
        }

        /// <summary>
        /// Добавляет один узел.
        /// </summary>
        /// <returns></returns>
        private TreeNode CreateNode(DocumentGroup documentGroup)
        {
            var tn = new TreeNode
            {
                Text = documentGroup.Name,
                Tag = documentGroup
            };

            if (IsRevisionHighlight || _isRevisionMode)
            {
                var state = GetGroupRevisionState((int)documentGroup.Id);
                switch (state)
                {
                    case RevisionState.Overdue:
                        tn.BackColor = REVISION_MODE_RED;
                        break;
                    case RevisionState.Approaching:
                        tn.BackColor = REVISION_MODE_YELLOW;
                        break;
                    default:
                        break;
                }
            }

            return tn;
        }

        private enum RevisionState
        {
            Normal = 0, // Всё впорядке
            Approaching = 1, // Приближается окончание
            Overdue = 2 // Просрочен
        }

        /// <summary>
        /// Возвращает состояние ревизии для документов привязанных к группе.
        /// </summary>
        private RevisionState GetGroupRevisionState(int documentGroupId)
        {
            var con = DB.Connect();

            var query = $@"
WITH tmp AS (
    SELECT CASE
        WHEN current_date >= date_revision_next THEN '2'
        WHEN current_date + interval '{DB.ToDBString(RevisionModeDay)} days' >= date_revision_next THEN '1'
        ELSE '0'
        END AS date_revision_next FROM 
        (
            WITH RECURSIVE tmpp AS 
            (
              SELECT id, parentid FROM dms.doc_groups WHERE id = {DB.ToDBString(documentGroupId)}
                UNION ALL
              SELECT e.id, e.parentid FROM tmpp t
              INNER JOIN dms.doc_groups e ON e.parentid = t.id
            ) 
            SELECT * FROM dms.docs WHERE docs.groupid in (SELECT id FROM tmpp)
        ) as docs
    GROUP BY date_revision_next
)
SELECT CASE
    WHEN EXISTS(SELECT * FROM tmp WHERE date_revision_next = '2') THEN '2'
    WHEN EXISTS(SELECT * FROM tmp WHERE date_revision_next = '1') THEN '1'
    ELSE '0'
END
";
            try
            {
                var com = con.CreateCommand();
                com.CommandText = query;
                var state = (string)com.ExecuteScalar();
                return (RevisionState)int.Parse(state);
            }
            finally
            {
                con.Disconnect();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            // Заполняем ListView.
            var dg = e.Node.Tag as DocumentGroup;
            _currentDocumentGroup = dg;

            if (dg != null)
            {
                if (!_isUserText)
                    UpdateListView((int)dg.Id);
                else
                    SearchDocument();
            }

            // Обновление UserControlPathNavigator.
            _controlPathNavigator.Update(dg);

            // Заполнение стека навигации.
            if (dg != null)
            {
                PathBackStack.Push((int)dg.Id);
                tsBtnPathBack.Enabled = PathBackStack.Count > 1;
            }

            tsBtnPathUp.Enabled = dg?.Parent != null;
            tsSoloBtnUp.Enabled = dg?.Parent != null;

            // Меняем название в строке поиска.
            _initialText = _currentDocumentGroup == null ? "Поиск" : $"Поиск в: {_currentDocumentGroup.Name}";
            if (!_isUserText && !tbSearch.Focused)
                tbSearch.Text = _initialText;
        }

        private void treeView1_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            panelUserControl.Controls.Clear();
            labelDocumentName.Text = "";

            var dg = e.Node.Tag as DocumentGroup;
            if (dg?.Id != null)
            {
                PathForwardStack.Push((int)dg.Id);
                tsBtnPathForward.Enabled = PathForwardStack.Count > 1;
            }
        }

        #endregion

        #region Создание/изменение/удаление групп документов

        #region В TreeView

        // Создание нового узла группы
        private void tsGroupCreate_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            var documentGroup = new DocumentGroup();

            var groupName = "Новая группа";

            // Родительская группа
            var parent = treeView1.SelectedNode?.Tag as DocumentGroup;
            if (parent != null)
            {
                documentGroup.Parent = parent;
                parent.DocumentSubgroups.Add(documentGroup);
            }

            // Название | "Новая группа (3)"
            if (ValidateGroupName(documentGroup, groupName, false))
            {
                documentGroup.Name = groupName;
            }
            else
            {
                var i = 1;
                groupName = $"Новая группа ({i})";
                while (!ValidateGroupName(documentGroup, $"Новая группа ({i})", false))
                {
                    i++;
                    if (i > 100)
                    {
                        MessageBox.Show("Не удалось создать группу!\nОшибка создания названия.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    groupName = $"Новая группа ({i})";
                }

                documentGroup.Name = groupName;
            }

            if (CreateGroup(documentGroup))
            {
                var node = new TreeNode
                {
                    Tag = documentGroup,
                    Text = documentGroup.Name
                };

                if (treeView1.SelectedNode == null)
                {
                    treeView1.Nodes.Add(node);
                }
                else
                {
                    treeView1.SelectedNode.Expand();
                    treeView1.SelectedNode.Nodes.Add(node);

                    treeView1.SelectedNode = node;
                }

                treeView1.LabelEdit = true;
                node.BeginEdit();
            }
        }

        // Переименовка узла группы
        private void tsGroupRename_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            if (treeView1.SelectedNode == null || treeView1.SelectedNode == treeView1.Nodes[0])
            {
                MessageBox.Show("Выберите группу", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var node = treeView1.SelectedNode;
            if ((node.Tag as DocumentGroup)?.Id == null)
            {
                MessageBox.Show("Ошибка! Невозможно переименовать группу.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!node.IsEditing)
            {
                treeView1.LabelEdit = true;
                node.BeginEdit();
            }

            //UpdateTreeView();
            //UpdateListView((node.Tag as DocumentGroup).I d);
        }

        // Создание новой записи/сохрание группы документов
        private void treeView1_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            var node = e.Node;
            var documentGroup = node.Tag as DocumentGroup;
            if (documentGroup?.Id == null)
            {
                e.CancelEdit = true;
                return;
            }

            treeView1.LabelEdit = false;
            if (e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }

            var newName = e.Label.Trim(); // Удаляем пробелы

            if (documentGroup.Name == newName || !ValidateGroupName(documentGroup, newName))
            {
                e.CancelEdit = true;
                return;
            }

            var oldName = documentGroup.Name;
            var oldPath = documentGroup.FullPath;

            documentGroup.Name = newName;

            if (RenameGroup(documentGroup, oldPath))
                RefreshForm(false);
            else
                documentGroup.Name = oldName;
        }

        // Удаление узла группы
        private void tsGroupDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            if (treeView1.SelectedNode == null || treeView1.SelectedNode == treeView1.Nodes[0])
            {
                MessageBox.Show("Выберите группу документов!",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var documentGroup = treeView1.SelectedNode.Tag as DocumentGroup;
            if (documentGroup?.Id == null) return;

            if (MessageBox.Show($"Удалить группу \"{documentGroup.Name}\"?\n",
                    "Удаление группы", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2)
                != DialogResult.Yes)
                return;

            if (DeleteGroup(documentGroup)) RefreshForm(false);
        }

        /// <summary>
        /// Проверка нового названия группы на оригинальность и корректность (Непустая, без пробелов).
        /// </summary>
        private bool ValidateGroupName(DocumentGroup documentGroup, string newGroupName, bool showMessageBox = true)
        {
            if (string.IsNullOrWhiteSpace(newGroupName))
                return false;

            if (documentGroup?.Parent == null)
            {
                if (treeView1.Nodes.Cast<TreeNode>().Any(node => node.Text == newGroupName))
                {
                    if (showMessageBox)
                        MessageBox.Show("Название групп не может быть одинаковым!",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            else
            {
                if (documentGroup.Parent.DocumentSubgroups.Exists(group => group.Name == newGroupName))
                {
                    if (showMessageBox)
                        MessageBox.Show("Название групп не может быть одинаковым!",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region В ListView

        private void tsSoloBtnAddGroup_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            if (_currentDocumentGroup == null)
            {
                MessageBox.Show("Не удалось создать группу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var documentGroup = new DocumentGroup();
            var groupName = "Новая группа";

            // Родительская группа
            documentGroup.Parent = _currentDocumentGroup;
            _currentDocumentGroup.DocumentSubgroups.Add(documentGroup);

            // Название | "Новая группа (3)"
            if (ValidateGroupName(documentGroup, groupName, false))
            {
                documentGroup.Name = groupName;
            }
            else
            {
                var i = 1;
                groupName = $"Новая группа ({i})";
                while (!ValidateGroupName(documentGroup, $"Новая группа ({i})", false))
                {
                    i++;
                    if (i > 100)
                    {
                        MessageBox.Show("Не удалось создать группу!\nОшибка создания названия.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    groupName = $"Новая группа ({i})";
                }

                documentGroup.Name = groupName;
            }

            if (CreateGroup(documentGroup))
            {
                var item = new ListViewItem(documentGroup.Name)
                {
                    Tag = documentGroup,
                    ImageIndex = GetImageByFileFormat("folder")
                };

                documentListView.Items.Add(item);
                UpdateTreeView();

                item.BeginEdit();
            }
        }

        private void tsGroupListViewOpen_Click(object sender, EventArgs e)
        {
            if (documentListView.SelectedItems.Count != 1)
                return;

            var documentGroup = documentListView.SelectedItems[0].Tag as DocumentGroup;
            if (documentGroup == null)
                return;

            documentListView_DoubleClick(null, null);
        }

        private void tsGroupListViewRename_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            if (documentListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите группу", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (documentListView.SelectedItems.Count > 1)
            {
                MessageBox.Show("Выберите одну группу", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var doc = documentListView.SelectedItems[0].Tag as DocumentGroup;
            if (doc == null) return;
            documentListView.SelectedItems[0].BeginEdit();
        }

        private void tsGroupListViewDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            if (documentListView.SelectedItems.Count != 1)
                return;

            var documentGroup = documentListView.SelectedItems[0].Tag as DocumentGroup;

            if (documentGroup?.Id == null) return;

            if (MessageBox.Show($"Удалить группу \"{documentGroup.Name}\"?\n",
                    "Удаление группы", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2)
                != DialogResult.Yes)
                return;

            if (DeleteGroup(documentGroup))
            {
                // Обновление TreeView и ListView
                UpdateTreeView();
                UpdateListView(_currentDocumentGroup.Id);
            }
        }

        #endregion

        /// <summary>
        /// Создание новой группы документов.
        /// </summary>
        /// <remarks>Новая группа создаётся с названием "Новая группа".</remarks>
        private bool CreateGroup(DocumentGroup documentGroup)
        {
            var connection = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = connection.BeginTransaction();

                // Сохранение в бд
                documentGroup.Save(transaction);

                // Создание на сетевом диске
                Directory.CreateDirectory(_superUserFolder.CombinePath(documentGroup.FullPath));

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show($"Не удалось создать новую группу !\n{ex}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Изменение названия группы документов.
        /// </summary>
        /// <param name="documentGroup"></param>
        /// <param name="oldRelativePath">Прежний относительный путь к директории.</param>
        /// <returns></returns>
        private bool RenameGroup(DocumentGroup documentGroup, string oldRelativePath)
        {
            var connection = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = connection.BeginTransaction();

                // Изменение в бд
                documentGroup.Save(transaction);

                // Изменение на сетевом диске
                if (Directory.Exists(_superUserFolder.CombinePath(oldRelativePath)))
                    Directory.Move(
                        _superUserFolder.CombinePath(oldRelativePath),
                        _superUserFolder.CombinePath(documentGroup.FullPath)
                    );
                else
                    Directory.CreateDirectory(_superUserFolder.CombinePath(documentGroup.FullPath));

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                MessageBox.Show($"Не удалось переименовать группу!\n{ex}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Удаление группы документов.
        /// </summary>
        private bool DeleteGroup(DocumentGroup documentGroup)
        {
            var connection = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                // Удаление в бд
                transaction = connection.BeginTransaction();
                DocumentGroup.DeleteDocumentGroups((int)documentGroup.Id, transaction);

                // Удаление документов и файлов
                FileManager.DeleteDirectory(_superUserFolder, documentGroup);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                connection.Disconnect();
                MessageBox.Show($"При удалении возникла ошибка!\n{ex}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        #endregion

        #region ListView (Документы)

        /// <summary>
        /// Заполняет элемент ListView документами переданной группой документов.
        /// </summary>
        private void UpdateListView(int? documentGroupId)
        {
            documentListView.Items.Clear();

            if (documentGroupId == null)
                return;

            DocumentGroup documentGroup;

            // Пытаемся загрузить группы с документами из бд
            try
            {
                documentGroup = DocumentGroup.LoadDocumentGroup((int)documentGroupId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось загрузить документы!\n{ex}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (documentGroup == null)
                MessageBox.Show($"Не удалось загрузить документы!\n DocumentGroupId: {documentGroupId}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Формирование коллекции ListView

            // Заполнение подгруппами
            foreach (var subgroup in documentGroup.DocumentSubgroups)
            {
                var item = new ListViewItem(subgroup.Name)
                {
                    Tag = subgroup
                };

                if (subgroup.Documents.Count > 0)
                    item.ImageIndex = GetImageByFileFormat("folderWithData");
                else if (subgroup.DocumentSubgroups.Count > 0)
                    item.ImageIndex = GetImageByFileFormat("folderWithFolders");
                else
                    item.ImageIndex = GetImageByFileFormat("folder");
                documentListView.Items.Add(item);
            }

            // Заполнение документами.
            foreach (var document in documentGroup.Documents)
            {
                if (_isRevisionMode && (document.DateNextRevision == null || 
                                        IsApproachingOrOverdue(document.DateNextRevision) == RevisionState.Normal))
                    continue;

                documentListView.Items.Add(CreateItem(document));
            }
        }

        /// <summary>
        /// Заполняет элемент ListView документами переданным списком.
        /// </summary>
        private void UpdateListView(List<Document> documents)
        {
            documentListView.Items.Clear();

            // Заполнение документами.
            foreach (var document in documents)
            {
                if (_isRevisionMode && (document.DateNextRevision == null ||
                                        IsApproachingOrOverdue(document.DateNextRevision) == RevisionState.Normal))
                    continue;
                documentListView.Items.Add(CreateItem(document));
            }
        }

        private RevisionState IsApproachingOrOverdue(DateTime? dateNextRevision)
        {
            if (dateNextRevision != null)
            {
                if (dateNextRevision <= DateTime.Now)
                    return RevisionState.Overdue;
                if (dateNextRevision <= DateTime.Now.AddDays(RevisionModeDay))
                    return RevisionState.Approaching;
            }

            return RevisionState.Normal;
        }

        /// <summary>
        /// Формирование ListViewItem.
        /// </summary>
        private ListViewItem CreateItem(Document document)
        {
            var item = new ListViewItem(document.Name)
            {
                Tag = document,
                ImageIndex = GetImageByFileFormat(
                    document.Files.Count == 0
                        ? "default"
                        : document.Files.Last(x => x.ReplacedTo == null).Format
                ),
                ToolTipText = Path.Combine(document.Group.FullPath, document.Name)
            };

            item.SubItems.Add(document.Comment);


            var z = DateTime.Now.AddDays(RevisionModeDay);

            if (IsRevisionHighlight || _isRevisionMode)
            {
                var revisionState = IsApproachingOrOverdue(document.DateNextRevision);
                switch (revisionState)
                {
                    case RevisionState.Overdue:
                        item.BackColor = REVISION_MODE_RED;
                        break;
                    case RevisionState.Approaching:
                        item.BackColor = REVISION_MODE_YELLOW;
                        break;
                    default:
                        break;
                }
            }

            return item;
        }

        private void SelectListViewItemByDocId(int id)
        {
            foreach (ListViewItem item in documentListView.Items)
            {
                var doc = item.Tag as Document;
                if (doc?.Id == id)
                    item.Selected = true;
            }
        }

        #region Словарь сопоставляющий формат файла с иконкой

        /// <summary>
        /// Словарь сопоставляющий формат файла с иконкой.
        /// </summary>
        private static readonly Dictionary<string, string> LinksFileFormatWithImageName =
            new Dictionary<string, string>()
            {
                { "doc", "word.png" },
                { "docx", "word.png" },
                { "dot", "word.png" },
                { "dotx", "word.png" },
                { "sys", "word.png" },

                { "accdb", "access.png" },
                { "accde", "access.png" },
                { "accdr", "access.png" },

                { "xlam", "excel.png" },
                { "xla", "excel.png" },
                { "xll", "excel.png" },
                { "xlm", "excel.png" },
                { "xls", "excel.png" },
                { "xlsm", "excel.png" },
                { "xlsx", "excel.png" },
                { "xlt", "excel.png" },
                { "xltm", "excel.png" },
                { "xltx", "excel.png" },

                { "ppt", "power-point.png" },
                { "pptx", "power-point.png" },
                { "pps", "power-point.png" },

                { "avi", "video.png" },
                { "mpg", "video.png" },
                { "mpeg", "video.png" },
                { "mov", "video.png" },

                { "bat", "cmd.png" },

                { "bmp", "image.png" },
                { "gif", "image.png" },
                { "jpg", "image.png" },
                { "jpeg", "image.png" },
                { "png", "image.png" },
                { "ico", "image.png" },

                { "exe", "exe.png" },

                { "mp3", "music.png" },
                { "wav", "music.png" },

                { "pdf", "pdf.png" },

                { "txt", "txt.png" },

                { "ini", "conf.png" },
                { "dll", "conf.png" },

                { "folder", "empty-folder.png" },
                { "folderWithData", "data-folder.png" },
                { "folderWithFolders", "folder-folder.png" },

                { "default", "default.png" }
            };

        #endregion

        /// <summary>
        /// Сопостовляет формат файла и id в imageListIcons.
        /// </summary>
        /// <param name="fileFormat"></param>
        /// <returns></returns>
        private int GetImageByFileFormat(string fileFormat)
        {
            return imageListIcons.Images.IndexOfKey(LinksFileFormatWithImageName.ContainsKey(fileFormat)
                ? LinksFileFormatWithImageName[fileFormat]
                : LinksFileFormatWithImageName["default"]);
        }

        private void documentListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete) BtnRemove_Click(null, null);
            if (e.KeyCode == Keys.C && e.Control) 
                tsMenuCopy_Click(null, null);
        }

        #endregion

        #region Создание/изменение/удаление документов

        /// <summary>
        /// Удаление выбранных документов.
        /// </summary>
        private bool RemoveDocument(List<Document> documents)
        {
            foreach (var doc in documents)
            {
                var con = DB.Connect();
                OdbcTransaction tr = null;
                try
                {
                    tr = con.BeginTransaction();
                    // Удаление записей в бд
                    Document.DeleteDocument((int)doc.Id, tr);

                    // Удаление файлов с диска
                    FileManager.DeleteFiles(_superUserFolder, doc);

                    foreach (var file in doc.Files)
                    {
                        var path = SuperUserFolder.CombinePath(doc.Group.FullPath, file.NetworkName);
                        var fi = new FileInfo(path);
                        if (fi.Exists)
                            fi.Delete();
                    }

                    tr.Commit();
                }
                catch (Exception ex)
                {
                    tr?.Rollback();
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                finally
                {
                    con.Disconnect();
                }
            }

            return true;
        }

        #endregion

        #region Поиск

        private bool _isUserText = false;
        private string _initialText = "Поиск";

        private void tbSearch_Enter(object sender, EventArgs e)
        {
            if (!_isUserText)
            {
                _isUserText = true;
                tbSearch.Text = "";
                tbSearch.ForeColor = SystemColors.WindowText;
            }
        }

        private void tbSearch_Leave(object sender, EventArgs e)
        {
            if (!_isUserText)
            {
                tbSearch.Text = _initialText;
                tbSearch.ForeColor = SystemColors.AppWorkspace;
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            _isUserText = !string.IsNullOrWhiteSpace(tbSearch.Text) && tbSearch.Text != _initialText;
            tsBtnClearSearch.Enabled = _isUserText;
        }

        private void tsBtnSearch_Click(object sender, EventArgs e)
        {
            SearchDocument();
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchDocument();
            }
        }

        private void tsBtnClearSearch_Click(object sender, EventArgs e)
        {
            tbSearch.Clear();
            tbSearch.Text = _initialText;
            tbSearch.ForeColor = SystemColors.AppWorkspace;
            SearchDocument();
        }

        /// <summary>
        /// Поиск документов.
        /// </summary>
        private void SearchDocument()
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text) || tbSearch.Text == _initialText)
            {
                RefreshForm(false);
                return;
            }

            var documents = GetDocumentsBySearchString(tbSearch.Text, (int)_currentDocumentGroup.Id);
            UpdateListView(documents);
            UpdateTreeView(documents);
            treeView1.ExpandAll();
        }

        /// <summary>
        /// Возвращает список документов, удовлетворяющих регулярной строке.
        /// </summary>
        private List<Document> GetDocumentsBySearchString(string searchString, int groupId)
        {
            var docList = new List<Document>();

            var query = $@"
WITH RECURSIVE tmp AS (
  SELECT id, parentid FROM dms.doc_groups WHERE id = {DB.ToDBString(groupId)}
    UNION ALL
  SELECT e.id, e.parentid FROM tmp t
  INNER JOIN dms.doc_groups e ON e.parentid = t.id
) SELECT * FROM dms.docs WHERE docs.groupid IN (SELECT id FROM tmp) AND docs.name ~* {searchString.ToDBString()}
";

            var con = DB.Connect();

            try
            {
                var com = con.CreateCommand();
                com.CommandText = query;

                var reader = com.ExecuteReader();
                while (reader.Read())
                {
                    var doc = Document.LoadDocument((int)reader["id"], con, false);
                    docList.Add(doc);
                }
            }
            finally
            {
                con.Disconnect();
            }

            return docList;
        }

//        private List<Document> GetDocumentRevisionMode()
//        {
//            var docList = new List<Document>();

//            var query = $@"
//WITH RECURSIVE tmp AS (
//  SELECT id, parentid FROM dms.doc_groups WHERE id = {DB.ToDBString(groupId)}
//    UNION ALL
//  SELECT e.id, e.parentid FROM tmp t
//  INNER JOIN dms.doc_groups e ON e.parentid = t.id
//) SELECT * FROM dms.docs WHERE docs.groupid IN (SELECT id FROM tmp) AND docs.name ~* {searchString.ToDBString()}
//";

//            var con = DB.Connect();

//            try
//            {
//                var com = con.CreateCommand();
//                com.CommandText = query;

//                var reader = com.ExecuteReader();
//                while (reader.Read())
//                {
//                    var doc = Document.LoadDocument((int)reader["id"]);
//                    docList.Add(doc);
//                }
//            }
//            finally
//            {
//                con.Disconnect();
//            }

//            return docList;
//        }

        #endregion

        #region Работа с элементами формы

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.F2 | Keys.Control))
            {
                if (DB.doLogin() == DialogResult.OK) EditMode = true;
            }
            else if (e.KeyData == Keys.F2)
            {
                EditMode = !EditMode;
            }
            else if (e.KeyCode == Keys.F5)
            {
                RefreshForm(false);
            }
        }

        // Копирование файла в буфер обмена
        private void tsMenuCopy_Click(object sender, EventArgs e)
        {
            System.Collections.Specialized.StringCollection replacementList = new System.Collections.Specialized.StringCollection();

            foreach (ListViewItem item in documentListView.SelectedItems)
            {
                if (!(item.Tag is Document))
                    continue;

                var doc = (Document)item.Tag;
                var path =
                    ReadOnlyFolder.CombinePath(doc.Group.FullPath,
                        doc.Files.Last(x => x.ReplacedTo == null).NetworkName);

                replacementList.Add(path);
            }

            Clipboard.SetFileDropList(replacementList);
        }

        #region Document

        /// <summary>
        /// Кнопка создания документа
        /// </summary>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            var fde = new FormDocumentEdit(_superUserFolder, _currentDocumentGroup);
            fde.Font = Font;
            fde.EditMode = EditMode;

            try
            {
                if (fde.ShowDialog() == DialogResult.Yes)
                {
                }

                UpdateListView(_currentDocumentGroup?.Id);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Создание документа не удалось!\n{exception.Message}",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Кнопка редактирования документа
        /// </summary>
        private void BtnRename_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            if (documentListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите документ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (documentListView.SelectedItems.Count > 1)
            {
                MessageBox.Show("Выберите один документ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var doc = documentListView.SelectedItems[0].Tag as Document;
            if (doc == null) return;
            documentListView.SelectedItems[0].BeginEdit();
        }

        // Событие после ввода нового названия.
        private void documentListView_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (documentListView.Items[e.Item].Tag is Document)
            {
                var doc = documentListView.Items[e.Item].Tag as Document;
                //var item = documentListView.Items[e.Item];

                if (doc == null || doc.Name == e.Label || string.IsNullOrWhiteSpace(e.Label))
                {
                    e.CancelEdit = true;
                    return;
                }

                if (doc.Group.Documents.Exists(x => x.Name == e.Label))
                {
                    MessageBox.Show($"Документ {e.Label} уже существует в группе!",
                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    e.CancelEdit = true;
                    return;
                }

                try
                {
                    doc.Name = e.Label.Trim();
                    doc.Save();
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Не удалось переименовать документ!\n{exception.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.CancelEdit = true;
                }
            }
            else if (documentListView.Items[e.Item].Tag is DocumentGroup)
            {
                var documentGroup = documentListView.Items[e.Item].Tag as DocumentGroup;

                if (documentGroup?.Id == null)
                {
                    e.CancelEdit = true;
                    return;
                }

                treeView1.LabelEdit = false;
                if (e.Label == null)
                {
                    e.CancelEdit = true;
                    return;
                }

                var newName = e.Label.Trim(); // Удаляем пробелы

                if (documentGroup.Name == newName || !ValidateGroupName(documentGroup, newName))
                {
                    e.CancelEdit = true;
                    return;
                }

                var oldName = documentGroup.Name;
                var oldPath = documentGroup.FullPath;

                documentGroup.Name = newName;

                if (RenameGroup(documentGroup, oldPath))
                {
                    UpdateTreeView();
                    UpdateListView(_currentDocumentGroup.Id);
                }
                else
                {
                    documentGroup.Name = oldName;
                }
            }
        }

        /// <summary>
        /// Кнопка удаления документа
        /// </summary>
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (!CheckSuperUserFolder())
                return;

            if (documentListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите документ или группу", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var documents = new List<Document>();
            var documentGroups = new List<DocumentGroup>();

            var message = new StringBuilder("Вы действительно хотите удалить:\n");
            foreach (ListViewItem item in documentListView.SelectedItems)
                if (item.Tag is Document)
                {
                    var doc = item.Tag as Document;
                    if (doc?.Id == null) continue;
                    documents.Add(doc);
                    message.AppendLine(" документ: " + doc.Name);
                }
                else if (item.Tag is DocumentGroup)
                {
                    var dg = item.Tag as DocumentGroup;
                    if (dg?.Id == null) continue;
                    documentGroups.Add(dg);
                    message.AppendLine(" группу: " + dg.Name);
                }

            if (MessageBox.Show(message.ToString(), "Подтверждение",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
                != DialogResult.Yes)
                return;

            if (documents.Count > 0)
                RemoveDocument(documents);

            if (documentGroups.Count > 0)
                foreach (var dg in documentGroups)
                    DeleteGroup(dg);

            if (documents.Count > 0 || documentGroups.Count > 0)
                UpdateListView(_currentDocumentGroup?.Id);
        }

        private void tsDocProperties_Click(object sender, EventArgs e)
        {
            if (documentListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите документ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (documentListView.SelectedItems.Count > 1)
            {
                MessageBox.Show("Выберите один документ", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (documentListView.SelectedItems[0].Tag is DocumentGroup) documentListView.SelectedItems[0].BeginEdit();

            if (!(documentListView.SelectedItems[0].Tag is Document))
                return;

            var document = (Document)documentListView.SelectedItems[0].Tag;

            FormDocumentEdit fde;
            if (!EditMode || _superUserFolder == null)
            {
                fde = new FormDocumentEdit(_readOnlyFolder, document);
                fde.Font = Font;
                fde.EditMode = false;
            }
            else
            {
                fde = new FormDocumentEdit(_superUserFolder, document);
                fde.Font = Font;
                fde.EditMode = EditMode;
            }

            if (fde.ShowDialog() == DialogResult.Yes)
            {
                UpdateListView(_currentDocumentGroup.Id);
                SelectListViewItemByDocId((int)document.Id);
            }
        }

        // Открытие на двойной клик
        private void documentListView_DoubleClick(object sender, EventArgs e)
        {
            if (documentListView.SelectedItems.Count != 1) return;

            if (documentListView.SelectedItems[0].Tag is Document)
            {
                try
                {
                    var doc = documentListView.SelectedItems[0].Tag as Document;
                    if (doc.Files.Count == 0) // Если с документом не связан ни один файл.
                    {
                        tsDocProperties_Click(null, null);
                    }
                    else
                    {
                        if (!CheckReadOnlyFolder())
                            return;

                        var path =
                            ReadOnlyFolder.CombinePath(doc.Group.FullPath,
                                doc.Files.Last(x => x.ReplacedTo == null).NetworkName);

                        FileManager.Open(path);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть файл\n{ex.Message}",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (documentListView.SelectedItems[0].Tag is DocumentGroup)
            {
                var group = documentListView.SelectedItems[0].Tag as DocumentGroup;
                SetSelectedByGroupId((int)group.Id, treeView1.Nodes);
                UpdateListView((int)group.Id);
            }
        }

        private void documentListView_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            panelUserControl.Controls.Clear();
            labelDocumentName.Text = "";

            if (documentListView.SelectedItems.Count != 1)
                return;

            var doc = documentListView.SelectedItems[0].Tag as Document;
            if (doc == null)
            {
                panelUserControl.Controls.Clear();
                labelDocumentName.Text = "";
                return;
            }


            var documentControl = new UserControlDocumentEdit(doc);
            documentControl.Dock = DockStyle.Fill;

            panelUserControl.Controls.Add(documentControl);
            labelDocumentName.Text = doc.Name;
        }

        #endregion

        private void BtnShowGroup_CheckedChanged(object sender, EventArgs e)
        {
            mainSplitContainer.Panel1Collapsed = !BtnShowGroup.Checked;
        }

        /// <summary>
        /// Убирает select со всех остальных.
        /// </summary>
        private void UncheckedOtherViewBtn(ToolStripMenuItem selectedMenuItem)
        {
            foreach (ToolStripMenuItem item in selectedMenuItem.Owner.Items) item.Checked = false;

            selectedMenuItem.Checked = true;
        }

        private void tsViewTable_Click(object sender, EventArgs e)
        {
            documentListView.View = View.Details;
            UncheckedOtherViewBtn((ToolStripMenuItem)sender);
        }

        private void tsViewIcon_Click(object sender, EventArgs e)
        {
            documentListView.View = View.LargeIcon;
            UncheckedOtherViewBtn((ToolStripMenuItem)sender);
        }

        private void tsViewList_Click(object sender, EventArgs e)
        {
            documentListView.View = View.List;
            UncheckedOtherViewBtn((ToolStripMenuItem)sender);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var settingForm = new FormDmsSetting(this);
            settingForm.Font = Font;
            settingForm.EditMode = EditMode;
            settingForm.ShowDialog();
        }

        // Выбираем какое контекстное меню будет отображено для документов
        private void documentListView_MouseDown(object sender, MouseEventArgs e)
        {
            // Актуально, т.к. item в ListView мы не можем задать свойство ContextMenu
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = documentListView.HitTest(e.X, e.Y);

                if (hitTestInfo.Item != null)
                {
                    if (hitTestInfo.Item.Tag is Document)
                        contextMenuDocument.Show(this, PointToClient(MousePosition));
                    if (hitTestInfo.Item.Tag is DocumentGroup)
                        contextMenuGroupInListView.Show(this, PointToClient(MousePosition));
                }
                else
                {
                    contextMenuListView.Show(this, PointToClient(MousePosition));
                }
            }
        }

        // Выбираем какое контекстное меню будет отображено для группы
        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            /* В случае с TreeView мы можем навесить на каждый TreeNode параметр ContextMenu,
             но сделал через обработку ПКМ, чтобы не отличалось от ListView +
             + при нажатии ПКМ не меняется SelectedNode => надо обрабатывать и менять 
             */
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = treeView1.HitTest(e.X, e.Y);

                if (hitTestInfo.Node != null && (hitTestInfo.Location == TreeViewHitTestLocations.Label ||
                                                 hitTestInfo.Location == TreeViewHitTestLocations.Image))
                {
                    if (hitTestInfo.Node == treeView1.Nodes[0])
                    {
                        treeView1.SelectedNode = hitTestInfo.Node;
                        contextMenuTreeView.Show(this, PointToClient(MousePosition));
                    }
                    else
                    {
                        treeView1.SelectedNode = hitTestInfo.Node;
                        contextMenuGroups.Show(this, PointToClient(MousePosition));
                    }
                }
                else
                {
                    if (treeView1.Nodes.Count == 0) return;
                    treeView1.SelectedNode = treeView1.Nodes[0];
                    contextMenuTreeView.Show(this, PointToClient(MousePosition));
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                var hitTestInfo = treeView1.HitTest(e.X, e.Y);

                if (hitTestInfo.Node == null)
                    treeView1.SelectedNode = null;
                else
                    PathForwardStack.Clear();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }

        /// <summary>
        /// Обновление элементов формы.
        /// </summary>
        /// <param name="tryConnect">Если отсутствуют соединения - пытаемся подключить</param>
        public void RefreshForm(bool tryConnect = true)
        {
            if (tryConnect && _readOnlyFolder == null)
            {
                _readOnlyFolder = ConnectToNetworkFolder(UncReadOnlyPath, ReadOnlyCredential, true);

                if (EditMode && _readOnlyFolder != null && _superUserFolder == null)
                    _superUserFolder = ConnectToNetworkFolder(UncSuperUserPath, SuperUserCredential, true);
            }

            PathBackStack.Clear();
            PathForwardStack.Clear();

            if (!_isUserText)
            {
                UpdateTreeView();
                
            }
            else
            {
                SearchDocument();
            }
        }

        private void btnShowDocInfo_CheckedChanged(object sender, EventArgs e)
        {
            documentSplitContainer.Panel2Collapsed = !btnShowDocInfo.Checked;
            btnShowDocInfo.Image = btnShowDocInfo.Checked
                ? Resources.view_right_close
                : Resources.view_right_new;
        }

        private void btnShowGroupToolStrip_CheckedChanged(object sender, EventArgs e)
        {
            toolStripGroup.Visible = btnShowGroupToolStrip.Checked;
        }

        private void btnRevisionMode_Click(object sender, EventArgs e)
        {
            _isRevisionMode = btnRevisionMode.Checked;
            RefreshForm(false);
        }

        #endregion

        #region Логика перемещения по узлам

        private static DocumentGroup _currentDocumentGroup = null;

        // Храним из какого узла пришли
        private static readonly Stack<int> PathBackStack = new Stack<int>();

        // Храним из какого узла ушли
        private static readonly Stack<int> PathForwardStack = new Stack<int>();

        private void tsBtnPathUp_Click(object sender, EventArgs e)
        {
            //if (_currentDocumentGroup == null) // Такого не должно быть
            //    return;
            if (_currentDocumentGroup.Parent == null) return;

            SetSelectedByGroupId((int)_currentDocumentGroup.Parent.Id, treeView1.Nodes);
        }

        private void tsBtnPathBack_Click(object sender, EventArgs e)
        {
            if (PathBackStack.Count < 1)
            {
                tsBtnPathBack.Enabled = false;
                return;
            }

            PathBackStack.Pop();
            SetSelectedByGroupId(PathBackStack.Pop(), treeView1.Nodes);

            tsBtnPathBack.Enabled = PathBackStack.Count > 1;
        }

        private void tsBtnPathForward_Click(object sender, EventArgs e)
        {
            if (PathBackStack.Count < 1)
            {
                tsBtnPathBack.Enabled = false;
                return;
            }

            PathForwardStack.Pop();
            SetSelectedByGroupId(PathForwardStack.Pop(), treeView1.Nodes);

            tsBtnPathBack.Enabled = PathBackStack.Count > 1;
        }

        #endregion

    }
}