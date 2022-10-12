using Microsoft.Win32;
using PInvoke;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PGS.Print
{
    /// <summary>
    /// Класс печати паспортов
    /// </summary>
    public class Printer
    {
        private static System.Windows.Forms.WebBrowser wb;
        private static IWebBrowser2 iwb2;
        private static DWebBrowserEvents2_Event events;

        public bool Busy { get; private set; } = false;

        //Очереди печати
        private Dictionary<PaperSize, List<Passport>> PrinterQueue;

        /// <summary>
        /// Добавляет паспорт в очередь для печати согласно размеру бумаги
        /// </summary>
        public void Append(Passport passport)
        {
            PrinterQueue[passport.PaperSize].Add(passport);
        }

        /// <summary>
        /// Очищает очередь указанного размера бумаги
        /// </summary>
        /// <param name="passport"></param>
        public void Clear(PaperSize paperSize)
        {
            PrinterQueue[paperSize].Clear();
        }

        /// <summary>
        /// Получает коллекцию паспартов из очереди для печати
        /// </summary>
        public ReadOnlyCollection<Passport> GetPassports(PaperSize paperSize)
        {
            return PrinterQueue[paperSize].AsReadOnly();
        }

        /// <summary>
        /// Получает коллекцию паспартов из очереди для печати и ОЧИЩАЕТ очередь указанного размера бумаги
        /// </summary>
        private ReadOnlyCollection<Passport> PopPassports(PaperSize paperSize)
        {
            var passportCollection = PrinterQueue[paperSize].AsReadOnly();
            PrinterQueue[paperSize].Clear();
            return passportCollection;
        }

        private System.Windows.Forms.Control parent;

        public Printer(System.Windows.Forms.Control parent)
        {
            this.parent = parent;

            RegistryKey rk = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");

            string self = System.IO.Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            rk.SetValue(self, 13880, RegistryValueKind.DWord);

            PrinterQueue = new Dictionary<PaperSize, List<Passport>>();

            // Создаём лист паспортов для каждого формата бумаги (У нас их немного (2), поэтому памяти много не займёт)
            foreach (var size in (PaperSize[])Enum.GetValues(typeof(PaperSize)))
            {
                PrinterQueue.Add(size, new List<Passport>());
            }
        }



        /// <summary>
        /// Печать паспартов узананного размера бумаги
        /// </summary>
        /// <param name="paperSize">Формат бумаги (A4, A5)</param>
        /// <param name="duplex">Печать с двух сторон</param>
        public void Print(PaperSize paperSize, bool duplex = false)
        {
            Busy = true;
            if (wb == null)
            {
                wb = new System.Windows.Forms.WebBrowser();

                wb.Parent = parent;

                wb.Dock = DockStyle.Fill;

                //wb.ScriptErrorsSuppressed = true;
            }

            wb.DocumentCompleted += PrintDocument;

            PrinterSetting.SetPrinterProperties(PrinterSetting.StringToPaperSize(paperSize.ToString()), duplex);
            var concatedPassport = ConcatPassports(paperSize);
            using (var file = new System.IO.StreamWriter("temp.html"))
            {
                file.Write(concatedPassport);
            }
            //wb.DocumentText =  concatedPassport;
            wb.Url = new Uri(string.Format("file:///{0}/temp.html", Environment.CurrentDirectory));

            Clear(paperSize);
        }

        /// <summary>
        /// Обработчик события окончания загрузки страницы
        /// </summary>
        private void PrintDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            iwb2 = (IWebBrowser2)wb.ActiveXInstance;
            events = (DWebBrowserEvents2_Event)iwb2;

            events.PrintTemplateTeardown += browser_PrintTemplateTeardown;

            var missing = Type.Missing;

            iwb2.ExecWB(OLECMDID.OLECMDID_PRINTPREVIEW, OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
            ref missing, ref missing);

            wb.DocumentCompleted -= PrintDocument;
        }



        private  void browser_PrintTemplateTeardown(object pDisp)
        {
            events.PrintTemplateTeardown -= browser_PrintTemplateTeardown;
            PrinterSetting.BackPrinterProperties();
            wb = null;
            Busy = false;
        }

        /// <summary>
        /// Печать паспартов узананного размера бумаги с предпросмотром
        /// </summary>
        public void PrintWithPreview(PaperSize paperSize)
        {
            wb = new System.Windows.Forms.WebBrowser();
            wb.ScriptErrorsSuppressed = true;
            wb.DocumentCompleted += PrintPreviewDocument;
            wb.DocumentText = ConcatPassports(paperSize);

        }

        /// <summary>
        /// Событие для Print
        /// </summary>
        private  void PrintPreviewDocument(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var iwb2 = (IWebBrowser2)wb.ActiveXInstance;
            var events = (DWebBrowserEvents2_Event)wb.ActiveXInstance;
            events.PrintTemplateTeardown += browser_PrintTemplateTeardown;
            var missing = Type.Missing;
            iwb2.ExecWB(OLECMDID.OLECMDID_PRINTPREVIEW, OLECMDEXECOPT.OLECMDEXECOPT_DONTPROMPTUSER,
            ref missing, ref missing);
        }

        /// <summary>
        /// Объединение паспартов указанного размера и ОЧИЩАЕТ очередь
        /// </summary>
        private string ConcatPassports(PaperSize paperSize)
        {
            var sb = new StringBuilder();
            foreach (var passport in GetPassports(paperSize))
            {
                sb.Append(passport.Html);
                sb.Append("\n");
            }

            return sb.ToString();
        }


        /// <summary>
        /// Настройки принтера
        /// </summary>
        public static class PrinterSetting
        {
            private static string _printerName = PrinterHelper.GetDefaultPrinterName();

            // Регистры
            private static int _paperSizeTmp; // Размер бумаги
            private static int _duplexTmp;    // Двухсторонняя печать

            public static string PrinterName
            {
                get
                {
                    return _printerName;
                }
                set
                {
                    _printerName = value;
                }
            }

            public enum PaperSize
            {
                A4 = 9, // A4 210 x 297 mm
                A5 = 11 // A5 148 x 210 mm
            }

            /// <summary>
            /// Конвертирует строку в enum PaperSize
            /// </summary>
            public static PaperSize StringToPaperSize(string paperSize)
            {
                switch (paperSize.ToLower())
                {
                    case "a4":
                        return PaperSize.A4;
                    case "a5":
                        return PaperSize.A5;
                    default:
                        throw new Exception("Несуществующий формат");
                }

            }

            /// <summary>
            /// Сохраняет текущие настройки принтера
            /// </summary>
            static PrinterSetting()
            {
                SavePrinterProperties();
            }

            /// <summary>
            /// Сохранение текущих настроек принтера
            /// </summary>
            private static void SavePrinterProperties()
            {
                var currentProperties = PrinterHelper.GetPrinterDevMode(_printerName);
                _paperSizeTmp = currentProperties.dmPaperSize;
                _duplexTmp = currentProperties.dmDuplex;
            }

            /// <summary>
            /// Настройка принтера
            /// </summary>
            public static void SetPrinterProperties(PaperSize paperSize, bool duplex)
            {
                IESetup();
                var settings = new PrinterHelper.PrinterSettingsInfo();
                settings.Size = (PrinterHelper.PaperSize)paperSize;
                settings.Duplex = (PrinterHelper.PageDuplex)(duplex == true ? 2 : 1);
                PrinterHelper.ModifyPrinterSettings(_printerName, ref settings);
            }

            /// <summary>
            /// Возвращает предыдущие настройки
            /// </summary>
            public static void BackPrinterProperties()
            {
                var settings = new PrinterHelper.PrinterSettingsInfo();
                settings.Size = (PrinterHelper.PaperSize)_paperSizeTmp;
                settings.Duplex = (PrinterHelper.PageDuplex)_duplexTmp;
                PrinterHelper.ModifyPrinterSettings(_printerName, ref settings);
            }

            /// <summary>
            /// Настройка реестра для печати с использованием IE 7 
            /// </summary>
            private static void IESetup()
            {
                // Настройка, создание записей реестра обязательна.
                // Без неё появляются разные "артефакты" при печати по типу ссылки на файл
                const string regPath = "Software\\Microsoft\\Internet Explorer\\PageSetup";
                using (var regKey = Registry.CurrentUser.OpenSubKey(regPath, true))
                {
                    if (regKey == null) return;
                    regKey.SetValue("footer", "");
                    regKey.SetValue("header", "");
                    regKey.SetValue("Print_Background", "no");
                    regKey.SetValue("Shrink_To_Fit", "yes");
                }
            }
        }
    }
}