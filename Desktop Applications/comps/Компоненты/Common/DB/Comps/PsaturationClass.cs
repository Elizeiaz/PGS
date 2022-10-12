using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PGS.Comps
{
    /// <summary>
    /// Интерфейс данных давления насыщеных паров от температуры
    /// </summary>
    public interface IPsaturation
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// Давление насыщеных паров от температуры
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        double this[double T] { get; }
    }

    /// <summary>
    /// Свойства компонента
    /// </summary>
    public partial class CompProperties : ICompProperties
    {
        /// <summary>
        /// Класс информация о давлении насыщеных паров от температуры
        /// </summary>
        public class Psaturation : IPsaturation
        {
            /// <summary>
            /// Поле Сохранено
            /// </summary>
            private bool _Saved = true;
            /// <summary>
            /// Сохранено
            /// </summary>
            public bool Saved
            {
                get
                {
                    return _Saved;
                }
            }
            /// <summary>
            /// Есть в базе
            /// </summary>
            private bool InBase = false;
            /// <summary>
            /// Событие объект изменен
            /// </summary>
            public event Action Changed;

            /// <summary>
            /// Поле ID компонента в базе
            /// </summary>
            private int _CompID;
            /// <summary>
            /// Информация о компоненте
            /// </summary>
            private CompInfo _CompInfo = null;
            /// <summary>
            /// ID компонента в базе
            /// </summary>
            public int CompID
            {
                get
                {
                    if (_CompInfo != null)
                    {
                        return _CompInfo.ID;
                    }
                    else return _CompID;
                }
            }
            /// <summary>
            /// Температуры для которых задается давление
            /// </summary>
            private double[] Ts = new double[] { 243.15, 253.15, 263.15, 273.15, 283.15, 293.15 };
            /// <summary>
            /// Давления насыщеных паров
            /// </summary>
            private double[] _Ps = new double[6]
                {double.PositiveInfinity,
                double.PositiveInfinity,
                double.PositiveInfinity,
                double.PositiveInfinity,
                double.PositiveInfinity,
                double.PositiveInfinity};
            /// <summary>
            /// Давление насыщеных паров
            /// </summary>
            public Types.ArrayEx<double> Ps { get; }
            /// <summary>
            /// Обработчик события get
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            private double GetPs(int index)
            {
                return _Ps[index];
            }
            /// <summary>
            /// Обработчик события set
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            private void SetPs(int index, double value)
            {
                if (_Ps[index] != value)
                {
                    _Ps[index] = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
            /// <summary>
            /// Обработчик события count
            /// </summary>
            /// <returns></returns>
            private int CountPs()
            {
                return _Ps.Length;
            }



            /// <summary>
            /// Давление насыщеных паров от температуры. 
            /// Табличные значения от -30°C до 20°C с шагом в 10°C.
            /// </summary>
            /// <param name="T">Температура, К</param>
            /// <returns>Давление насыщеных паров, ат</returns>
            public double this[double T]
            {
                get
                {
                    for (int i = 0; i < Ts.Length; i++)
                    {
                        if (Math.Abs(T - Ts[i]) <= 2.5)
                            return Ps[i];

                        if (i + 1 < Ts.Length)
                        {
                            if (Math.Abs(T - (Ts[i] + Ts[i + 1]) / 2) <= 2.5)
                                return (Ps[i] + Ps[i + 1]) / 2;
                        }
                    }
                    return double.PositiveInfinity;
                }
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="CompID"></param>
            private Psaturation(int CompID)
            {
                Ps = new Types.ArrayEx<double>(GetPs, SetPs, CountPs);
                _CompID = CompID;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compinfo"></param>
            public Psaturation(CompInfo compinfo)
            {
                Ps = new Types.ArrayEx<double>(GetPs, SetPs, CountPs);
                _CompInfo = compinfo;
            }
            /// <summary>
            /// Кэш загрузки из базы
            /// </summary>
            static private List<Psaturation> _Cache = new List<Psaturation>();
            /// <summary>
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="Forced">Не использовать кэш</param>
            /// <returns></returns>
            static public Psaturation Load(int CompID, bool Forced = false)
            {
                var r = _Cache.Find((x) => { return x.CompID == CompID; });
                if (!Forced && (r != null))
                {
                    return r;
                }
                else
                {
                    var con = DB.Connect();
                    try
                    {
                        return Load(CompID, con);
                    }
                    finally
                    {
                        con.Disconnect();
                    }
                }

            }
            /// <summary>
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="con"></param>
            /// <returns></returns>
            static public Psaturation Load(int CompID, OdbcConnection con)
            {
                Psaturation r = _Cache.Find((x) => { return x.CompID == CompID; });
                if (r == null)
                {
                    r = new Psaturation(CompID);
                    _Cache.Add(r);
                }

                OdbcCommand cm = new OdbcCommand();
                cm.Connection = con;
                cm.CommandText = string.Format("SELECT * FROM Comps.Psat WHERE CompID = {0}", CompID);
                OdbcDataReader dr = cm.ExecuteReader();


                if (dr.Read())
                {
                    r.Ps[0] = dr["Pss243K"] is DBNull ? double.PositiveInfinity : (double)dr["Pss243K"];
                    r.Ps[1] = dr["Pss253K"] is DBNull ? double.PositiveInfinity : (double)dr["Pss253K"];
                    r.Ps[2] = dr["Pss263K"] is DBNull ? double.PositiveInfinity : (double)dr["Pss263K"];
                    r.Ps[3] = dr["Pss273K"] is DBNull ? double.PositiveInfinity : (double)dr["Pss273K"];
                    r.Ps[4] = dr["Pss283K"] is DBNull ? double.PositiveInfinity : (double)dr["Pss283K"];
                    r.Ps[5] = dr["Pss293K"] is DBNull ? double.PositiveInfinity : (double)dr["Pss293K"];
                    r.InBase = true;
                }
                else
                {
                    for (int i = 0; i < r.Ps.Count; i++)
                    {
                        r.Ps[i] = double.PositiveInfinity;
                    }

                }
                r._Saved = true;
                return r;
            }
            /// <summary>
            /// Сохранить данные
            /// </summary>
            /// <param name="con"></param>
            /// <param name="tr"></param>
            public void Save(OdbcConnection con, OdbcTransaction tr)
            {
                if (Saved)
                    return;

                string Q;
                if (isEmpty)
                {
                    Q = @"DELETE FROM comps.psat WHERE compid = {0}";
                }
                else
                {
                    if (!InBase)
                    {
                        Q = @"
INSERT INTO comps.psat
    (compid, pss243k, pss253k, pss263k, pss273k, pss283k, pss293k)
VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})";
                    }
                    else
                    {
                        Q = @"
UPDATE comps.psat
SET 
    pss243k = {1}, 
    pss253k = {2}, 
    pss263k = {3}, 
    pss273k = {4},
    pss283k = {5},
    pss293k = {6} 
WHERE compid = {0}";
                    }
                }
                OdbcCommand cm = new OdbcCommand(string.Format(DB.CultureSQL, Q,
                    CompID,
                    Ps[0] == Double.PositiveInfinity ? (double?)null : Ps[0],
                    Ps[1] == Double.PositiveInfinity ? (double?)null : Ps[1],
                    Ps[2] == Double.PositiveInfinity ? (double?)null : Ps[2],
                    Ps[3] == Double.PositiveInfinity ? (double?)null : Ps[3],
                    Ps[4] == Double.PositiveInfinity ? (double?)null : Ps[4],
                    Ps[5] == Double.PositiveInfinity ? (double?)null : Ps[5]
                    ), con, tr);
                cm.ExecuteNonQuery();
                InBase = !isEmpty;
                _Saved = true;
            }


            /// <summary>
            /// Удалить данные из базы
            /// </summary>
            /// <param name="id">ID компонента в базе</param>
            /// <param name="con"></param>
            /// <param name="tr"></param>
            /// <returns></returns>
            public static bool Delete(int id, OdbcConnection con, OdbcTransaction tr)
            {
                OdbcCommand cm = new OdbcCommand("", con, tr);
                cm.CommandText = string.Format(@"DELETE FROM comps.psat WHERE compid = {0}", id);
                return cm.ExecuteNonQuery() == 1;
            }

            /// <summary>
            /// Нет данных для сохранения
            /// </summary>
            public bool isEmpty
            {
                get
                {
                    return double.IsInfinity(Ps[0]) &&
                        double.IsInfinity(Ps[1]) &&
                        double.IsInfinity(Ps[2]) &&
                        double.IsInfinity(Ps[3]) &&
                        double.IsInfinity(Ps[4]) &&
                        double.IsInfinity(Ps[5]);

                }
            }


        }

    }

}
