using System;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace fillComplectation
{
    public class ExcelHandler
    {
        private string _pathToXls = Environment.CurrentDirectory + "\\Комплектации.xls";

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
        public class Complectation
        {
            public int kod { get; private set; }
            public string name { get; private set; }
            public double cylType { get; private set; }
            public double? valveType { get; private set; }

            public Complectation(int kod, string name, double cylType, double? valveType)
            {
                this.kod = kod;
                this.name = name;
                this.cylType = cylType;
                this.valveType = valveType;
            }

            public override string ToString()
            {
                return $"{this.kod, 3} {this.name, 40} {this.cylType, 2} {this.valveType, 4}";
            }
        }

        public List<Complectation> GetComplectations()
        {
            List<Complectation> complectations;
            
            Excel.Application excelPackage = new Excel.Application();

            var connection = excelPackage.Workbooks;
            try
            {
                Excel.Workbook excelBook = connection.Open(_pathToXls);
                Excel.Worksheet excelSheet = excelBook.ActiveSheet;

                complectations = ParseComponents(excelSheet);
            }
            finally
            {
                connection.Close();
            }
            
            return complectations;
        }

        private List<Complectation> ParseComponents(Excel.Worksheet excelSheet)
        {
            var complectations = new List<Complectation>();
            
            var startRow = 2;
            var currentRow = startRow;

            int kodColumn = 1;
            int nameColumn = 2;
            int cylTypeColumn = 3;
            int valveTypeColumn = 4;
            
            while (excelSheet.Cells[currentRow, kodColumn].Value != null)
            {
                var test = excelSheet.Cells[currentRow, valveTypeColumn].Value;
                
                var sadas = test.GetType();
                
                complectations.Add(new Complectation(
                    (int) excelSheet.Cells[currentRow, kodColumn].Value,
                    (string) excelSheet.Cells[currentRow, nameColumn].Value,
                    excelSheet.Cells[currentRow, cylTypeColumn].Value,
                    excelSheet.Cells[currentRow, valveTypeColumn].Value as double?));
                    
                currentRow++;
            }

            return complectations;
        }
    }
}