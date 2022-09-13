using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Excel = Microsoft.Office.Interop.Excel;

namespace Comp1S
{
    class ExcelHandler
    {
        //Класс предстовляющий сущность компонента
        public class MyComponent
        {
            //Обработка исключений, таких как n-бутан, i-бутан
            public string componentName { get; private set; }
            public int compopnentID { get; set; }
            public double nkprAir { get; private set; }
            public double vkprAir { get; private set; }
            public double maxConcAir { get; private set; }
            
            
            public MyComponent(string componentName, double nkprAir, double vkprAir, double maxConcAir)
            {
                this.componentName = componentName;
                this.nkprAir = nkprAir;
                this.vkprAir = vkprAir;
                this.maxConcAir = maxConcAir;
            }

            public override string ToString()
            {
                return compopnentID.ToString() + '|'
                    + componentName + '|'
                    + "nkpr: " + nkprAir.ToString() + '|'
                    + "vkpr: " + vkprAir.ToString() + '|'
                    + "maxConc " + maxConcAir.ToString();
            }
        }

        static private Excel.Workbooks ConnectToExcel()
        {
            Excel.Application excelPackage = new Excel.Application();
            //            excelPackage.Visible = true;
            return excelPackage.Workbooks;
        }

        static private void CloseConnection(Excel.Workbooks connection)
        {
            connection.Close();
        }

        static private Excel.Workbook GetExcelBook(Excel.Workbooks connection, string pathToFile)
        {   
            return connection.Open(pathToFile);
        }

        static private Excel.Worksheet GetExcelSheet(Excel.Workbook excelDoc)
        {
            return excelDoc.ActiveSheet;
        }

        /// <summary>
        /// Преобразовывает сокращённое название компонента к полному.
        /// Например: 
        /// "n-бутан" => "нормальный бутан"
        /// "i-бутан" => "изобутан"
        /// </summary>
        /// <param name="componentName"></param>
        /// <returns></returns>
        static private string ComponentNameConverter(String componentName)
        {
            if (componentName[0] == 'n')
            {
                return "нормальный " + componentName.Substring(2);
            }
            else if (componentName[0] == 'i')
            {
                componentName = "изо" + componentName.Substring(2);
            }

            return componentName;
        }

        //Формирую массив элементов 
        static private List<MyComponent> GetComponents(Excel.Worksheet excelSheet)
        {
            var myComponents = new List<MyComponent>();

            int startRow = 4;
            int currentRow = startRow;

            int componentNameColumn = 1;
            int nkprAirColumn = 4;
            int vkprAirColumn = 5;
            int maxConcAirColumn = 7;

            while (excelSheet.Cells[currentRow, componentNameColumn].Value != null)
            {
                myComponents.Add(new MyComponent(
                    ComponentNameConverter(excelSheet.Cells[currentRow, componentNameColumn].Value.Trim()),
                    excelSheet.Cells[currentRow, nkprAirColumn].Value,
                    excelSheet.Cells[currentRow, vkprAirColumn].Value,
                    excelSheet.Cells[currentRow, maxConcAirColumn].Value
                    ));

                currentRow++;
            }

            return myComponents;
        }

        //Сделал закрытие соединения
        static public List<MyComponent> ParseComponents(string pathToFile)
        {
            var connection = ConnectToExcel();
            var excelDocConnetcion = GetExcelBook(connection, pathToFile);
            var excelSheet = GetExcelSheet(excelDocConnetcion);

            List<MyComponent> components = GetComponents(excelSheet);

            CloseConnection(connection);
            return components;
        }
    }
}
