using PGS.Print;
using PGS.Print.HTML;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TestApp
{
    public class TestComp
    {
        public string Formula;
        public double Molar;
        public double Coef;

        public TestComp(string formula, double molar, double coef)
        {
            Formula = formula;
            Molar = molar;
            Coef = coef;
        }
    }

    public class TestPassport
    {
        public List<TestComp> CompList = new List<TestComp>();

        public string pathToTemplate;
        public PaperSize PaperSize;

        public int RepeatCount = 0;
        public int OrderNumber = 9999;
        public string PassportNumber = "";
        public string PassportName = "";
        public string NameCharacteristic = "";
        public string TechNumber = "";
        public string OrderRecipient = "";
        public DateTime ShipmentDate = DateTime.MinValue;
        public string[] CylNumber;
        public double CylVolume = 0;
        public string CylPressure = "";
        public bool IsToxic = false;
        public bool IsFlammable = false;
        public string UnitTitle = "";
        public string UnitName = "";
        public string CylClass = "";
        public string StandartType = "";
        public string Certified = "";
        public string SampleNumber = "";
        public int Mintemperature = 0;
        public string GosNumber = "";
        public string IntNumber = "";

        public Dictionary<string, string> testOtherDict = new Dictionary<string, string> { };

        public int CylCount
        {
            get { return CylNumber.Length; }
        }

        public DateTime FillDate = DateTime.MinValue;
        public DateTime AnalysDate = DateTime.MinValue;
        public DateTime ValidToDate = DateTime.MinValue;

        public TestPassport()
        {
        }

        /// <summary>
        /// Упаковка словаря 
        /// </summary>
        /// <param name="mixClass">Класс компонента</param>
        /// <returns>Словарь, где ключ - аргумент в html, значение - значение к подстановке</returns>
        public static Dictionary<string, string> PackDictionary(TestPassport mixClass)
        {
            var dict = new Dictionary<string, string>()
            {
                {"pathToTemplate", AsString(mixClass.pathToTemplate)}, // Путь к шаблону
                {"[repeatCount]", AsString(mixClass.RepeatCount)}, // Число повторений паттерна Repeat  

                {"[orderNumber]", AsString(mixClass.OrderNumber)}, // Номер заказа
                {"[passportName]", AsString(mixClass.PassportName)}, // Имя паспорта П: Аргон газообразный
                {"[passportNumber]", AsString(mixClass.PassportNumber)}, // Номер паспорта
                {"[nameCharacteristic]", AsString(mixClass.NameCharacteristic)}, // Характеристика имени П: марка 5.6
                {"[techNumber]", AsString(mixClass.TechNumber)}, // ТУ
                {"[orderRecipient]", AsString(mixClass.OrderRecipient)}, // Грузополучатель
                {"[shipmentDate]", AsString(mixClass.ShipmentDate)}, // Дата отгрузки
                {"[cylNumber]", AsString(mixClass.CylNumber)}, // Номер баллона (Номера)
                {"[cylVolume]", AsString(mixClass.CylVolume)}, // Ёмкость баллона
                {"[pressure]", AsString(mixClass.CylPressure)}, // Давление
                {"[minTemperature]", AsString(mixClass.Mintemperature)}, // Давление
                {"[cylPressure]", "14,7±0,5 МПа(150±5 ат)"}, // Давление в баллоне
                {"[cylCount]", AsString(mixClass.CylCount)}, // Количество баллонов
                {"[isToxic]", AsString(mixClass.IsToxic)}, // Есть токсичный компонент
                {"[isFlammable]", AsString(mixClass.IsFlammable)}, // Воспламеняемый
                {"[analyseDate]", AsString(mixClass.AnalysDate)}, // Дата анализа
                {"[validToDate]", AsString(mixClass.ValidToDate)}, // Действителен по
                {"[unitTitle]", AsString(mixClass.UnitTitle)}, // Имя единиц измерения П: "Молярная масса"
                {"[unitName]", Builder.ToHtml.ComUnitNameToHTML(AsString(mixClass.UnitName))}, // Обозначение единиц измерения П: "%"
                {"[cylClass]", AsString(mixClass.CylClass)}, // Разряд П: "Первый"
                {"[certified]", AsString(mixClass.Certified)}, // ПГС аттестована с использованием рабочего эталона...
                {"[standartType]", AsString(mixClass.Certified)}, // Поверочная газовая смесь соответствует утверждённому типу...
                {"[sampleNumber]", AsString(mixClass.SampleNumber)}, // СОП
                {"[gosNumber]", AsString(mixClass.GosNumber)}, // ГСО
                {"[intNumber]", AsString(mixClass.IntNumber)}, // МСО
            };

            return dict;
        }


        /// <summary>
        /// Конвертация компонентов в словарь
        /// </summary>
        private static Dictionary<string, string> CompsToDict(List<TestComp> compsList)
        {
            var dict = new Dictionary<string, string>() { };

            for (var i = 0; i < compsList.Count; i++)
            {
                dict.Add("[formula" + i + "]", Builder.ToHtml.CompFormulaToHTML(compsList[i].Formula));
                dict.Add("[percent" + i + "]", compsList[i].Molar.ToString());
                dict.Add("[uncertainty" + i + "]", compsList[i].Coef.ToString());
            }

            return dict;
        }
        /// <summary>
        /// Объединение 2 словарей
        /// </summary>
        /// <param name="firstDict"></param>
        /// <param name="secondDict"></param>
        /// <returns></returns>
        private static Dictionary<string, string> UnionDicts(Dictionary<string, string> firstDict,
            Dictionary<string, string> secondDict)
        {
            return firstDict.Union(secondDict).ToDictionary(x => x.Key, x => x.Value);
        }

        // Формирует паспорт на основе входного класса
        public static Passport GetPassport(Dictionary<string, string> dict)
        {
            PaperSize papersize = (PaperSize)Enum.Parse(typeof(PaperSize), dict["Размер бумаги"], true);
            string path = dict["Путь"];
            string html = Builder.Build(path, dict);
            return new Passport(papersize, html);
        }

        #region AsString методы

        private static string AsString(int value)
        {
            return value.ToString();
        }

        private static string AsString(int[] value)
        {
            return string.Join(", ", value);
            ;
        }

        private static string AsString(string value)
        {
            return value;
        }

        private static string AsString(string[] value)
        {
            return string.Join(", ", value);
            ;
        }

        private static string AsString(double value)
        {
            return value.ToString(CultureInfo.CurrentCulture);
        }

        private static string AsString(double[] value)
        {
            return string.Join(", ", value);
            ;
        }

        private static string AsString(DateTime value)
        {
            return value.ToShortDateString();
        }

        private static string AsString(bool value)
        {
            return value == true ? "Да" : "Нет";
        }

        #endregion

    }

}