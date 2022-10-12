using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PGS.UI.Comps
{
    /// <summary>
    /// Представляет окно списка компонентов
    /// </summary>
    public partial class FormCompsList : Form
    {
        private bool _EditMode = false;

        /// <summary>
        /// Режим редактирования
        /// </summary>
        public bool EditMode
        {
            get { return _EditMode; }
            set
            {
                if (_EditMode == value) return;

                if (value)
                    if (!DB.doCheckPermissionLevel("app_comps_rw"))
                        return;
                _EditMode = value;
                BtnAdd.Visible = value;
                BtnRemove.Visible = value;
                BtnReGroup.Visible = value;
                BtnEdit.Text = value ? "Редактировать" : "Просмотр";
            }
        }

        private bool _TempInKelvin = true;

        /// <summary>
        /// Показывать температуру в К
        /// </summary>
        public bool TempInKelvin
        {
            get { return _TempInKelvin; }
            set
            {
                if (_TempInKelvin == value) return;

                _TempInKelvin = value;
                dgv_column_tkip.HeaderText = value ? "Ткип, К" : "Ткип, °С";
                dgv_column_tkr.HeaderText = value ? "Ткр, К" : "Ткр, °С";
            }
        }


        // Флаги
        private bool _isShowMolar = true;
        private bool _isShowDensity = true;
        private bool _isShowPss = false;
        private bool _isShowKFlow = true;
        private bool _isShowToxity = false;
        private bool _isShowFlammability = false;
        private bool _isShowCriticalParams = false;

        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable("dt");

        /// <summary>
        /// Констркуктор
        /// </summary>
        public FormCompsList()
        {
            InitializeComponent();
            SetDoubleBufered(dgv);

            dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv_column_Isomer.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv_column_toxic.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv_column_flammability.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgv.AutoGenerateColumns = false;
            ds.Tables.Add(dt);
            dgv.DataSource = new BindingSource(ds, "dt");

            LoadGroups();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadSettings();
            UpdateCompList();
        }

        /// <summary>
        /// Обновить список компонентов
        /// </summary>
        private void UpdateCompList(bool isSavePosition = false)
        {
            var position = dgv.FirstDisplayedScrollingRowIndex;

            var selected = new Point(-1, -1);
            if (dgv.SelectedCells.Count > 0)
                selected = new Point(dgv.SelectedCells[0].ColumnIndex, dgv.SelectedCells[0].RowIndex);

            dt.Clear();
            var commandText = GetSQLRequest();
            var con = DB.Connect();
            var adapter = new OdbcDataAdapter(commandText, con);
            adapter.Fill(dt);
            con.Disconnect();
            dgv.Columns[0].Visible = false;

            dgv.ClearSelection();
            if (selected.X >= 0 && selected.X < dgv.Columns.Count && selected.Y >= 0 && selected.Y < dgv.RowCount)
                dgv.Rows[selected.Y].Cells[selected.X].Selected = true;

            if (isSavePosition)
                dgv.FirstDisplayedScrollingRowIndex = position;
        }

        private string BuildWhere()
        {
            var group = "";
            var search = tbFind.Text != ""
                ? $"AND (c.name ~ '(?i){tbFind.Text}' OR c.namealt ~ '(?i){tbFind.Text}' OR c.formula ~ '(?i){tbFind.Text}')"
                : "";
            var filter = "";

            if (cbGroup.ComboBox.SelectedValue != null)
                group = $@"
AND (c.groupid IN
(
	WITH RECURSIVE allgroup(id, parentid) AS 
	(
		SELECT p.id, p.parentid, p.name
		FROM comps.comp_groups p
		WHERE p.id = {(int)cbGroup.ComboBox.SelectedValue}
		UNION
		SELECT c.id, c.parentid, c.name
		FROM comps.comp_groups c, allgroup p
		WHERE c.parentid = p.id
	)
	SELECT id FROM allgroup
))
";
            if (toolStripMenuItemInRow.Checked && !toolStripMenuItemInReferences.Checked)
                filter = @"
AND (SELECT comp_statuses.in_raw FROM comps.comp_statuses WHERE c.id = comp_statuses.compid)
";
            else if (!toolStripMenuItemInRow.Checked && toolStripMenuItemInReferences.Checked)
                filter = @"
AND (SELECT comp_statuses.in_references FROM comps.comp_statuses WHERE c.id = comp_statuses.compid)
";
            else if (toolStripMenuItemInRow.Checked && toolStripMenuItemInReferences.Checked)
                filter = @"
AND (SELECT (comp_statuses.in_raw = true OR comp_statuses.in_references = true) FROM comps.comp_statuses WHERE c.id = comp_statuses.compid)
";

            return $"WHERE 1=1 {search} {group} {filter}";
        }

        /// <summary>
        /// Формирует Sql-запрос, с учётом поиска
        /// </summary>
        private string GetSQLRequest()
        {
            return $@"
SELECT *,  units.name as units_name
FROM Comps.Comps c
LEFT JOIN Comps.Properties cp ON cp.CompID = c.ID
LEFT JOIN Comps.Psat ps ON ps.CompID = c.ID
LEFT JOIN Comps.kFlow k ON k.CompID = c.ID
LEFT JOIN Comps.toxicity tox ON tox.CompID = c.ID
LEFT JOIN Common.conc_units units ON units.ID = tox.pdk_units
LEFT JOIN Comps.flammability flame ON flame.CompID = c.ID
LEFT JOIN Comps.Comp_groups cg ON cg.ID = c.GroupID
{BuildWhere()}
ORDER BY cg.indexes nulls last, c.indexes nulls last, isomer, formula
";
        }


        private void LoadGroups()
        {
            var groups = new List<Groups.Group>();
            groups.Add(new Groups.Group() { ID = null, Name = "" });

            var con = DB.Connect();
            var com = con.CreateCommand();
            com.CommandText = @"SELECT * FROM comps.comp_groups ORDER BY indexes";
            var reader = com.ExecuteReader();

            while (reader.Read())
            {
                var id = (int)reader["id"];
                var name = (string)reader["name"];
                var indexes = DB.FromDBArrayInt((string)reader["indexes"]);
                var parentid = reader["parentid"] as int?;

                groups.Add(new Groups.Group() { ID = id, Name = name, Indexes = indexes, ParentID = parentid });
            }

            con.Disconnect();
            /*  Надо делать нормальное построение списка для combobox
            for (int i = 0; i < groups.Count; i++)
            {
               

                if ((groups[i].Indexes != null) && (groups[i].Indexes.Length > 1))
                {
                    StringBuilder sb = new StringBuilder();
                    int p = groups[i].Indexes.Length - 1;
                    while (p > 0)
                    {
                        if (p > 1)
                        {
                            sb.Append("\x2502");
                        } else
                        {
                            if ((i + 1 < groups.Count) && (groups[i+1].ParentID == groups[i].ParentID))
                            {
                                sb.Append("\x251C");
                            } else
                            {
                                sb.Append("\x2514");
                            }
                        }
                        p--;
                    }
                    groups[i].Name = sb.ToString() + groups[i].Name;
                }
            }
            */

            cbGroup.ComboBox.ValueMember = "ID";
            cbGroup.ComboBox.DisplayMember = "Name";
            cbGroup.ComboBox.DataSource = groups;

            cbGroup.SelectedItem = cbGroup.ComboBox.Items[0];
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!DB.doCheckPermissionLevel("app_comps_rw"))
                return;

            var fce = new FormCompEdit();
            fce.EditMode = EditMode;
            fce.ShowDialog();
            TempInKelvin = fce.TempInKelvin;
            UpdateCompList();
        }


        #region Загрузка настроек

        /// <summary>
        /// Загрузка и установка ностроек формы из реестра
        /// </summary>
        private void LoadSettings()
        {
            using (var regKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PGS\\UI\\FormCompParamList"))
            {
                if (regKey == null) return;

                WindowState = (FormWindowState)Enum.Parse(
                    typeof(FormWindowState), (string)regKey.GetValue("WindowState", WindowState.ToString()));

                Left = (int)regKey.GetValue("WindowLeft", Left);
                Top = (int)regKey.GetValue("WindowTop", Top);
                Width = (int)regKey.GetValue("WindowWidth", Width);
                Height = (int)regKey.GetValue("WindowHeight", Height);


                var rk_dgv = regKey.OpenSubKey("table");
                if (rk_dgv != null)
                    for (var i = 0; i < dgv.Columns.Count; i++)
                        dgv.Columns[i].Width =
                            (int)rk_dgv.GetValue(string.Format("Column{0}Width", i), dgv.Columns[i].Width);

                _isShowMolar = bool.Parse((string)regKey.GetValue("IsShoMolar", _isShowMolar.ToString()));
                _isShowDensity = bool.Parse((string)regKey.GetValue("IsShowDensity", _isShowDensity.ToString()));
                _isShowPss = bool.Parse((string)regKey.GetValue("IsShowPss", _isShowPss.ToString()));
                _isShowKFlow = bool.Parse((string)regKey.GetValue("IsShowKFlow", _isShowKFlow.ToString()));
                _isShowToxity = bool.Parse((string)regKey.GetValue("IsShowToxity", _isShowToxity.ToString()));
                _isShowFlammability =
                    bool.Parse((string)regKey.GetValue("IsShowFlammability", _isShowFlammability.ToString()));
                _isShowCriticalParams =
                    bool.Parse((string)regKey.GetValue("IsShowCriticalParams", _isShowCriticalParams.ToString()));
                toolStripMenuItemAll.Checked =
                    bool.Parse((string)regKey.GetValue("IsFilterAll", toolStripMenuItemAll.Checked.ToString()));
                toolStripMenuItemInRow.Checked =
                    bool.Parse((string)regKey.GetValue("IsFilterInRow", toolStripMenuItemInRow.Checked.ToString()));
                toolStripMenuItemInReferences.Checked = bool.Parse((string)regKey.GetValue("IsFilterInReferences",
                    toolStripMenuItemInReferences.Checked.ToString()));

                toolStripButton_molar.Checked = _isShowMolar;
                toolStripButton_density.Checked = _isShowDensity;
                toolStripButton_rnp.Checked = _isShowPss;
                toolStripButton_kflow.Checked = _isShowKFlow;
                toolStripButton_toxicity.Checked = _isShowToxity;
                toolStripButton_flammability.Checked = _isShowFlammability;
                toolStripButton_critical.Checked = _isShowCriticalParams;

                SetVisibleData();

                TempInKelvin = bool.Parse((string)regKey.GetValue("TempInKelvin", "false"));
                EditMode = bool.Parse((string)regKey.GetValue("EditMode", _EditMode.ToString()));
            }
        }

        /// <summary>
        /// Сохранение настроек формы в реестр
        /// </summary>
        private void SaveSettings()
        {
            using (var regKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PGS\\UI\\FormCompParamList"))
            {
                if (regKey == null)
                    return;

                regKey.SetValue("WindowState", WindowState.ToString());
                if (WindowState != FormWindowState.Maximized)
                {
                    regKey.SetValue("WindowLeft", Left);
                    regKey.SetValue("WindowTop", Top);
                    regKey.SetValue("WindowWidth", Width);
                    regKey.SetValue("WindowHeight", Height);
                    regKey.SetValue("WindowHeight", Height);
                }

                regKey.SetValue("EditMode", _EditMode);

                regKey.SetValue("IsShoMolar", toolStripButton_molar.Checked);
                regKey.SetValue("IsShowDensity", toolStripButton_density.Checked);
                regKey.SetValue("IsShowPss", toolStripButton_rnp.Checked);
                regKey.SetValue("IsShowKFlow", toolStripButton_kflow.Checked);
                regKey.SetValue("IsShowToxity", toolStripButton_toxicity.Checked);
                regKey.SetValue("IsShowFlammability", toolStripButton_flammability.Checked);
                regKey.SetValue("IsShowCriticalParams", toolStripButton_critical.Checked);

                regKey.SetValue("TempInKelvin", TempInKelvin);

                regKey.SetValue("IsFilterAll", toolStripMenuItemAll.Checked);
                regKey.SetValue("IsFilterInRow", toolStripMenuItemInRow.Checked);
                regKey.SetValue("IsFilterInReferences", toolStripMenuItemInReferences.Checked);

                using (var rk_dgv = regKey.CreateSubKey("table"))
                {
                    for (var i = 0; i < dgv.Columns.Count; i++)
                        try
                        {
                            rk_dgv.SetValue(string.Format("Column{0}Width", i), dgv.Columns[i].Width);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                }
            }
        }

        #endregion

        #region Изменение WF

        /// <summary>
        /// Оптимизация производительности dgv
        /// </summary>
        private static void SetDoubleBufered(DataGridView dgv)
        {
            // Значительное ускорение (Памяти хватит)
            // Пришлось использовать рефлексию, чтобы решить проблему медленной отрисовки таблицы
            // (Параметр двойной буфферизации dgv по стандарту выключен, а поле скрыто)
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgv,
                new object[] { true });

            // Записей мало. Виртульный режим использовать не буду
        }

        #endregion

        #region columnsVisible

        /// <summary>
        /// Отображение выбраных данных в таблице
        /// </summary>
        private void SetVisibleData()
        {
            dgv_column_MolarMass.Visible = toolStripButton_molar.Checked;
            dgv_column_dMolarMass.Visible = toolStripButton_molar.Checked;

            dgv_column_MassDensity.Visible = toolStripButton_density.Checked;

            // Рнп.
            dgv_column_Pss243k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss253k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss263k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss273k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss283k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss293k.Visible = toolStripButton_rnp.Checked;

            dgv_column_Kflow.Visible = toolStripButton_kflow.Checked;

            // Токсичность
            dgv_column_toxicity.Visible = toolStripButton_toxicity.Checked;
            dgv_column_units.Visible = toolStripButton_toxicity.Checked;

            // Воспламеняемость
            dgv_column_nkpr.Visible = toolStripButton_flammability.Checked;
            dgv_column_vkpr.Visible = toolStripButton_flammability.Checked;
            dgv_column_max_conc.Visible = toolStripButton_flammability.Checked;

            // Критические параметры
            dgv_column_tkip.Visible = toolStripButton_critical.Checked;
            dgv_column_tkr.Visible = toolStripButton_critical.Checked;
            dgv_column_pkr.Visible = toolStripButton_critical.Checked;
        }

        private void toolStripButton1_CheckedChanged(object sender, EventArgs e)
        {
            dgv_column_MolarMass.Visible = toolStripButton_molar.Checked;
            dgv_column_dMolarMass.Visible = toolStripButton_molar.Checked;
        }

        private void toolStripButton2_CheckedChanged(object sender, EventArgs e)
        {
            dgv_column_MassDensity.Visible = toolStripButton_density.Checked;
        }

        private void toolStripButton3_CheckedChanged(object sender, EventArgs e)
        {
            dgv_column_Pss243k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss253k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss263k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss273k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss283k.Visible = toolStripButton_rnp.Checked;
            dgv_column_Pss293k.Visible = toolStripButton_rnp.Checked;
        }

        private void toolStripButton4_CheckedChanged(object sender, EventArgs e)
        {
            dgv_column_Kflow.Visible = toolStripButton_kflow.Checked;
        }

        private void toolStripButton5_CheckedChanged(object sender, EventArgs e)
        {
            dgv_column_toxicity.Visible = toolStripButton_toxicity.Checked;
            dgv_column_units.Visible = toolStripButton_toxicity.Checked;
        }

        private void toolStripButton6_CheckedChanged(object sender, EventArgs e)
        {
            dgv_column_nkpr.Visible = toolStripButton_flammability.Checked;
            dgv_column_vkpr.Visible = toolStripButton_flammability.Checked;
            dgv_column_max_conc.Visible = toolStripButton_flammability.Checked;
        }

        private void toolStripButton7_CheckedChanged(object sender, EventArgs e)
        {
            dgv_column_tkip.Visible = toolStripButton_critical.Checked;
            dgv_column_tkr.Visible = toolStripButton_critical.Checked;
            dgv_column_pkr.Visible = toolStripButton_critical.Checked;
        }

        #endregion

        private void dgv_CellDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var fce = new FormCompEdit((int)dgv.Rows[e.RowIndex].Cells[0].Value);
            fce.EditMode = EditMode;
            fce.ShowDialog();
            TempInKelvin = fce.TempInKelvin;
            UpdateCompList(true);
        }

        private void FormCompParamList_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            UpdateCompList();
        }

        private void tbFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) UpdateCompList();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedCells.Count > 0)
            {
                var index = dgv.SelectedCells[0].RowIndex;
                var id = (int)dgv.Rows[index].Cells[0].Value;
                var fce = new FormCompEdit(id);
                fce.EditMode = EditMode;
                if (fce.ShowDialog() == DialogResult.OK) 
                    UpdateCompList(true);
                TempInKelvin = fce.TempInKelvin;
            }
            else
            {
                MessageBox.Show("Выберите компонент", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void BtnRemove_Click(object sender, EventArgs e)
        {
            if (!DB.doCheckPermissionLevel("app_comps_rw"))
                return;

            if (dgv.SelectedCells.Count > 0)
            {
                var index = dgv.SelectedCells[0].RowIndex;
                var id = (int)dgv.Rows[index].Cells[0].Value;

                if (MessageBox.Show("Удалить " + dgv[dgv_column_Name.Index, index].Value + "?", "",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    OdbcTransaction transaction = null;
                    try
                    {
                        var con = DB.Connect();
                        transaction = con.BeginTransaction();
                        var com = con.CreateCommand();
                        com.Transaction = transaction;
                        PGS.Comps.CompInfo.Delete(id, con, transaction);
                        transaction.Commit();
                        con.Disconnect();
                    }
                    catch (Exception exception)
                    {
                        if (transaction != null) transaction.Rollback();
                        throw exception;
                    }


                    UpdateCompList();
                }
            }
            else
            {
                MessageBox.Show("Выберите компонент", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCompList();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCompList();
        }

        private static List<int> ColumnsToRound = new List<int>()
        {
            9, 10, 11, 12, 13, 14, 15, 16, 17, 19, 20, 21, 22, 23, 24
        };

        // Округление
        private void dgv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //TODO: Надо переделать - использование номеров столбцов плохая идея.
            if (e.ColumnIndex >= 0)
            {
                if (e.Value is DBNull)
                    return;

                var dpn = dgv.Columns[e.ColumnIndex].DataPropertyName;
                switch (dpn)
                {
                    case "pkr":
                    {
                        var v = (double)e.Value;
                        e.Value = Rounding.ValueToString(v / Constants.Punits.MPa, 2);
                        return;
                    }
                    case "tkip":
                    case "tkr":
                    {
                        var To = TempInKelvin ? 0 : 273.15;
                        var v = (double)e.Value;
                        e.Value = Rounding.ValueToString(v - To, 3);
                        return;
                    }
                    default:
                        break;
                }
            }

            var i = dgv_column_flammability.Index;
            var k = dgv_column_MolarMass.Index;
            // Для молярной массы
            if (e.ColumnIndex == dgv_column_MolarMass.Index && e.Value != DBNull.Value)
            {
                double? dcabs = null;
                if ((sender as DataGridView)[dgv_column_dMolarMass.Index, e.RowIndex].Value != DBNull.Value)
                    dcabs = (double)(sender as DataGridView)[dgv_column_dMolarMass.Index, e.RowIndex].Value;

                if (dcabs != null)
                    e.Value = Rounding.ValueToString((double)e.Value, (double)dcabs);
                else
                    e.Value = Rounding.ValueToString((double)e.Value, 3);
            }

            if (e.ColumnIndex == dgv_column_dMolarMass.Index && e.Value != DBNull.Value)
            {
                double? dcabs = null;
                if ((sender as DataGridView)[dgv_column_dMolarMass.Index, e.RowIndex].Value != DBNull.Value)
                    dcabs = (double)(sender as DataGridView)[dgv_column_dMolarMass.Index, e.RowIndex].Value;

                if (dcabs == null) return;

                e.Value = Rounding.UncertaintyToString((double)dcabs);
            }

            // Для остальных столбцов
            if (ColumnsToRound.Contains(e.ColumnIndex) && e.Value != DBNull.Value)
                e.Value = Rounding.ValueToString((double)e.Value, 3);
        }

        // Копирование из ячеек
        private void dgv_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode != Keys.C || !e.Control) return;

            var sb = new StringBuilder();
            var cols = new List<int>();
            var rows = new List<int>();
            for (var i = 0; i < dgv.SelectedCells.Count; i++)
            {
                var cell = dgv.SelectedCells[i];
                if (!cols.Contains(cell.ColumnIndex)) cols.Add(cell.ColumnIndex);
                if (!rows.Contains(cell.RowIndex)) rows.Add(cell.RowIndex);
            }

            rows.Sort();
            cols.Sort();

            var cells = new string[rows.Count, cols.Count];
            for (var i = 0; i < dgv.SelectedCells.Count; i++)
            {
                var cell = dgv.SelectedCells[i];
                string v;
                if (cell.OwningColumn is DataGridViewComboBoxColumn)
                    v = cell.EditedFormattedValue.ToString();
                else
                    v = cell.Value.ToString();

                cells[rows.IndexOf(cell.RowIndex), cols.IndexOf(cell.ColumnIndex)] = v;
            }

            for (var r = 0; r < rows.Count; r++)
            {
                for (var c = 0; c < cols.Count; c++)
                {
                    if (c != 0)
                        sb.Append("\t");
                    sb.Append(cells[r, c]);
                }

                sb.AppendLine();
            }

            Clipboard.SetDataObject(sb.ToString());
        }

        private void BtnReGroup_Click(object sender, EventArgs e)
        {
            if (!DB.doCheckPermissionLevel("app_comps_rw"))
                return;

            var groupForm = new Groups.FormGroupEdit("comps.comp_groups");
            groupForm.ShowDialog();
            LoadGroups();
        }

        private void FormCompsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.F2 | Keys.Control))
            {
                if (DB.doLogin() == DialogResult.OK) EditMode = true;
            }
            else if (e.KeyData == Keys.F2)
            {
                EditMode = !EditMode;
            }
        }

        private void BtnShowLog_Click(object sender, EventArgs e)
        {
            var f = new PGS.Comps.UI.FormChangeLog();
            f.ShowDialog();
        }

        private void toolStripMenuItemAll_Click(object sender, EventArgs e)
        {
            toolStripMenuItemInRow.Checked = false;
            toolStripMenuItemInReferences.Checked = false;
            UpdateCompList();
        }

        private void toolStripMenuItemInRow_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItemInRow.Checked)
                toolStripMenuItemAll.Checked = false;
            else if (!toolStripMenuItemInRow.Checked && !toolStripMenuItemInReferences.Checked)
                toolStripMenuItemAll.Checked = true;

            UpdateCompList();
        }

        private void toolStripMenuItemInReferences_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItemInReferences.Checked)
                toolStripMenuItemAll.Checked = false;
            else if (!toolStripMenuItemInRow.Checked && !toolStripMenuItemInReferences.Checked)
                toolStripMenuItemAll.Checked = true;

            UpdateCompList();
        }
    }
}