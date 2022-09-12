using System;
using PGS.UI.Connection;

namespace fill1C
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var fc = new FormConnectionSetting();
            fc.ShowDialog();
            
            var excelHandler = new ExcelHandler();
            var postgresHandler = new PostgreSqlHandler();
            
            //Словарь. Ключ - id газа, значение - id 1C
            var compDict = excelHandler.GetComps();
            postgresHandler.InsertComplectationInDb(compDict);

            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}