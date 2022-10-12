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
    /// Интерфейс информация о коэф. расхода
    /// </summary>
    public interface IKFlowInfoClass
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// Коэф. расхода
        /// </summary>
        double K { get; }
    }

    /// <summary>
    /// Свойства компонента
    /// </summary>
    public partial class CompProperties : ICompProperties
    {
        /// <summary>
        /// Класс информация о KFlow
        /// </summary>
        public class KFlowInfoClass : IKFlowInfoClass
        {
            /// <summary>
            /// Событие объект изменен
            /// </summary>
            public event Action Changed;
            /// <summary>
            /// Есть в базе
            /// </summary>
            private bool _InBase = false;
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

            private double _K = double.NaN;
            /// <summary>
            /// KFlow компонента
            /// </summary>
            public double K
            {
                get
                {
                    return _K;
                }
                set
                {
                    if (_K != value)
                    {
                        _K = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }
            /// <summary>
            /// Значение коэф. К
            /// </summary>
            /// <param name="value"></param>
            public static implicit operator double(KFlowInfoClass value) => value.K;

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compid"></param>
            private KFlowInfoClass(int compid)
            {
                _CompID = compid;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compinfo"></param>
            public KFlowInfoClass(CompInfo compinfo)
            {
                _CompInfo = compinfo;
            }


            /// <summary>
            /// Кэш загрузки из базы
            /// </summary>
            static private Dictionary<int, KFlowInfoClass> _Cache = new Dictionary<int, KFlowInfoClass>();

            /// <summary>
            /// Загрузить из кэша или базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="Forced">Не использовать кэш</param>
            /// <returns></returns>
            static public KFlowInfoClass Load(int CompID, bool Forced = false)
            {
                if (_Cache.ContainsKey(CompID) && !Forced)
                {
                    return _Cache[CompID];
                }
                else
                {

                    OdbcConnection con = DB.Connect();
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
            /// Загрузка из базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="con"></param>
            /// <returns></returns>
            static public KFlowInfoClass Load(int CompID, OdbcConnection con)
            {
                OdbcCommand cm = new OdbcCommand();
                cm.Connection = con;
                cm.CommandText = string.Format("SELECT * FROM Comps.KFlow WHERE CompID = {0}", CompID);
                OdbcDataReader dr = cm.ExecuteReader();

                KFlowInfoClass r;
                _Cache.TryGetValue(CompID, out r);
                if (r == null)
                {
                    r = new KFlowInfoClass(CompID);
                }

                if (dr.Read())
                {
                    double k = (double)dr["k"];
                    r.K = k;
                    r._InBase = true;
                }
                else
                {
                    r.K = double.NaN;
                }
                r._Saved = true;
                _Cache[CompID] = r;
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
                if (!double.IsNaN(K))
                {
                    if (!_InBase)
                    {
                        Q = @"INSERT INTO comps.kflow (compid, k) VALUES({0}, {1})";
                    }
                    else
                    {
                        Q = @"UPDATE comps.kflow SET k = {1} WHERE compid = {0}";
                    }
                }
                else
                {
                    Q = @"DELETE FROM comps.kflow WHERE compid = {0}";
                }
                OdbcCommand cm = new OdbcCommand(string.Format(DB.CultureSQL, Q, CompID, K), con, tr);
                cm.ExecuteNonQuery();
                _InBase = !double.IsNaN(K);
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
                OdbcCommand cm = new OdbcCommand(string.Format(@"DELETE FROM comps.kflow WHERE compid = {0}", id), con, tr);
                cm.ExecuteNonQuery();
            }
        }
    }
}