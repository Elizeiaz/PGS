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
    /// Свойства компонента
    /// </summary>
    public partial class CompProperties : ICompProperties
    {
        /// <summary>
        /// Параметры для ГОСТ 31369_2008.
        /// Газ природный.
        /// Вычисление тплоты сгорания, плотности, оносительной плотности 
        /// и числа воббе на основе компонентного состава.
        /// </summary>
        public class GOST_31369_Class
        {
            /// <summary>
            /// Поле сохранено в бзау
            /// </summary>
            private bool _Saved = true;
            /// <summary>
            /// Сохранено в базе
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
            /// Событие изменено
            /// </summary>
            public event Action Changed;

            /// <summary>
            /// ID компонента в базе
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
            /// Поле Фактор сжимаемости
            /// </summary>
            private double _Z = double.NaN;
            /// <summary>
            /// Фактор сжимаемости
            /// </summary>
            public double Z
            {
                get
                {
                    return _Z;
                }
                set
                {
                    if (_Z != value)
                    {
                        _Z = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Поле Погрешность фактора сжимаемости
            /// </summary>
            private double _dZ = double.NaN;
            /// <summary>
            /// Погрешность фактора сжимаемости
            /// </summary>
            public double dZ
            {
                get
                {
                    return _dZ;
                }
                set
                {
                    if (_dZ != value)
                    {
                        _dZ = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Поле Коэф. суммирования
            /// </summary>
            private double _b = double.NaN;
            /// <summary>
            /// Коэф. суммирования
            /// </summary>
            public double b
            {
                get
                {
                    return _b;
                }
                set
                {
                    if (_b != value)
                    {
                        _b = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Поле Теплота сгорания вышшая
            /// </summary>
            private double _Hmax = double.NaN;
            /// <summary>
            /// Теплота сгорания вышшая
            /// </summary>
            public double Hmax
            {
                get
                {
                    return _Hmax;
                }
                set
                {
                    if (_Hmax != value)
                    {
                        _Hmax = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }


            /// <summary>
            /// Поле Теплота сгорания низшая
            /// </summary>
            private double _Hmin = double.NaN;
            /// <summary>
            /// Теплота сгорания низшая
            /// </summary>
            public double Hmin
            {
                get
                {
                    return _Hmin;
                }
                set
                {
                    if (_Hmin != value)
                    {
                        _Hmin = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="CompID"></param>
            private GOST_31369_Class(int CompID)
            {
                _CompID = CompID;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compinfo"></param>
            public GOST_31369_Class(CompInfo compinfo)
            {
                _CompInfo = compinfo;
            }

            /// <summary>
            /// Кэш загрузки из базы
            /// </summary>
            static private List<GOST_31369_Class> _Cache = new List<GOST_31369_Class>();
            /// <summary>
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="Forced">Не использовать кэш</param>
            /// <returns></returns>
            static public GOST_31369_Class Load(int CompID, bool Forced = false)
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
            /// <param name="CompID"></param>
            /// <param name="con"></param>
            /// <returns></returns>
            static public GOST_31369_Class Load(int CompID, OdbcConnection con)
            {
                GOST_31369_Class r = _Cache.Find((x) => { return x.CompID == CompID; });
                if (r == null)
                {
                    r = new GOST_31369_Class(CompID);
                    _Cache.Add(r);
                }

                OdbcCommand cm = new OdbcCommand();
                cm.Connection = con;
                cm.CommandText = string.Format("SELECT * FROM comps.gost_31369_2008 WHERE compid = {0}", CompID);
                OdbcDataReader dr = cm.ExecuteReader();

                if (dr.Read())
                {
                    r.Z = (double)dr["z"];
                    r.dZ = (double)dr["dz"];
                    r.b = (double)dr["b"];
                    r.Hmax = (double)dr["hmax"];
                    r.Hmin = (double)dr["hmin"];
                    r.InBase = true;
                }

                r._Saved = true;
                return r;
            }
            /// <summary>
            /// Сохранить в базу
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
                    Q = @"DELETE FROM comps.gost_31369_2008 WHERE compid = {0}";
                }
                else
                {
                    if (!InBase)
                    {
                        Q = @"
INSERT INTO comps.gost_31369_2008
    (compid, z, dz, b, hmax, hmin)
VALUES ({0}, {1}, {2}, {3}, {4}, {5})";
                    }
                    else
                    {
                        Q = @"
UPDATE comps.gost_31369_2008
SET 
    z = {1}, 
    dz = {2}, 
    b = {3}, 
    hmax = {4},
    hmin = {5}    
WHERE compid = {0}";
                    }
                }
                OdbcCommand cm = new OdbcCommand(string.Format(DB.CultureSQL, Q,
                    CompID,
                    Z,
                    dZ,
                    b,
                    Hmax,
                    Hmin), con, tr);
                cm.ExecuteNonQuery();
                InBase = !isEmpty;
                _Saved = true;
            }


            /// <summary>
            /// Удалить из базы
            /// </summary>
            /// <param name="id"></param>
            /// <param name="con"></param>
            /// <param name="tr"></param>
            /// <returns></returns>
            public static bool Delete(int id, OdbcConnection con, OdbcTransaction tr)
            {
                OdbcCommand cm = new OdbcCommand("", con, tr);
                cm.CommandText = string.Format(@"DELETE FROM comps.gost_31369_2008 WHERE compid = {0}", id);
                return cm.ExecuteNonQuery() == 1;
            }

            /// <summary>
            /// Объект пустой
            /// </summary>
            public bool isEmpty
            {
                get
                {
                    return double.IsNaN(Z) &&
                        double.IsNaN(dZ) &&
                        double.IsNaN(b) &&
                        double.IsNaN(Hmax) &&
                        double.IsNaN(Hmin);
                }
            }
        }
    }
}