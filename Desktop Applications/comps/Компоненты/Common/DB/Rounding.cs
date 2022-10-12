using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PGS
{
    /// <summary>
    /// Класс для округления по правилам метрологии
    /// </summary>
    static public class Rounding
    {
        /// <summary>
        /// Количество знаков после запятой для погрешности (с учетом 0.30 => 0.3)
        /// Оставляет одну или две значащих цифры
        /// </summary>
        /// <param name="x">Погрешность</param>
        /// <returns>Последний значащий разряд</returns>
        static public int GetLSDUncertainty(double x)
        {
            //Проверка параметров
            if (x == 0)
            {
                return 0;
            };


            int MaxValue = 100;   //10^2
            int MinValue = MaxValue / 10;

            int counter = 0;
            double operand = Math.Abs(x);

            while (operand <= MinValue)
            {
                operand = operand * 10;
                counter = counter + 1;
            };

            while (operand > MaxValue)
            {
                operand = operand / 10;
                counter = counter - 1;
            };

            if (Math.Round(operand) >= 30)
            {
                counter = counter - 1;
            };
            return counter;

        }

        /// <summary>
        /// Количество знаков после запятой, для требуемого числа значащих разрядов
        /// </summary>
        /// <param name="x">Значение</param>
        /// <param name="NumD">Кол-во значащих разрядов</param>
        /// <returns>Знаков после запятой</returns>
        static public int GetLSD(double x, int NumD)
        {

            //Проверка параметров
            if (x == 0)
            { return 0; }


            //
            int MaxValue = (int)Math.Pow(10, NumD);
            int MinValue = MaxValue / 10;

            int counter = 0;
            double operand = Math.Abs(x);

            while (operand <= MinValue)
            {
                operand = operand * 10;
                counter = counter + 1;
            };

            while (operand > MaxValue)
            {
                operand = operand / 10;
                counter = counter - 1;
            }

            return counter;
        }

        /// <summary>
        /// Округлить неопределенность
        /// </summary>
        /// <param name="uncertainty"></param>
        /// <returns></returns>
        static public string UncertaintyToString(double uncertainty)
        {
            if (double.IsNaN(uncertainty))
                return "";

            int decimalPlaces = GetLSDUncertainty(uncertainty);
            string format = GetFormatString(decimalPlaces);
            return string.Format(format, uncertainty);
        }

        /// <summary>
        /// Округлить значение по погрешности
        /// </summary>
        /// <param name="value"></param>
        /// <param name="uncertainty"></param>
        /// <returns></returns>
        static public string ValueToString(double value, double uncertainty)
        {
            if (double.IsNaN(value))
            {
                return "";
            }
            else
            {
                if (double.IsNaN(uncertainty))
                    return value.ToString();

                int decimalPlaces = GetLSDUncertainty(uncertainty);
                string format = GetFormatString(decimalPlaces);
                return string.Format(format, value);
            }
        }

        /// <summary>
        /// Округление до заданного кол-ва значащих разрядов
        /// </summary>
        /// <param name="value"></param>
        /// <param name="signedDigits"></param>
        /// <returns></returns>
        static public string ValueToString(double value, int signedDigits)
        {
            if (Double.IsNaN(value))
            {
                return "";
            }
            else
            {
                if (value == 0)
                {
                    // return "0";
                    return string.Format(string.Format("{{0:f{0}}}", signedDigits), value);
                }
                else
                {
                    int m = (int)Math.Floor(Math.Log10(Math.Abs(value)));
                    if (m > signedDigits - 1)
                    {
                        return string.Format("{0}", (int)value);
                    }
                    else
                    {
                        return string.Format(string.Format("{{0:f{0}}}", signedDigits - m - 1), value);
                    }

                }
            }
        }

        /// <summary>
        /// Создает строку форматоирования вида {0:flsd}
        /// </summary>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        static private string GetFormatString(int decimalPlaces)
        {
            if (decimalPlaces >= 0)
            {
                return string.Format("{{0:f{0}}}", decimalPlaces);
            }
            else
            {
                return "{0:f0}";
            }
        }
    }
}
