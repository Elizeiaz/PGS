using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PGS.Print.UI
{
    public partial class FormOptionalParamEditor : Form
    {
        private struct Param
        {
            public string Key;
            public RadioButton[] Buttons;
        }
        private Param[] Params;

        static private Regex parse = new Regex(@"\[@(?:([^\@]*)@)?(?:([^|\]]+)\|?)*\]");

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
                    Params = new Param[_dictonary.Keys.Count];
                    tlp.RowCount = 0;
                    int r = 0;
                    foreach (string key in value.Keys)
                    {
                        tlp.RowCount = r + 1;

                        Match m = parse.Match(key);
                        if (m.Success)
                        {
                            Param param = new Param();
                            param.Key = key;
                            param.Buttons = new RadioButton[m.Groups[2].Captures.Count];

                            Panel p = new Panel();
                            Label label = new Label();
                            p.Controls.Add(label);
                            label.Text = System.Net.WebUtility.HtmlDecode(m.Groups[1].Value);
                            label.Location = new Point(6, 6);
                            label.AutoSize = true;

                            for (int i = 0; i < m.Groups[2].Captures.Count; i++)
                            {
                                RadioButton rb = new RadioButton();
                                rb.Text = System.Net.WebUtility.HtmlDecode(m.Groups[2].Captures[i].Value);
                                rb.Location = new Point(12, 6 + 20 + (rb.Height + 3) * i);
                                rb.Checked = i == 0;

                                p.Controls.Add(rb);
                                param.Buttons[i] = rb;
                            }
                            p.AutoSize = true;
                            tlp.Controls.Add(p, 0, r);
                            Params[r] = param;
                            r++;
                        }
                    }
                    FillDictonary();
                }
            }
        }


        public FormOptionalParamEditor()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            FillDictonary();
            DialogResult = DialogResult.OK;
        }

        private void FillDictonary()
        {
            foreach (var p in Params)
            {
                foreach (var b in p.Buttons)
                {
                    if (b.Checked)
                    {
                        _dictonary[p.Key] = b.Text;
                    }
                }
            }
        }
    }
}
