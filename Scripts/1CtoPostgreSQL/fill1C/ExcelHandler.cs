using System;
using System.Collections.Generic;
using Excel = Microsoft.Office.Interop.Excel;

namespace fill1C
{
    public class ExcelHandler
    {
        private string _pathToXls = Environment.CurrentDirectory + "\\Компоненты из 1С.xls";

        /// <summary>
        /// Выбор файла xls, если он не лежит в корневой папке программы (Debug)
        /// </summary>
        /// <returns></returns>
        private string ValidPathToXls()
        {
            if (!System.IO.File.Exists(_pathToXls))
            {
                using (var ofd = new System.Windows.Forms.OpenFileDialog())
                {
                    ofd.Filter = "xls files (*.xls)|*.xls";
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        _pathToXls = ofd.FileName;
                    }
                }
            }
            return _pathToXls;
        }

        public ExcelHandler()
        {
            ValidPathToXls();
        }

        public Dictionary<double, double> GetComps()
        {
            Dictionary<double, double> complectations;
            
            Excel.Application excelPackage = new Excel.Application();

            var connection = excelPackage.Workbooks;
            try
            {
                Excel.Workbook excelBook = connection.Open(_pathToXls);
                Excel.Worksheet excelSheet = excelBook.ActiveSheet;

                complectations = ParseComps(excelSheet);
            }
            finally
            {
                connection.Close();
            }
            
            return complectations;
        }

        /// <summary>
        /// Возвращает словарь, где ключ - id газа, значение - id из 1C
        /// </summary>
        /// <param name="excelSheet"></param>
        /// <returns></returns>
        private Dictionary<double, double> ParseComps(Excel.Worksheet excelSheet)
        {
            var compDict = new Dictionary<double, double>();

            var startRow = 2;
            var currentRow = startRow;

            var idComp = 2;
            var kodColumn = 1;


            while (excelSheet.Cells[currentRow, idComp].Value != null)
            {
                if ((int) excelSheet.Cells[currentRow, idComp].Value != -1)
                {
                    if (!compDict.ContainsKey(excelSheet.Cells[currentRow, idComp].Value))
                    {
                        compDict.Add(
                            (double) excelSheet.Cells[currentRow, idComp].Value,
                            (double) excelSheet.Cells[currentRow, kodColumn].Value);
                    }
                    else
                    {
                        Console.WriteLine("    Повтор Id: " + excelSheet.Cells[currentRow, idComp].Value);
                    }
                }
                
                currentRow++;
            }

            return compDict;
        }
    }
}