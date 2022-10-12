using System;
using System.Data;
using System.Data.Odbc;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using PGS;

namespace UI
{
    public partial class FormCompRawSelect : Form
    {
        private static int? _compId = null;
        private static string _compName = null;
        private static int? _compFocused = null;

        public int? CompID
        {
            get { return _compId; }
        }

        public string CompName
        {
            get { return _compName; }
        }
        
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("dt"); 

        public FormCompRawSelect()
        {
            InitializeComponent();

            ds.Tables.Add(dt);
            dgv.DataSource = new BindingSource(ds, "dt");
            dgv.AutoGenerateColumns = false;
            
            OptimizeDataGridView(dgv);
        }

        private void FormCompRawSelect_Load(object sender, EventArgs e)
        {
            LoadComps();
            DeserializeProperties();
        }

        #region Работа с данными

        private void LoadComps()
        {
            dt.Clear();
            var con = new OdbcConnection(DB.ConnectionString);
            var commandText = "SELECT id, name, namealt, formula FROM Comps.Comps";
            var adapter = new OdbcDataAdapter(commandText, con);
            adapter.Fill(dt);
        }
        
        #warning Изменится, когда будут compGroup
        private void SearchComp(string searchingString)
        {
            dt.Clear();
            var con = new OdbcConnection(DB.ConnectionString);
            var commandText =
                string.Format("select * from comps.comps where (name ~ '{0}' OR namealt ~ '{0}' OR formula ~ '(?i){0}')",
                    searchingString);
            var adapter = new OdbcDataAdapter(commandText, con);
            adapter.Fill(dt);
        }

        #endregion

        #region Загрузка настроек

        /// <summary>
        /// Загрузка и установка ностроек формы из реестра
        /// </summary>
        private void DeserializeProperties()
        {
            using (var regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\UI\\FormCompRawSelect"))
            {
                if (regKey == null) return;
            
                WindowState = (FormWindowState)Enum.Parse(
                    typeof(FormWindowState), (string)regKey.GetValue("WindowState", WindowState.ToString()));

                Left = (int)regKey.GetValue("WindowLeft", Left);
                Top = (int)regKey.GetValue("WindowTop", Top);
                Width = (int)regKey.GetValue("WindowWidth", Width);
                Height = (int)regKey.GetValue("WindowHeight", Height);
            }
        }

        /// <summary>
        /// Сохранение настроек формы в реестр
        /// </summary>
        private void SerializeProperties()
        {
            using (var regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\UI\\FormCompRawSelect", true))
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
                        upperRegKey.CreateSubKey("FormCompRawSelect");
                    }
                    
                    SerializeProperties();
                }

                regKey.SetValue("WindowState", WindowState.ToString());
                regKey.SetValue("WindowLeft", Left);
                regKey.SetValue("WindowTop", Top);
                regKey.SetValue("WindowWidth", Width);
                regKey.SetValue("WindowHeight", Height);
            }
        }

        #endregion

        private void label2_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchComp(textBox1.Text);
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SearchComp(textBox1.Text);
            }
        }
        
        private void FormCompRawSelect_FormClosing(object sender, FormClosingEventArgs e)
        {
            SerializeProperties();
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            _compId = (int) dgv[Column1.Index, e.RowIndex].Value;
            _compName = (string) dgv[Column2.Index, e.RowIndex].Value;
            Close();
        }
        
        // Кнопка выбрать
        private void button3_Click(object sender, EventArgs e)
        {
            if (_compFocused != null)
            {
                _compId = (int) dgv[Column1.Index, (int) _compFocused].Value;
                _compName = (string) dgv[Column2.Index, (int) _compFocused].Value;
                Close();
            }
            else
            {
                MessageBox.Show("Выберите компонент", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _compId = null;
            _compName = null;
            Close();
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

        private void dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            _compFocused = e.RowIndex;
        }
    }
}