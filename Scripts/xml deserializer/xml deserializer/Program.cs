using System;
using System.Xml.Linq;
using PGS;

namespace xml_deserializer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var pathToFile = Environment.CurrentDirectory + "\\correctXML.txt";
            foreach (var order in Order.ParseXML(XDocument.Load(pathToFile).Element("Orders")))
            {
                Console.WriteLine(order.InProduce);
            }
        }
    }
}