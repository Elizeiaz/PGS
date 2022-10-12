using PGS.Print;
using PGS.Print.HTML;
using PGS.Standarts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace TestApp
{
    public partial class Form1 : Form
    {
        #region Старое тестирование
        /*
                public TestPassport GroupFirstTestMix()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\group\group.html";
                    passport.PaperSize = PaperSize.A4; // Обязательно указанае размера бумаги
                    passport.RepeatCount = 4;
                    passport.PassportName = "КИСЛОРОД ГАЗООБРАЗНЫЙ";
                    passport.NameCharacteristic = "Особой чистоты";
                    passport.TechNumber = "ТУ 2114-001-05798345-2007";
                    passport.OrderRecipient = "АО \"Уфимская Химическаяя Компания\"";
                    passport.ShipmentDate = new DateTime(2021, 06, 02);
                    passport.CylNumber = new[] { "31456", "4093" };
                    passport.CylVolume = 4;
                    passport.AnalysDate = new DateTime(2021, 05, 26);
                    passport.ValidToDate = new DateTime(2023, 05, 26);
                    return passport;
                }

                public TestPassport GroupSecondTestMix()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\group\group.html";
                    passport.PaperSize = PaperSize.A4; // Обязательно указанае размера бумаги
                    passport.RepeatCount = 10;
                    passport.PassportName = "КИСЛОРОД";
                    passport.NameCharacteristic = "Особой чистоты";
                    passport.TechNumber = "ТУ 2114-001-05798345-2007";
                    passport.OrderRecipient = "НИИ \"УрФУ\"";
                    passport.ShipmentDate = new DateTime(2021, 06, 02);
                    passport.CylNumber = new[] { "23136", "4533" };
                    passport.CylVolume = 4;
                    passport.AnalysDate = new DateTime(2019, 05, 26);
                    passport.ValidToDate = new DateTime(2021, 05, 26);
                    return passport;
                }

                public TestPassport SingleFirstTestMix()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\single\single.html";
                    passport.PaperSize = PaperSize.A4; // Обязательно указанае размера бумаги
                    passport.RepeatCount = 3;
                    passport.OrderNumber = 4324;
                    passport.PassportName = "АРГОН ГАЗООБРАЗНЫЙ";
                    passport.NameCharacteristic = "марка 5.6";
                    passport.CylNumber = new[] { "175709" };
                    passport.CylVolume = 40;
                    passport.TechNumber = "ТУ 2114-005-53373468-2006";


                    passport.AnalysDate = new DateTime(2021, 06, 02);
                    passport.ValidToDate = new DateTime(2023, 06, 02);
                    return passport;
                }

                public TestPassport SingleSecondTestMix()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\single\single.html";
                    passport.PaperSize = PaperSize.A4; // Обязательно указанае размера бумаги
                    passport.RepeatCount = 3;
                    passport.OrderNumber = 4324;
                    passport.PassportName = "АРГОН ГАЗООБРАЗНЫЙ";
                    passport.NameCharacteristic = "марка 5.6";
                    passport.CylNumber = new[] { "25724" };
                    passport.CylVolume = 40;
                    passport.TechNumber = "ТУ 2114-005-53373468-2006";


                    passport.AnalysDate = new DateTime(2021, 06, 02);
                    passport.ValidToDate = new DateTime(2023, 06, 02);
                    return passport;
                }

                public TestPassport A5TestMix()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\2015\front\grad.html";
                    passport.PaperSize = PaperSize.A5; // Обязательно указанае размера бумаги

                    passport.OrderNumber = 7154;
                    passport.PassportNumber = "П-49795";
                    passport.CylNumber = new[] { "7154" };
                    passport.CylVolume = 4;
                    passport.CylPressure = "7,5";
                    passport.Mintemperature = -30;
                    passport.TechNumber = "ТУ 2114-006-53373468-2008";
                    passport.IsToxic = false;
                    passport.IsFlammable = true;
                    passport.AnalysDate = new DateTime(2021, 05, 06);
                    passport.ValidToDate = new DateTime(2022, 05, 06);
                    passport.CylClass = "Первый";
                    passport.SampleNumber = "СОП 15.01-11";

                    passport.CompList = new System.Collections.Generic.List<TestComp>()
                    {
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                    };

                    passport.RepeatCount = passport.CompList.Count;
                    return passport;
                }

                public TestPassport A5TestMixMiddle()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\a5\front\tech.html";
                    passport.PaperSize = PaperSize.A5; // Обязательно указанае размера бумаги

                    passport.OrderNumber = 7154;
                    passport.PassportNumber = "П-49795";
                    passport.CylNumber = new[] { "7154" };
                    passport.CylVolume = 4;
                    passport.CylPressure = "7,5";
                    passport.Mintemperature = -30;
                    passport.TechNumber = "ТУ 2114-006-53373468-2008";
                    passport.IsToxic = false;
                    passport.IsFlammable = true;
                    passport.AnalysDate = new DateTime(2021, 05, 06);
                    passport.ValidToDate = new DateTime(2022, 05, 06);
                    passport.CylClass = "Первый";
                    passport.SampleNumber = "СОП 15.01-11";

                    passport.CompList = new System.Collections.Generic.List<TestComp>()
                    {
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                    };

                    passport.RepeatCount = passport.CompList.Count;
                    return passport;
                }

                public TestPassport A5TestMixSmall()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\2021\back\standart.html";

                    passport.OrderNumber = 7154;
                    passport.PassportNumber = "П-49795";
                    passport.CylNumber = new[] { "7154" };
                    passport.CylVolume = 4;
                    passport.CylPressure = "7,5";
                    passport.Mintemperature = -30;
                    passport.TechNumber = "ТУ 2114-006-53373468-2008";
                    passport.IsToxic = false;
                    passport.IsFlammable = true;
                    passport.AnalysDate = new DateTime(2021, 05, 06);
                    passport.ValidToDate = new DateTime(2022, 05, 06);
                    passport.CylClass = "Первый";
                    passport.SampleNumber = "СОП 15.01-11";
                    passport.GosNumber = "ГСО 123432-2323";
                    passport.IntNumber = "МСО 123123-4321";
                    passport.Certified = "ПГС аттестована с использованием рабочего эталона 1 разряда РЭ 154-1-23-2005. Регистрационный номер 3.6.ПГС.0001.2015. Прослеживаемый к ГЭТ 154";
                    passport.StandartType = "Поверочная газовая смесь соответствует утвержденному типу стандартного образца искусственной газовой смеси углеводородов(ИПГ - П - 1)";

                    passport.CompList = new System.Collections.Generic.List<TestComp>()
                    {
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                    };

                    passport.RepeatCount = passport.CompList.Count;
                    return passport;
                }


                public TestPassport sflyMiddleFront()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\gases\sfly\front.html";
                    passport.PaperSize = PaperSize.A4; // Обязательно указанае размера бумаги

                    passport.OrderNumber = 7154;
                    passport.PassportNumber = "П-49795";
                    passport.CylNumber = new[] { "7154" };
                    passport.CylVolume = 4;
                    passport.CylPressure = "7,5";
                    passport.Mintemperature = -30;
                    passport.TechNumber = "ТУ 2114-006-53373468-2008";
                    passport.IsToxic = false;
                    passport.IsFlammable = true;
                    passport.AnalysDate = new DateTime(2021, 05, 06);
                    passport.ValidToDate = new DateTime(2022, 05, 06);
                    passport.CylClass = "Первый";
                    passport.SampleNumber = "СОП 15.01-11";
                    passport.GosNumber = "ГСО 123432-2323";
                    passport.IntNumber = "МСО 123123-4321";
                    passport.Certified = "ПГС аттестована с использованием рабочего эталона 1 разряда РЭ 154-1-23-2005. Регистрационный номер 3.6.ПГС.0001.2015. Прослеживаемый к ГЭТ 154";
                    passport.StandartType = "Поверочная газовая смесь соответствует утвержденному типу стандартного образца искусственной газовой смеси углеводородов(ИПГ - П - 1)";

                    passport.CompList = new System.Collections.Generic.List<TestComp>()
                    {
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4)
                    };

                    passport.RepeatCount = passport.CompList.Count;
                    return passport;
                }

                public TestPassport sflyMiddleBack()
                {
                    var passport = new TestPassport();
                    passport.pathToTemplate = @"\html\gases\sfly\back.html";
                    passport.PaperSize = PaperSize.A4; // Обязательно указанае размера бумаги

                    passport.OrderNumber = 7154;
                    passport.PassportNumber = "П-49795";
                    passport.CylNumber = new[] { "7154" };
                    passport.CylVolume = 4;
                    passport.CylPressure = "7,5";
                    passport.Mintemperature = -30;
                    passport.TechNumber = "ТУ 2114-006-53373468-2008";
                    passport.IsToxic = false;
                    passport.IsFlammable = true;
                    passport.AnalysDate = new DateTime(2021, 05, 06);
                    passport.ValidToDate = new DateTime(2022, 05, 06);
                    passport.CylClass = "Первый";
                    passport.SampleNumber = "СОП 15.01-11";
                    passport.GosNumber = "ГСО 123432-2323";
                    passport.IntNumber = "МСО 123123-4321";
                    passport.Certified = "ПГС аттестована с использованием рабочего эталона 1 разряда РЭ 154-1-23-2005. Регистрационный номер 3.6.ПГС.0001.2015. Прослеживаемый к ГЭТ 154";
                    passport.StandartType = "Поверочная газовая смесь соответствует утвержденному типу стандартного образца искусственной газовой смеси углеводородов(ИПГ - П - 1)";

                    passport.CompList = new System.Collections.Generic.List<TestComp>()
                    {
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("C10H22", 0.5, 3),
                        new TestComp("trans-C5H12", 19.5, 1.32),
                        new TestComp("cis-C4H10", 51.5, 0.9),
                        new TestComp("C4H10", 12.5, 3.4),
                        new TestComp("CH4", 21.5, 12.7),
                        new TestComp("i-C4H8", 4.5, 0.4),
                        new TestComp("C4H10", 12.5, 3.4)
                    };

                    passport.RepeatCount = passport.CompList.Count;
                    return passport;
                }
                */
        #endregion

        #region Тестирование через Excel

        static String pathToExcel = "Tester.xlsx";

        // Виды паспортов, где значение - номер колонки в excel
        private enum Tester
        {
            pgsOld = 3,
            pgsNew = 4,
            gradOld = 5,
            gradNew = 6,
            tech = 7,
            backStandartOld = 8,
            backStandartNew = 9,
            backParamsOld = 10,
            backParamsNew = 11,
            backParamsValume = 12,
            group = 13,
            groupTech = 14,
            sfly = 15,
            sflyBack = 16,
            single = 17,
            sflyOld = 18,
            sflyBackOld = 19
        }



        static Excel.Worksheet ws = null;
        private Dictionary<string, string> GetDictFromExcel(Tester tester)
        {
            var dict = new Dictionary<string, string>();

            var currentRow = 2;

            ws = new Excel.Application().Workbooks.Open(Environment.CurrentDirectory + "\\" + pathToExcel).ActiveSheet;
            while (ws.Cells[currentRow, 1].Value != null)
            {
                var key = ws.Cells[currentRow, 1].Value + "";

                var curValue = ws.Cells[currentRow, (int)tester].Value;


                if (curValue is DateTime)
                    curValue = ((DateTime)curValue).ToShortDateString(); // Обработка даты
                currentRow++;

                if (curValue is string)
                    curValue = Builder.ToHtml.CompFormulaToHTML(curValue); // Обработка индексов формул компонентов

                if (key == "Путь")
                {
                    curValue = System.IO.Path.Combine(Environment.CurrentDirectory, curValue);
                }

                dict.Add(key, ((string)(curValue + "")).Trim());

            }

            return dict;
        }

        #endregion

        Printer printer;

        public Form1()
        {

            InitializeComponent();
            #region Старое тестирование
            // Тестовый класс
            //var mix1 = GroupFirstTestMix();
            //var mix2 = Gr oupSecondTestMix();
            //var mix3 = SingleFirstTestMix();
            //var mix4 = SingleSecondTestMix();
            //var mix5 = A5TestMix();
            //var mix6 = A5TestMixMiddle();
            //var mix7 = A5TestMixSmall();
            //var mix8 = sflyMiddleFront();
            //var mix9 = sflyMiddleBack();


            //var group1 = PassportClass.GetPassport(mix1);
            //var group2 = PassportClass.GetPassport(mix2);
            //var single1 = PassportClass.GetPassport(mix3);
            //var single2 = PassportClass.GetPassport(mix4);
            //var a5Span = PassportClass.GetPassport(mix5);
            //var a5Middle = PassportClass.GetPassport(mix6);
            //var a5Small = PassportClass.GetPassport(mix7);
            //var sflyMid = PassportClass.GetPassport(mix8);
            //var sflyMidBack = PassportClass.GetPassport(mix9);

            //var printer = new Printer();

            //printer.Append(single1);
            //printer.Append(single2);
            //printer.Append(single2);
            //printer.Append(group1);
            //printer.Append(single2);
            //printer.Append(group2);
            //printer.Append(single2);
            //printer.Append(a5Span);
            //printer.Append(a5Span);
            //printer.Append(a5Middle);
            //printer.Append(a5Small);
            //printer.Append(a5Small);

            //printer.Append(sflyMid);
            //printer.Append(sflyMidBack);
            #endregion

            #region
            /*

                        var pgsOldDict = GetDictFromExcel(Tester.pgsOld);
                        var pgsOldPassport = TestPassport.GetPassport(pgsOldDict);

                        var pgsNewDict = GetDictFromExcel(Tester.pgsNew);
                        var pgsNewPassport = TestPassport.GetPassport(pgsNewDict);

                        var htmlText = pgsNewPassport.Html; // Беру html собранного паспорта

                        var gradOldDict = GetDictFromExcel(Tester.gradOld);
                        var gradOldPassport = TestPassport.GetPassport(gradOldDict);

                        var graNewDict = GetDictFromExcel(Tester.gradNew);
                        var gradNewPassport = TestPassport.GetPassport(graNewDict);

                        var techDict = GetDictFromExcel(Tester.tech);
                        var techPassport = TestPassport.GetPassport(techDict);

                        var backStandartOldDict = GetDictFromExcel(Tester.backStandartOld);
                        var backStandartOldPassport = TestPassport.GetPassport(backStandartOldDict);

                        var backStandartNewDict = GetDictFromExcel(Tester.backStandartNew);
                        var backStandartNewPassport = TestPassport.GetPassport(backStandartNewDict);

                        var backParamsOldDict = GetDictFromExcel(Tester.backParamsOld);
                        var backParamsOldPassport = TestPassport.GetPassport(backParamsOldDict);

                        var backParamsNewDict = GetDictFromExcel(Tester.backParamsNew);
                        var backParamsNewPassport = TestPassport.GetPassport(backParamsNewDict);

                        var backParamsValumeDict = GetDictFromExcel(Tester.backParamsValume);
                        var backParamsValumePassport = TestPassport.GetPassport(backParamsValumeDict);

                        var GroupDict = GetDictFromExcel(Tester.group);
                        var GroupPassport = TestPassport.GetPassport(GroupDict);

                        var GroupTechDict = GetDictFromExcel(Tester.groupTech);
                        var GroupTechPassport = TestPassport.GetPassport(GroupTechDict);

                        var SflyDict = GetDictFromExcel(Tester.sfly);
                        var SflyPassport = TestPassport.GetPassport(SflyDict);

                        var backSflyDict = GetDictFromExcel(Tester.sflyBack);
                        var backSflyPassport = TestPassport.GetPassport(backSflyDict);

                        var singleDict = GetDictFromExcel(Tester.single);
                        var singlePassport = TestPassport.GetPassport(singleDict);

                        //var SflyOldDict = GetDictFromExcel(Tester.sflyOld);
                        //var SflyOldPassport = PassportClass.GetPassport(SflyOldDict);

                        //var backSflyOldDict = GetDictFromExcel(Tester.sflyBackOld);
                        //var backSflyOldPassport = PassportClass.GetPassport(backSflyOldDict);


                    //    printer.Append(pgsOldPassport);
                      //  printer.Append(backStandartOldPassport);

                     //   printer.Append(pgsNewPassport);
                     //   printer.Append(backStandartNewPassport);

                        //printer.Append(gradOldPassport);
                        //printer.Append(backParamsOldPassport);

                        printer.Append(gradNewPassport);
                        Clipboard.SetText(gradNewPassport.Html);
                        //printer.Append(backParamsNewPassport);

                        //printer.Append(techPassport);
                        //printer.Append(backParamsValumePassport);

                        //printer.Append(GroupPassport);
                        //printer.Append(GroupTechPassport);

                    //    printer.Append(SflyPassport);
                      //  printer.Append(backSflyPassport);

                        //printer.Append(SflyOldPassport);
                        //printer.Append(backSflyOldPassport);

                        //printer.Append(singlePassport);
                        //printer.Append(singlePassport);
                        //printer.Append(singlePassport);
                        */
            #endregion

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            button2_Click(null, null);
            //Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var m = PGS.Customers.Mix.Load(6934); //6934 //7018
            Dictionary<string, string> data = new Dictionary<string, string>();
            data = m.GetPasportData("ГОСТ 31369-2008");

            StringBuilder sb = new StringBuilder();
            foreach (string  key in data.Keys)
            {
                sb.Append(key + "=" + data[key] + "\r\n");
            }
            Clipboard.SetText(sb.ToString());
            COMPrinter cp = new COMPrinter();
            cp.AddPage(8, sb.ToString());
            cp.AddPage(11, sb.ToString());
            cp.AddPage(7, sb.ToString());
            cp.AddPage(12, sb.ToString());
            cp.doPrint();
            return;


            printer = new Printer(this);

            var template = Standart.TemplateInfo.Load(3);//3 - Тех. 8 - ГСО 9 - СОП 5 - оборот


            /*int k = 0;
            while (m.Comps.Count < 23)
            {
                m.Comps.Add(m.Comps[k]);
                k++;
            }
            */

            
            
            //string[] counts  = new string[]{ "2", "4", "7", "8"};
            //string[] counts = new  string[]{/* "7",*/ "8", "11", "14"};
           // string[] counts = new string[]{ "15", "19", "23"};
           // string[] counts = new string[] {"2", "4", "7", "8", "11", "14", "15", "19", "22" };
            //Для 1С
            string[] counts = new string[] { "2", "8", "15"};
            for (int i = 0; i < counts.Length; i++)
            {
                data["[CompCount]"] = counts[i];
                PGS.Print.Passport p = new PGS.Print.Passport(data, template);
                printer.Append(p);
            }

            if (printer.GetPassports(PGS.Print.PaperSize.A4).Count > 0)
                printer.Print(PGS.Print.PaperSize.A4);
            if (printer.GetPassports(PGS.Print.PaperSize.A5).Count > 0)
                printer.Print(PGS.Print.PaperSize.A5);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}