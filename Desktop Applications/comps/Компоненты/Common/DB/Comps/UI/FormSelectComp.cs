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
    [Obsolete("Используйте FormCompSelect")]
    public partial class FormSelectComp : Form
    {
        /// <summary>
        /// Выбраный компонент
        /// </summary>
        public PGS.Comps.CompInfo SelectedComp = null;
        /// <summary>
        /// Констркуктор
        /// </summary>
        public FormSelectComp()
        {
            InitializeComponent();
        }

        private void FormSelectComp_Load(object sender, EventArgs e)
        {
            OdbcConnection con = new OdbcConnection();
            con.ConnectionString = DB.ConnectionString;
            OdbcDataAdapter adapter = new OdbcDataAdapter(@"
SELECT 
    comps.id,
    comps.Name,
    NameAlt,
    Isomer,
    Formula
FROM Comps.Comps
LEFT JOIN comps.comp_groups ON comp_groups.id = Comps.groupid
ORDER BY comp_groups.indexes, comps.indexes, comps.isomer, Comps.formula", con);
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable("CompBaseInfo"));
            adapter.Fill(ds.Tables[0]);
            dgv.DataSource = new BindingSource(ds, "CompBaseInfo");
            dgv.Columns[0].Visible = false;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 1)
            {
                int id = (int)dgv.SelectedRows[0].Cells[0].Value;

                SelectedComp = PGS.Comps.CompInfo.Load(id);
                DialogResult = DialogResult.OK;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            BtnOk.Enabled = dgv.SelectedRows.Count == 1;
        }

        private void FormSelectComp_Shown(object sender, EventArgs e)
        {
            dgv.ClearSelection();
            if (SelectedComp != null)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if ((int)dgv.Rows[i].Cells[0].Value == SelectedComp.ID)
                    {
                        dgv.Rows[i].Selected = true;
                        dgv.FirstDisplayedCell = dgv[1, i];
                        break;
                    }
                }
                if (dgv.SelectedRows.Count == 0)
                {
                    SelectedComp = null;
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                BtnOk_Click(null, null);
        }
    }


}
