using Microsoft.Win32;
using PGS.Print.HTML;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PGS.Print
{
    // Можем использовать встроенный класс PaperKind, но тут можно сопоставить ID с значениями бд
    public enum PaperSize
    {
        A4 = 1,
        A5 = 0,
        A6 = 2
    }

    

    /// <summary>
    /// Паспорт для очереди печати
    /// </summary>
    public class Passport
    {
        public PaperSize PaperSize { get; private set; }
        public string Html { private set; get; }

        public Passport(PaperSize paperSize, string html)
        {
            this.PaperSize = paperSize;
            this.Html = html;
        }


        public Passport(Dictionary<string, string> data, params Standarts.Standart.TemplateInfo[] templates)
        {
            if ((templates == null) || (templates.Length == 0))
                throw new Exception("Должен быть хотябы один шаблон");

            PaperSize = (PaperSize)templates[0].Format;
            string template_dir = @"E:\#Work\#PGS\!АПК\Печать\Passport\TestApp\bin\Debug\html\";
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\Print\\Pasports");
            if (rk != null)
            {
                template_dir = (string)rk.GetValue("TemplateDir", "");
            }


            StringBuilder sb = new StringBuilder();
            foreach (var template in templates)
            {
                string template_file = System.IO.Path.Combine(template_dir, template.File);
                sb.Append(Builder.Build(template_file, data));
                sb.Append("\n");
            }
            Html = sb.ToString();

        }


        /// <summary>
        /// Создание паспорта с контролем заполнения всех полей
        /// </summary>
        /// <param name="data"></param>
        /// <param name="templates"></param>
        /// <returns></returns>
        static public Passport CreatePasport(Dictionary<string, string> data, params Standarts.Standart.TemplateInfo[] templates)
        {
            PGS.Print.Passport p = new PGS.Print.Passport(data, templates);

            var OptionalKeys = p.GetOptionalParams();
            if (OptionalKeys.Count > 0)
            {
                PGS.Print.UI.FormOptionalParamEditor fde = new PGS.Print.UI.FormOptionalParamEditor();
                //Заполним словарь
                Dictionary<string, string> empty = new Dictionary<string, string>();
                foreach (string s in OptionalKeys)
                {
                    empty.Add(s, "");
                }

                fde.dictonary = empty;

                fde.ShowDialog();
                p.SetParams(fde.dictonary);
            }


            var EmptyKeys = p.GetEmptyParams();
            if (EmptyKeys.Count != 0)
            {
                PGS.Print.UI.FormDictonaryEdit fde = new PGS.Print.UI.FormDictonaryEdit();

                Dictionary<string, string> empty = new Dictionary<string, string>();
                foreach (string s in EmptyKeys)
                {
                    empty.Add(s, "");
                }

                fde.dictonary = empty;
                if (fde.ShowDialog() == DialogResult.OK)
                {
                    p.SetParams(fde.dictonary);
                }
                else return null;
            }

            return p;
        }


        public List<string> GetEmptyParams()
        {
            return Builder.GetParams(Html);
        }

        public List<string> GetOptionalParams()
        {
            return Builder.GetOptionalParams(Html);
        }


        public void SetParams(Dictionary<string, string> dictonary)
        {
            Html = Builder.Replace(Html, dictonary);
        }
    }
}