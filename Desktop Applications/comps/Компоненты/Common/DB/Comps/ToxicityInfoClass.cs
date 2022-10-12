using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PGS.Comps
{
    /// <summary>
    /// Интерфейс информация о токсичности
    /// </summary>
    public interface IToxicityInfoClass
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// ПДК 
        /// </summary>
        double PDK { get; }
        /// <summary>
        /// Ед. измерения ПДК
        /// </summary>
        Concentration.Units Units { get; }
    }

    /// <summary>
    /// Свойства компонента
    /// </summary>
    public partial class CompProperties : ICompProperties
    {
        /// <summary>
        /// Класс информация о токсичности компонента
        /// </summary>
        public class ToxicityInfoClass : IToxicityInfoClass
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
            /// Поле ПДК компонента
            /// </summary>
            private double _PDK = double.NaN;
            /// <summary>
            /// ПДК компонента
            /// </summary>
            public double PDK
            {
                get
                {
                    return _PDK;
                }
                set
                {
                    if (_PDK != value)
                    {
                        _PDK = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }
            /// <summary>
            /// Поле Еденицы измерения ПДК
            /// </summary>
            private PGS.Concentration.Units _Units;
            /// <summary>
            /// Еденицы измерения ПДК
            /// </summary>
            public PGS.Concentration.Units Units
            {
                get
                {
                    return _Units;
                }
                set
                {
                    if (_Units != value)
                    {
                        _Units = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compid"></param>
            private ToxicityInfoClass(int compid)
            {
                _CompID = compid;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compinfo"></param>
            public ToxicityInfoClass(CompInfo compinfo)
            {
                _CompInfo = compinfo;
            }
            /// <summary>
            /// Кэш загрузки из базы
            /// </summary>
            static private Dictionary<int, ToxicityInfoClass> _Cache = new Dictionary<int, ToxicityInfoClass>();
            /// <summary>
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="Forced">Не использовать кэш</param>
            /// <returns></returns>
            static public ToxicityInfoClass Load(int CompID, bool Forced = false)
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
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="con"></param>
            /// <returns></returns>
            static public ToxicityInfoClass Load(int CompID, OdbcConnection con)
            {
                OdbcCommand cm = new OdbcCommand();
                cm.Connection = con;
                cm.CommandText = string.Format("SELECT * FROM Comps.Toxicity WHERE CompID = {0}", CompID);
                OdbcDataReader dr = cm.ExecuteReader();

                ToxicityInfoClass r = null;
                _Cache.TryGetValue(CompID, out r);
                if (r == null)
                {
                    r = new ToxicityInfoClass(CompID);
                    _Cache[CompID] = r;
                }

                if (dr.Read())
                {
                    double pdk = (double)dr["pdk"];
                    PGS.Concentration.Units units = (PGS.Concentration.Units)dr["pdk_units"];



                    r.PDK = pdk;
                    r.Units = units;
                    r.InBase = true;
                    r._Saved = true;


                }

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
                    Q = @"DELETE FROM comps.toxicity WHERE compid = {0}";
                }
                else
                {
                    if (!InBase)
                    {
                        Q = @"INSERT INTO comps.toxicity (compid, pdk, pdk_units) VALUES ({0}, {1}, {2})";
                    }
                    else
                    {
                        Q = @"UPDATE comps.toxicity SET pdk = {1}, pdk_units = {2} WHERE compid = {0}";
                    }
                }
                OdbcCommand cm = new OdbcCommand(string.Format(Q,
                    CompID,
                    DB.ToDBString(PDK),
                    (int)Units),
                    con, tr);

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
            public static void Delete(int id, OdbcConnection con, OdbcTransaction tr)
            {
                OdbcCommand cm = new OdbcCommand("", con, tr);
                cm.CommandText = string.Format(@"DELETE FROM comps.toxicity WHERE compid = {0}", id);
                cm.ExecuteNonQuery();
            }

            /// <summary>
            /// Нет данных для сохранения
            /// </summary>
            public bool isEmpty
            {
                get
                {
                    return double.IsNaN(PDK);
                }
            }
        }
    }
}