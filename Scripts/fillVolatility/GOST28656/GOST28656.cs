using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOST28656;

internal class GOST28656
{
    /// <summary>
    /// Словарь хранит значения пдк для компонентов, где key - id компонента в таблице comps.comps, value - значения пдк компонента под разное давление
    /// </summary>
    private static Dictionary<int, Table> pdkDictionary;

    #region Class Table

    /// <summary>
    /// Класс таблицы, хранящий значения пдк компонентов для различный температур
    /// </summary>
    public class Table
    {
        // Key - температура (П: -35, -30, -20, 45), Value - массив из 5 значений пдк для давлений 0,1/0,5/1,0/1,5/2,0 соответственно
        private Dictionary<int, double[]> _table;

        public Table(Dictionary<int, double[]> table)
        {
            _table = table;
        }

        /// <returns>Массив из 5 элементов</returns>
        public double[] this[int temp]
        {
            get
            {
                if (!_table.ContainsKey(temp)) throw new ArgumentException($"Отсутствуют значения для температуры: {temp}");
                return _table[temp];
            }
        } 

        /// <returns>Значение пдк компонента</returns>
        public double this[int temp, int index] 
        {
            get
            {
                if (!_table.ContainsKey(temp)) throw new ArgumentException($"Отсутствуют значения для температуры: {temp}");
                if (index < 0 || index > 4) throw new ArgumentOutOfRangeException($"Выход за границы 0 <= index <= 4");
                return _table[temp][index];
            }
            
        } 

        /// <returns>Значение пдк компонента по значению давления 0,1/0,5/1,0/1,5/2,0</returns>
        public double this[int temp, double pressure]
        {
            get
            {
                if (!_table.ContainsKey(temp)) throw new ArgumentException($"Отсутствуют значения для температуры: {temp}");

                // Для старых версий
                switch (pressure)
                {
                    case 0.1:
                        return _table[temp][0];
                    case 0.5:
                        return _table[temp][1];
                    case 1:
                        return _table[temp][2];
                    case 1.5:
                        return _table[temp][3];
                    case 2.0:
                        return _table[temp][4];
                    default:
                        throw new ArgumentException($"Отсутствует значение пдк для давления: {pressure}");
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// Возвращает класс с данными Table. Если ключ отсутствует => null
    /// </summary>
    public Table this[int compId]
    {
        get
        {
            if (!pdkDictionary.ContainsKey(compId)) throw new ArgumentException($"Отсутствуют значения ПДК для компонента: {compId}");
            return pdkDictionary[compId];
        }
    } 

    #region Заполнение значий словаря pdkDictionary
    static GOST28656()
    {
        pdkDictionary = new Dictionary<int, Table>()
        {
            { 163, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 13.2, 14, 15, 15.5, 16.4 } } }) }, // CH4
            { 166, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 4, 4.2, 4.4, 4.7, 5 } } }) }, // C2H6
            { 165, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 5.6, 5.7, 6.2, 6.5, 7 } } }) }, // C2H4
            { 168, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 1.25, 1.37, 1.45, 1.53, 1.68 } } }) }, // C3H8

            { 167, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 1.5, 1.55, 1.65, 1.73, 1.92 } } }) }, // C3H6
            { 230, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 1.5, 1.55, 1.65, 1.73, 1.92 } } }) }, // C3H6

            { 174, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.55, 0.6, 0.66, 0.69, 0.76 } } }) }, // i-C4H10
            { 175, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.41, 0.45, 0.48, 0.51, 0.56 } } }) }, // n-C4H10
            { 229, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.36, 0.41, 0.45, 0.48, 0.54 } } }) }, // C4H8
            { 181, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.2, 0.21, 0.24, 0.26, 0.28 } } }) }, // i-C5H12
            { 182, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.13, 0.15, 0.17, 0.18, 0.2 } } }) }, // n-C5H12

            { 176, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.17, 0.19, 0.17, 0.18, 0.2 } } }) }, // C5H10 "1-пентен"
            { 177, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.17, 0.19, 0.17, 0.18, 0.2 } } }) }, // C5H10 "3-метил-1-бутен"
            { 178, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.17, 0.19, 0.17, 0.18, 0.2 } } }) }, // C5H10 "2-метил-1-бутен"
            { 220, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.17, 0.19, 0.17, 0.18, 0.2 } } }) }, // C5H10 "циклопентан"
            { 275, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.17, 0.19, 0.17, 0.18, 0.2 } } }) }, // C5H10 "2-метил-2-бутен"

            { 185, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.045, 0.053, 0.06, 0.063, 0.072 } } }) }, // n-C6H14
            { 164, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 6, 6.25, 6.9, 7.05, 7.38 } } }) }, // C2H2
            { 276, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.98, 1.1, 1.15, 1.23, 1.34 } } }) }, // C3H4 (Аллен)
            { 243, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.76, 0.85, 0.9, 0.93, 1.04 } } }) }, // C3H4 (Метилацетилен)

            { 169, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.43, 0.49, 0.54, 0.57, 0.62 } } }) }, // C4H6 "1,3-бутадиен"
            { 223, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.43, 0.49, 0.54, 0.57, 0.62 } } }) }, // C4H6 "1,2-бутадиен"
            { 228, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.43, 0.49, 0.54, 0.57, 0.62 } } }) }, // C4H6 "циклобутен"
            { 286, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.43, 0.49, 0.54, 0.57, 0.62 } } }) }, // C4H6 "этилацетилен"
            { 288, new Table(new Dictionary<int, double[]>() { { 45, new double[5] { 0.43, 0.49, 0.54, 0.57, 0.62 } } }) }, // C4H6 "диметилацетилен"
        };
    }

    #endregion

}