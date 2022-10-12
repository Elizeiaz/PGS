using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PGS.Print.UI
{
    public partial class FormDictonaryEdit : Form
    {
        public Dictionary<string, string> _dictonary = null;
        public Dictionary<string, string> dictonary
        {
            get
            {
                return _dictonary;
            }
            set
            {
                if (_dictonary != value)
                {
                    _dictonary = value;
                    dgv.RowCount = 0;
                    foreach (string key in value.Keys)
                    {
                        //TODO: Надо декодировать строки для ползователя. Но в словаре должны быть как есть.
                        //System.Net.WebUtility.HtmlDecode(
                        dgv.Rows.Add(key, value[key]);
                    }
                }
            }
        }
        public FormDictonaryEdit()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            _dictonary.Clear();
            foreach (DataGridViewRow row in dgv.Rows)
            {
                string key = row.Cells[0].Value as string;
                string value = row.Cells[1].Value as string;
                if (key != null)
                {
                    _dictonary[key] = value;
                }
            }
            DialogResult = DialogResult.OK;
        }
    }
}
