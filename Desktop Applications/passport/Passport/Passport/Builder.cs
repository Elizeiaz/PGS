using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

/// <summary>
/// Средства для работы с HTML
/// </summary>
namespace PGS.Print.HTML
{
    /// <summary>
    /// Представляет сборщик HTML шаблонов для печати
    /// </summary>
    public static class Builder
    {
        // Компиляция Regex долгая вещь, поэтому создаю статическую переменную (Компилирую заранее и всего один раз)
        private readonly static Regex _paramsRegex = new Regex(@"\[[\d\D]+?\]");                          // Ищет все выражения типа ["НазваниеКлюча"]
        private readonly static Regex _compFormulaConverter = new Regex(@"(?<value>[CHONSF]{1}[e]{0,1})(?<num>\d+)");

        // Названия файлов со стилями. Должны лежать в папке с html!
        private const string littleCss = "little.css";
        private const string middleCss = "middle.css";
        private const string bigCss = "big.css";

        // Границы применения шаблонов (До, не включительно)
        private const int bigEdge = 8;  // 0 <= i < 8
        private const int middleEdge = 15; // 8 <= i < 15

        /// <summary>
        /// Выбор css стиля для использования
        /// </summary>
        private static string SelectCssStyle(int count)
        {
            if (count < bigEdge)
                return bigCss;

            if (count < middleEdge)
                return middleCss;

            return littleCss;
        }


        public static string Build(string template, Dictionary<string, string> dict)
        {
            var text = ReadFile(template);
            string s_count = dict["[CompCount]"];
            if (s_count != "")
            {
                int count = int.Parse(s_count);

                string file_css = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(template), SelectCssStyle(count));
                text = text.Replace("[css]", file_css);
            }

            text = text.Replace("[CompCount]", dict["[CompCount]"]);
            if (dict.ContainsKey("[Cyl.New]"))
                text = text.Replace("[Cyl.New]", dict["[Cyl.New]"]);

            text = Replace(text, dict);

            var xml = StringToXml(text);
            Patterns.DoPatterns(xml);
            var html = XmlToString(xml);

            html = Replace(html, dict);

            html = PreMailer.Net.PreMailer.MoveCssInline(html, true).Html; // Inliner

            return html;
        }


        private static string ReadFile(string filename)
        {
            if (!File.Exists(filename))
                throw new IOException(string.Format("Файл '{0}' не существует", filename));
            return File.ReadAllText(filename);
        }

        private static XElement StringToXml(string text)
        {
            try
            {
                return XElement.Parse(text);
            }
            catch (System.Xml.XmlException e)
            {
                throw new XmlSyntaxException("Ошибка в синтаксисе html \n(Все одиночные узлы необходимо закрывать)\n " + e.Message);
            }

        }

        private static string XmlToString(XElement xml)
        {
            return xml.ToString();
        }


        /// <summary>
        /// Замена старого значения на новое, где старое - ключь, новое - значение словаря
        /// </summary>
        public static string Replace(string sb, Dictionary<string, string> dict)
        {
            foreach (var entry in dict) sb = sb.Replace(entry.Key, entry.Value);

            return sb;
        }

        /// <summary>
        /// Улучшенный метод замены. Делает замену только тем ключам из словаря, которые есть во входном тексте
        /// </summary>
        private static StringBuilder AdvancedReplace(StringBuilder sb, Dictionary<string, string> dict)
        {
            foreach (var match in _paramsRegex.Matches(sb.ToString()))
            {
                var key = match.ToString();
                if (!dict.ContainsKey(key)) throw new Exception("Отсутствует ключ: " + key);
                sb.Replace(key, dict[key]);
            }

            return sb;
        }

        /// <summary>
        /// Получает список параметров типа ["НазваниеКлюча"]
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static List<string> GetParams(string template)
        {
            List<string> r = new List<string>();
            foreach (var match in _paramsRegex.Matches(template))
            {
                var key = match.ToString();
                if (!r.Contains(key))
                {
                    if (key.StartsWith("[."))
                        continue;
                    r.Add(key);
                }

            }
            return r;
        }


        /// <summary>
        /// Получает список параметров типа ["НазваниеКлюча"]
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public static List<string> GetOptionalParams(string template)
        {
            List<string> r = new List<string>();
            foreach (var match in _paramsRegex.Matches(template))
            {
                var key = match.ToString();
                if (!r.Contains(key))
                {
                    if (key.StartsWith("[@"))
                        r.Add(key);
                }

            }
            return r;
        }


        /// <summary>
        /// Вспомогательный класс для работы с html
        /// </summary>
        public static class ToHtml
        {
            /// <summary>
            /// Заменяет все пробелы ссылки на пробел в закодированом виде
            /// </summary>
            private static StringBuilder SpacesConverter(StringBuilder sb)
            {
                return sb.Replace(" ", "%20");
            }

            /// <summary>
            /// Заменяет все пробелы ссылки на пробел в закодированом виде
            /// </summary>
            private static string SpacesConverter(string str)
            {
                return str.Replace(" ", "%20");
            }

            /// <summary>
            /// Обособляет все цифры тегом sub
            /// </summary>
            public static string CompFormulaToHTML(string compFormula)
            {
                var replacedStr = _compFormulaConverter.Replace(compFormula, match => match.Groups["value"] + "<sub>" + match.Groups["num"].Value + "</sub>");
                return replacedStr;
            }

            /// <summary>
            /// Обособляет все цифры тегом sup
            /// </summary>
            public static string ComUnitNameToHTML(string compFormula)
            {
                var replacedStr = Regex.Replace(compFormula, @"[-\d]+", match => "<sup>" + match + "</sup>");
                return replacedStr;
            }
        }

        /// <summary>
        /// Описание и реализация дополнительных тэгов (паттернов)
        /// </summary>
        private static class Patterns
        {

            // Компиляция Regex долгая вещь, поэтому создаю статическую переменную (Компилирую заранее и всего один раз)
            private readonly static Regex _paramsWithNumberRegex = new Regex(@"\[(?<name>.*?)\{(?<num>\d+)\}\]"); // Ищет все выражения с повторяющимися параметрами [paramName0]

            /// <summary>
            /// Лист служит "корзиной" для xml узлов.
            /// Например, после реализации паттерна Repeat узел нужно удалить
            /// </summary>
            private static List<XElement> trash = new List<XElement>();

            /// <summary>
            /// Ищет тег papersize и возвращает его значение
            /// </summary>
            public static string GetPaperSize(XElement xml)
            {
                foreach (var node in xml.Descendants())
                {
                    if (node.Name != "papersize") continue;

                    if (node.HasElements) throw new XmlSyntaxException("Неверный синтаксис тега <papersize>\"Value\"</papersize>");
                    var paperSize = node.Value;
                    node.Remove();

                    return paperSize;
                }

                throw new Exception("В шаблоне отсутствует тег <papersize>\"Value\"</papersize>");
            }

            /// <summary>
            /// Находит все паттерны и реализует их
            /// </summary>
            public static void DoPatterns(XElement xml)
            {
                foreach (var node in xml.Descendants())
                    switch (node.Name.ToString())
                    {
                        case "repeat":
                            Repeat(node);
                            break;
                        case "include":
                            Include(node);
                            break;
                        case "optional":
                            Optional(node);
                            break;
                        default:
                            continue;
                    }

                DeleteNodes();
            }

            private static void Optional(XElement node)
            {
                var field = node.Attribute("field");

                if (field == null)
                    throw new XmlSyntaxException("Атрибут field обязателен");


                if (field.Value != "")
                {
                    
                    var content = new XElement(node).Nodes();
                    foreach (XNode repeatNode in content)
                    {
                        if (repeatNode is XElement)
                        {
                            var tmp = new XElement(repeatNode as XElement);
                            node.AddBeforeSelf(tmp);
                        } else
                        {
                            node.AddBeforeSelf(repeatNode.ToString());
                        }
                    }
                }
                trash.Add(node);
            }

            /// <summary>
            /// Удаление узлов из "корзины"
            /// </summary>
            private static void DeleteNodes()
            {
                foreach (var node in trash) node.Remove();
                trash.Clear();
            }

            /// <summary>
            /// Увеличивает значения узлов в соответствии с регулярным выражением
            /// </summary>
            private static void IncreaseValue(XElement node, Regex reg, int increaseValue)
            {
                if (!node.HasElements)
                {
                    node.Value = reg.Replace(node.Value, match =>
                        "[" + match.Groups["name"] + (int.Parse(match.Groups["num"].Value) + increaseValue) + "]");
                    return;
                }

                foreach (var nd in node.Elements()) IncreaseValue(nd, reg, increaseValue);
            }

            /// <summary>
            /// Повторяет содержимое repeat count='i'>.../repeat> i раз
            /// </summary>
            private static void Repeat(XElement node)
            {
                var attrCount = node.Attribute("count");
                if (attrCount == null)
                    throw new NullReferenceException("Отсутствует аттрибут count");

                var count = int.Parse(attrCount.Value);
                if (count < 0)
                    throw new Exception("Значение аргумента count должно быть положительным");

                var repeatNodes = new XElement(node).Nodes();

                for (var i = 0; i < count; i++)
                    foreach (XElement repeatNode in repeatNodes)
                    {
                        var tmp = new XElement(repeatNode);
                        IncreaseValue(tmp, _paramsWithNumberRegex, i);

                        node.AddBeforeSelf(tmp);
                    }

                trash.Add(node);
            }

            /// <summary>
            /// Вставляет вместо include
            ///     href="..."> содержимое файла указанного в href (Указывается абсолютный путь)
            ///     text="..."> содержимое аттрибута text
            /// </summary>
            private static void Include(XElement node)
            {
                if (node.HasElements) throw new XmlSyntaxException("Неверный синтаксис паттерна include");

                var href = node.Attribute("href");
                var text = node.Attribute("text");

                if (href != null && text != null)
                    throw new XmlSyntaxException("Невозможно обработать сразу несколько аттрибутов");

                if (href != null)
                {
                    if (!File.Exists(href.Value))
                        throw new IOException(string.Format("Файл '{0}' не существует", href.Value));

                    node.AddBeforeSelf(File.ReadAllText(href.Value));
                }

                if (text != null) node.AddBeforeSelf(text.Value);

                trash.Add(node);
            }
        }
    }
}