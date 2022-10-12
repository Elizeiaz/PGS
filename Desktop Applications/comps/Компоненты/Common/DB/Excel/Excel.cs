using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PGS._Excel
{
    /// <summary>
    /// Предоставляет функции для работы с фрагментом Excel в буфере обмена
    /// </summary>
    [Obsolete("Использовать Excel.dll", true)]
    static public class ExcelClipboard
    {
        /// <summary>
        /// Проверка, что в буфере обмена часть таблицы Excel
        /// </summary>
        /// <returns></returns>
        static public bool ContainExcelSheet
        {
            get
            {
                return System.Windows.Forms.Clipboard.ContainsData("Xml Spreadsheet");
            }
        }

        /// <summary>
        /// Разбирает содержащуюся в буфере страницу
        /// </summary>
        /// <returns></returns>
        static public List<List<string>> ParseExcelSheet()
        {
            List<List<string>> table = new List<List<string>>();
            if (!ExcelClipboard.ContainExcelSheet)
                return table;


            MemoryStream ms = (MemoryStream)System.Windows.Forms.Clipboard.GetData("Xml Spreadsheet");
            ms.SetLength(ms.Length - 1);
            XDocument xd = XDocument.Load(ms, LoadOptions.None);

            string ns = "urn:schemas-microsoft-com:office:spreadsheet";
            XElement xE = xd.Root.Element(XName.Get("Worksheet", ns));
            string SheetName = xE.Attribute(XName.Get("Name", ns)).Value;
            XElement xT = xE.Element(XName.Get("Table", ns));

            foreach (XElement xR in xT.Elements(XName.Get("Row", ns)))
            {
                XAttribute xa = xR.Attribute(XName.Get("Index", ns));
                if (xa != null)
                {
                    int i = int.Parse(xa.Value);
                    while (table.Count < i - 1)
                    {
                        table.Add(new List<string>());
                    }
                }

                List<string> row = new List<string>();
                foreach (XElement xC in xR.Elements(XName.Get("Cell", ns)))
                {
                    xa = xC.Attribute(XName.Get("Index", ns));
                    if (xa != null)
                    {
                        int i = int.Parse(xa.Value);
                        while (row.Count < i - 1)
                        {
                            row.Add("");
                        }

                    }
                    row.Add(xC.Value);
                    xa = xC.Attribute(XName.Get("MergeAcross", ns));
                    if (xa != null)
                    {
                        int i = int.Parse(xa.Value);
                        while (i > 0)
                        {
                            row.Add("");
                            i--;
                        }
                    }
                }
                table.Add(row);
            }
            return table;
        }
    }
}
