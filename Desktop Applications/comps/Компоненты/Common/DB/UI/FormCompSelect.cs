using PGS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PGS.UI.Comps
{
    /// <summary>
    /// Диалог выбора компонента
    /// </summary>
    public partial class FormCompSelect : Form
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        public int? CompID = null;
        /// <summary>
        /// Пользовательское название компонента
        /// </summary>
        public string UserCompName;
        /// <summary>
        /// Принимать только из базы
        /// </summary>
        public bool BaseOnly = true;


        DataTable DT = new DataTable();
        /// <summary>
        /// Конструктор
        /// </summary>
        public FormCompSelect()
        {
            InitializeComponent();

            DataSet ds = new DataSet();
            ds.Tables.Add(DT);
            DT.TableName = "MainTable";
            dgv.AutoGenerateColumns = false;
            dgv.DataSource = new BindingSource(ds, "MainTable");
            UpdateTable();
        }


        private void FormCompSelect_Shown(object sender, EventArgs e)
        {
            if (CompID != null)
            {
                tbFind.Text = "";
                dgv.Focus();
                for (int i = 0; i < dgv.RowCount; i++)
                {
                    if ((int)dgv.Rows[i].Cells[0].Value == CompID)
                    {
                        dgv.Rows[i].Selected = true;
                        dgv.FirstDisplayedCell = dgv[1, i];
                        break;
                    }
                }
            }
            else
            {
                tbFind.Text = UserCompName;
                tbFind.SelectAll();
            }
        }

        private void FormCompSelect_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private void UpdateTable()
        {
            OdbcConnection con = new OdbcConnection();
            con.ConnectionString = DB.ConnectionString;

            try
            {
                Cursor = Cursors.WaitCursor;
                dgv.SuspendLayout();
                con.Open();

                OdbcDataAdapter adapter = new OdbcDataAdapter();


                string s = tbFind.Text.ToLower();

                adapter.SelectCommand = new OdbcCommand(string.Format(@"

SELECT
    id, 
    name,
    namealt,
    formula
FROM comps.comps 
WHERE 
    (LOWER(name) like '%{0}%') or
    (LOWER(namealt) like '%{0}%') or 
    (LOWER(Formula) like '%{0}%') or 
    (LOWER(Formula) like '%{1}%')
", s, DB.ConvertToLatin(s)), con);



                DT.Clear();
                adapter.Fill(DT);
                Column4.Visible = false;
                if (dgv.RowCount > 1)
                {
                    dgv.ClearSelection();
                }
            }
            finally
            {
                dgv.ResumeLayout();
                con.Close();
                Cursor = Cursors.Default;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            doSelect();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateTable();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if ((dgv.RowCount > 0) && BaseOnly)
                {
                    dgv.Focus();
                }

                if ((dgv.RowCount == 1) || !BaseOnly)
                {
                    doSelect();
                }
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            dgv.ClearSelection();
        }

        private void dgv_Enter(object sender, EventArgs e)
        {
            if (dgv.RowCount > 0)
            {
                dgv.Rows[0].Selected = true;
            }
        }

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                doSelect();
            }
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                doSelect();
        }


        private void doSelect()
        {
            UserCompName = tbFind.Text;

            if (dgv.SelectedRows.Count > 0)
            {
                CompID = (int)dgv.SelectedRows[0].Cells[0].Value;
            }
            else
            {
                CompID = null;
            }
            DialogResult = DialogResult.OK;

        }

        private void FormCompSelect_Load(object sender, EventArgs e)
        {

        }
    }
}
