using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PGS.Comps.UI
{
    /// <summary>
    /// Окно лога изменений данных
    /// </summary>
    public partial class FormChangeLog : Form
    {

        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable("dt");

        /// <summary>
        /// Констркуктор
        /// </summary>
        public FormChangeLog()
        {
            InitializeComponent();
            dgv.AutoGenerateColumns = false;
            ds.Tables.Add(dt);
            dgv.DataSource = new BindingSource(ds, "dt");
            SetDoubleBufered(dgv);
        }

        /// <summary>
        /// Оптимизация производительности dgv
        /// </summary>
        private static void SetDoubleBufered(DataGridView dgv)
        {
            typeof(DataGridView).InvokeMember(
                "DoubleBuffered",
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                null,
                dgv,
                new object[] { true });
        }

        private void UpdateTable()
        {
            Point selected = new Point(-1, -1);
            if (dgv.SelectedCells.Count > 0)
            {
                selected = new Point(dgv.SelectedCells[0].ColumnIndex, dgv.SelectedCells[0].RowIndex);
            }

            dt.Clear();
            var commandText = @"
SELECT stamp, userid, obj_description(concat('comps.', tablename)::regclass) AS  tablename, operation, dname as compname, old_rec, new_rec
FROM comps.change_log
LEFT join comps.comps ON id = compid 
ORDER BY stamp";
            var con = DB.Connect();
            var adapter = new OdbcDataAdapter(commandText, con);
            adapter.Fill(dt);
            con.Disconnect();


            if ((selected.X >= 0) && (selected.X < dgv.Columns.Count) && (selected.Y >= 0) && (selected.Y < dgv.RowCount))
            {
                dgv.Rows[selected.Y].Cells[selected.X].Selected = true;
            }
            else
            {
                if (dgv.RowCount > 0)
                {
                    dgv.Rows[dgv.RowCount - 1].Cells[0].Selected = true;
                    dgv.FirstDisplayedCell = dgv.Rows[dgv.RowCount - 1].Cells[0];
                }

            }
        }

        private void FormChangeLog_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
                UpdateTable();

            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }

        private void FormChangeLog_Load(object sender, EventArgs e)
        {
            UpdateTable();
        }
    }
}
