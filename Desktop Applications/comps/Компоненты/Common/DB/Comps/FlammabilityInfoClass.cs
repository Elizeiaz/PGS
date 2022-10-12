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
    /// Интерфейс информация о воспламеняемости
    /// </summary>
    public interface IFlammabilityInfoClass
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// Максимальная концентрация в воздухе
        /// </summary>
        double MaxConc_Air { get; }
        /// <summary>
        /// НКПР, %
        /// </summary>
        double NKPR_Air { get; }
        /// <summary>
        /// ВКПР, %
        /// </summary>
        double VKPR_Air { get; }
    }

    /// <summary>
    /// Свойства компонента
    /// </summary>
    public partial class CompProperties : ICompProperties
    {
        /// <summary>
        /// Класс информация о горючести компонента
        /// </summary>
        public class FlammabilityInfoClass : IFlammabilityInfoClass
        {
            /// <summary>
            /// Поле сохранено в базу
            /// </summary>
            private bool _Saved = true;
            /// <summary>
            /// Сохранено в базу
            /// </summary>
            public bool Saved
            {
                get
                {
                    return _Saved;
                }
            }
            /// <summary>
            /// Поле есть в базе
            /// </summary>
            private bool InBase = false;
            /// <summary>
            /// Событие изменено
            /// </summary>
            public event Action Changed;


            /// <summary>
            /// Поле ID компонента в базе
            /// </summary>
            private int _CompID;
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
            /// Поле НКПР, %
            /// </summary>
            private double _NKPR_Air = double.NaN;
            /// <summary>
            /// НКПР, %
            /// </summary>
            public double NKPR_Air
            {
                get
                {
                    return _NKPR_Air;
                }
                set
                {
                    if (_NKPR_Air != value)
                    {
                        _NKPR_Air = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Поле ВКПР, %
            /// </summary>
            private double _VKPR_Air = double.NaN;
            /// <summary>
            /// ВКПР, %
            /// </summary>
            public double VKPR_Air
            {
                get
                {
                    return _VKPR_Air;
                }
                set
                {
                    if (_VKPR_Air != value)
                    {
                        _VKPR_Air = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Поле Максимальная концентрация в воздухе, %
            /// </summary>
            private double _MaxConc_Air = double.NaN;
            /// <summary>
            /// Максимальная концентрация в воздухе, %
            /// </summary>
            public double MaxConc_Air
            {
                get
                {
                    return _MaxConc_Air;
                }
                set
                {
                    if (_MaxConc_Air != value)
                    {
                        _MaxConc_Air = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compid"></param>
            private FlammabilityInfoClass(int compid)
            {
                _CompID = compid;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compinfo"></param>
            public FlammabilityInfoClass(CompInfo compinfo)
            {
                _CompInfo = compinfo;
            }

            /// <summary>
            /// К\ш загрузки из базы
            /// </summary>
            static private Dictionary<int, FlammabilityInfoClass> _Cache = new Dictionary<int, FlammabilityInfoClass>();
            /// <summary>
            /// Загрузить из базы
            /// </summary>
            /// <param name="CompID">ID компонента</param>
            /// <param name="Forced">Не использовать кэш</param>
            /// <returns></returns>
            static public FlammabilityInfoClass Load(int CompID, bool Forced = false)
            {
                if (_Cache.ContainsKey(CompID) && !Forced)
                {
                    return _Cache[CompID];
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
            /// Загрзить из базы
            /// </summary>
            /// <param name="CompID"></param>
            /// <param name="con"></param>
            /// <returns></returns>
            static public FlammabilityInfoClass Load(int CompID, OdbcConnection con)
            {
                OdbcCommand cm = new OdbcCommand();
                cm.Connection = con;
                cm.CommandText = string.Format("SELECT * FROM Comps.Flammability WHERE CompID = {0}", CompID);
                OdbcDataReader dr = cm.ExecuteReader();

                FlammabilityInfoClass r = null;
                _Cache.TryGetValue(CompID, out r);
                if (r == null)
                {
                    r = new FlammabilityInfoClass(CompID);
                    _Cache[CompID] = r;
                }

                if (dr.Read())
                {


                    r.NKPR_Air = (double)dr["NKPR_Air"];
                    r.VKPR_Air = (double)dr["VKPR_Air"];
                    r.MaxConc_Air = (double)dr["max_conc_air"];
                    r.InBase = true;
                    r._Saved = true;

                }
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
                    Q = @"DELETE FROM comps.flammability WHERE compid = {0}";
                }
                else
                {
                    if (!InBase)
                    {
                        Q = @"INSERT INTO comps.flammability
    (compid, nkpr_air, vkpr_air, max_conc_air)
VALUES ({0}, {1}, {2}, {3})";
                    }
                    else
                    {
                        Q = @"UPDATE comps.flammability
SET nkpr_air = {1}, vkpr_air = {2}, max_conc_air = {3} WHERE compid = {0}";
                    }
                }
                OdbcCommand cm = new OdbcCommand(string.Format(Q,
                     CompID,
                    DB.ToDBString(NKPR_Air),
                    DB.ToDBString(VKPR_Air),
                    DB.ToDBString(MaxConc_Air)), con, tr);
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
            public static void Delete(int id, OdbcConnection con, OdbcTransaction tr)
            {
                OdbcCommand cm = new OdbcCommand("", con, tr);
                cm.CommandText = string.Format(@"DELETE FROM comps.flammability WHERE compid = {0}", id);
                cm.ExecuteNonQuery();
            }
            /// <summary>
            /// Объект пустой
            /// </summary>
            public bool isEmpty
            {
                get
                {
                    return double.IsNaN(NKPR_Air) && double.IsNaN(VKPR_Air) && double.IsNaN(MaxConc_Air);
                }
            }


        }
    }
}
