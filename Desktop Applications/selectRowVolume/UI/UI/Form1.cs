using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using PGS;
using PGS.Mixes;

namespace UI
{
    public partial class FormRawSelect : Form
    {
        private static MixBaseID _mixBaseId = null;
        
        private int? _compID = null;
        private int? _compFocused = null;
        
        private bool _isShowAll = false;
        private bool _isShowMixes = false;
        private bool _isShowComps = false;
        
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("dt"); 
        
        public FormRawSelect()
        {
            InitializeComponent();

            ds.Tables.Add(dt);
            dgv.DataSource = new BindingSource(ds, "dt");
            dgv.AutoGenerateColumns = false;
            
            OptimizeDataGridView(dgv);
        }

        // Лист с индексами колон, которые можно объединять
        private List<int> ColumnsToRepaint = new List<int>()
        {
            0, 1, 2, 7, 8, 9, 10, 11, 12, 13
        };

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                e.AdvancedBorderStyle.Bottom = dgv.AdvancedCellBorderStyle.Bottom;
                return;
            }

            if (CompareCells(e.RowIndex) && ColumnsToRepaint.Contains(e.ColumnIndex))
            {
                e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            }
            else
            {
                e.AdvancedBorderStyle.Top = dgv.AdvancedCellBorderStyle.Top;
            }
            
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
        }

        private bool CompareCells(int row)
        {
            return row >= 1 &&
                   dgv.Rows[row - 1].Cells[0].Value.Equals(dgv.Rows[row].Cells[0].Value);
        }

        private void FormRawSelect_Load(object sender, EventArgs e)
        {
            DeserializeProperties();
            UpdateFormFlasg();
            LoadComps();
            
//            this.dgv.Paint += new PaintEventHandler(dgv_Paint);
        }
        
        #region Загрузка настроек

        /// <summary>
        /// Загрузка и установка ностроек формы из реестра
        /// </summary>
        private void DeserializeProperties()
        {
            using (var regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\UI\\FormRawSelect"))
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
                {
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        dgv.Columns[i].Width = (int)rk_dgv.GetValue(string.Format("Column{0}Width", i), dgv.Columns[i].Width);
                    }
                }

                
                _isShowAll = bool.Parse((string) regKey.GetValue("IsShowAll", _isShowAll));
                _isShowMixes = bool.Parse((string) regKey.GetValue("IsShowMixes", _isShowMixes));
                _isShowComps = bool.Parse((string) regKey.GetValue("IsShowComps", _isShowComps));
            }
        }

        /// <summary>
        /// Сохранение настроек формы в реестр
        /// </summary>
        private void SerializeProperties()
        {
            using (var regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\UI\\FormRawSelect", true))
            {
                if (regKey == null)
                {
                    using (var upperRegKey = Registry.CurrentUser.OpenSubKey("SOFTWARE", true))
                    {
                        upperRegKey.CreateSubKey("PGS");
                    }

                    using (var upperRegKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS", true))
                    {
                        upperRegKey.CreateSubKey("UI");
                    }
                    
                    using (var upperRegKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\UI", true))
                    {
                        upperRegKey.CreateSubKey("FormRawSelect");
                    }
                    
                    SerializeProperties();
                }

                regKey.SetValue("WindowState", WindowState.ToString());
                regKey.SetValue("WindowLeft", Left);
                regKey.SetValue("WindowTop", Top);
                regKey.SetValue("WindowWidth", Width);
                regKey.SetValue("WindowHeight", Height);
                regKey.SetValue("WindowHeight", Height);
                regKey.SetValue("IsShowAll", checkBox2.Checked);
                regKey.SetValue("IsShowMixes", checkBox1.Checked);
                regKey.SetValue("IsShowComps", checkBox3.Checked);
                using (var rk_dgv = regKey.OpenSubKey("table", true))
                {
                    if (rk_dgv == null)
                    {
                        regKey.CreateSubKey("table");
                        
                        SerializeProperties();
                    }
                    
                    for (var i = 0; i < dgv.Columns.Count; i++)
                    {
                        rk_dgv.SetValue(string.Format("Column{0}Width", i), dgv.Columns[i].Width);
                    }
                }
            }
        }

        private void UpdateFormFlasg()
        {
            checkBox2.Checked = _isShowAll;
            checkBox1.Checked = _isShowMixes;
            checkBox3.Checked = _isShowComps;
        }

        #endregion

        #region Запросы к бд и загрузка в dgv

        private void LoadComps()
        {
            dt.Clear();
            var con = new OdbcConnection(DB.ConnectionString);
            var commandText = GetSQLCommand();
            var adapter = new OdbcDataAdapter(commandText, con);
            adapter.Fill(dt);
            dgv.Sort(Column1, ListSortDirection.Ascending);
        }

        /// <summary>
        /// Возвращает sql запрос в зависимости от флагов
        /// </summary>
        private string GetSQLCommand()
        {
            var sqlCommand = new StringBuilder();
            
            var mixesRequest = @"
SELECT m.id, m.date, m.mixnumber, comps.formula, mc.c, mc.dcabs, mc.dcrel, 
	   mbasecomps.formula, mcyls.volume, cyltypes.name as typename, valvetypes.name as valvename,
	   mcyls.cylnum, m.pressure as quantity, 'ат' as quantity_name, 'Mix' as type
FROM lab_references.mixes m
LEFT JOIN lab_references.mix_comps mc ON m.id = mc.mixid
LEFT JOIN comps.comps ON mc.compid = comps.id
LEFT JOIN comps.comps mbasecomps ON m.basecompid = mbasecomps.id
LEFT JOIN cyls.cyls mcyls ON m.cylid = mcyls.id
LEFT JOIN cyls.cyltypes ON mcyls.cyltype = cyltypes.id
LEFT JOIN cyls.valvetypes ON mcyls.valvetype = valvetypes.id
";
            var rawsRequest = @"
SELECT r.id, r.date, 'Чистые' as mixnumber, comps.formula, rc.c, rc.dcabs, rc.dcrel,
	   '' as basecomp, cyls.volume, cyltypes.name as typename, valvetypes.name as valvename,
	   cyls.cylnum, r.quantity, quantity_units.short_name as quantity_name, 'Reference' as type
FROM raw_materials.raws r
LEFT JOIN raw_materials.raw_comps rc ON r.id = rc.rawid
LEFT JOIN comps.comps ON rc.compid = comps.id
LEFT JOIN cyls.cyls ON r.cylid = cyls.id
LEFT JOIN cyls.cyltypes ON cyls.cyltype = cyltypes.id
LEFT JOIN cyls.valvetypes ON cyls.valvetype = valvetypes.id
LEFT JOIN common.quantity_units ON r.quantity_units = quantity_units.id
";
            
            const string rawTarget = "(rc.target is true)";
            const string mixTarget = "(mc.target is true)";

            // Формирование Where
            var whereRaw = "";
            var whereMix = "";
            
            if (_compID == null || _isShowAll)
            {
                if (!_isShowComps)
                {
                    whereMix = "\nWHERE " + mixTarget;
                    whereRaw = "\nWHERE " + rawTarget;
                }
            }
            else
            {
                var searchCompIDRaw = string.Format(
                    "(EXISTS(SELECT * FROM raw_materials.raw_comps ct WHERE (r.id = ct.rawid AND ct.compid = {0})))",
                _compID);
                var searchCompIDMix = string.Format(
                    "(EXISTS(SELECT * FROM lab_references.mix_comps ct WHERE (m.id = ct.mixid AND ct.compid = {0})))" +
                    "OR (m.basecompid = {0})",
                    _compID);

                if (!_isShowComps)
                {
                    whereMix = "WHERE (" + mixTarget + " AND " + searchCompIDMix + ")";
                    whereRaw = "WHERE (" + rawTarget + " AND " + searchCompIDRaw + ")";
                }
                else
                {
                    whereMix = "WHERE " + searchCompIDMix;
                    whereRaw = "WHERE " + searchCompIDRaw;
                }
            }

            // Формирование sql запроса
            sqlCommand.Append(rawsRequest);
            sqlCommand.Append(whereRaw);

            if (!_isShowMixes) return sqlCommand.ToString();
            
            sqlCommand.Append("\n");
            sqlCommand.Append("UNION ALL");
            sqlCommand.Append("\n");
            sqlCommand.Append(mixesRequest);
            sqlCommand.Append(whereMix);

            return sqlCommand.ToString();
        }

        #endregion

        private void button4_Click(object sender, EventArgs e)
        {
            var compForm = new FormCompRawSelect();
            compForm.ShowDialog();
            
            _compID = compForm.CompID;
            button4.Text = compForm.CompName ?? "Выбрать компонент";

            LoadComps();
        }
        
        /// <summary>
        /// Оптимизация производительности dgv
        /// </summary>
        private static void OptimizeDataGridView(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgv,
                new object[] {true});
        }

        private void FormRawSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerializeProperties();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            _isShowAll = checkBox2.Checked;
            LoadComps();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _isShowMixes = checkBox1.Checked;
            LoadComps();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            _isShowComps = checkBox3.Checked;
            LoadComps();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _mixBaseId = null;
            Close();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _mixBaseId = new MixBaseID(
                (int) dgv[Column1.Index, e.RowIndex].Value,
                (MixTypes) Enum.Parse(typeof(MixTypes), (string) dgv[Column15.Index, e.RowIndex].Value, true));
            Close();
        }

        private void dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            _compFocused = e.RowIndex;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_compFocused != null)
            {
                _mixBaseId = new MixBaseID(
                    (int) dgv[Column1.Index, (int) _compFocused].Value,
                    (MixTypes) Enum.Parse(typeof(MixTypes), (string) dgv[Column15.Index, (int) _compFocused].Value, true));
            }
            else
            {
                MessageBox.Show("Выберите исходник", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
        }
    }
}
