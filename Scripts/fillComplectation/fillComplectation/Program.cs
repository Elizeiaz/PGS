using System;
using fillComplectation.Properties;
using PGS.UI.Connection;

namespace fillComplectation
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var fc = new FormConnectionSetting();
            fc.ShowDialog();

            var excelHandler = new ExcelHandler();
            var postgresHandler = new PostgreSqlHandler();
            
            var complectationList = excelHandler.GetComplectations();
            postgresHandler.InsertComplectationInDb(complectationList);

            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}