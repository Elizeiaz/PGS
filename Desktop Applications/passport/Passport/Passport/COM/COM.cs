using PGS.Standarts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGS.Print
{
    [ComVisible(true)]
    public class COMPrinter : ICOMPrinter
    {
        private Form form;
        Printer printer;
        public COMPrinter()
        {
            form = new Form();
            form.Text = "Предварительный просмотр";
            form.WindowState = FormWindowState.Maximized;
            form.Size = new System.Drawing.Size(400, 400);
            printer = new Printer(form);
        }
        public void Config()
        {

        }

        public void doPrint()
        {
            try
            {
                form.Show();

                if (printer.GetPassports(PGS.Print.PaperSize.A5).Count > 0)
                    printer.Print(PGS.Print.PaperSize.A5);

                if (printer.GetPassports(PGS.Print.PaperSize.A4).Count > 0)
                    Task.Factory.StartNew(() =>
                {
                    while (printer.Busy) Thread.Sleep(100);

                    form.Invoke((Action)delegate { printer.Print(PGS.Print.PaperSize.A4); });

                });


                form.Hide();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Модуль печати паспортов", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Abort()
        {
            MessageBox.Show("Не реализованая функция");
        }

        public void AddPage(int templateID, string data)
        {
            try
            {
                var template = Standart.TemplateInfo.Load(templateID);//3 - Тех. 8 - ГСО 9 - СОП 5 - оборот
                var dict = ParseDictonary(data);
                PGS.Print.Passport p = new PGS.Print.Passport(dict, template);
                printer.Append(p);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Модуль печати паспортов", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static private Dictionary<string, string> ParseDictonary(string s)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            string[] lines = s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var vs = line.Split(new char[] { '=' }, 2);
                string key = vs[0];
                string value = vs.Length > 1 ? vs[1] : "";
                value = value.Replace("\\r\\n", "\r\n");
                dict.Add(key, value);
            }

            return dict;
        }


    }
}
