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
    /// Информация о несовместимости компонентов
    /// </summary>
    public interface IIncompatibilityInfoClass
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// Список не совместимых компонентов
        /// </summary>
        CompInfo[] Comps { get; }
    }

    /// <summary>
    /// Свойства компонента
    /// </summary>
    public partial class CompProperties : ICompProperties
    {
        /// <summary>
        /// Класс информации о несовместимости компонента
        /// </summary>
        public class IncompatibilityInfoClass : IIncompatibilityInfoClass
        {
            /// <summary>
            /// Поле Сохранено в базе
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
            /// Поле спсок Не совместимые компоненты
            /// </summary>
            private Types.ListEx<CompInfo> _Comps = new Types.ListEx<CompInfo>();
            /// <summary>
            /// Список Не совместимые компоненты
            /// </summary>
            public Types.ListEx<CompInfo> Comps
            {
                get
                {
                    return _Comps;
                }
            }

            /// <summary>
            /// Список Не совместимые компоненты
            /// </summary>
            CompInfo[] IIncompatibilityInfoClass.Comps
            {
                get
                {
                    return _Comps.ToArray();
                }
            }


            /// <summary>
            /// Оператор преобразования к массиму CompInfo[]
            /// </summary>
            /// <param name="value"></param>
            public static implicit operator CompInfo[] (IncompatibilityInfoClass value) => value.Comps.ToArray();

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="CompID"></param>
            private IncompatibilityInfoClass(int CompID)
            {
                _CompID = CompID;
                _Comps.Added += CompListChanged;
                _Comps.Removed += CompListChanged;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="compinfo"></param>
            public IncompatibilityInfoClass(CompInfo compinfo)
            {
                _CompInfo = compinfo;
                _Comps.Added += CompListChanged;
                _Comps.Removed += CompListChanged;
            }
            /// <summary>
            /// Обработчик событий изменения списка
            /// </summary>
            /// <param name="obj"></param>
            private void CompListChanged(CompInfo obj)
            {
                _Saved = false;
                Changed?.Invoke();
            }
            /// <summary>
            /// Кэш загрузки из базы
            /// </summary>
            static private Dictionary<int, IncompatibilityInfoClass> _Cache = new Dictionary<int, IncompatibilityInfoClass>();
            /// <summary>
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="CompID">ID компонента в базе</param>
            /// <param name="Forced">Не использовать кэш</param>
            /// <returns></returns>
            static public IncompatibilityInfoClass Load(int CompID, bool Forced = false)
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
            static public IncompatibilityInfoClass Load(int CompID, OdbcConnection con)
            {
                OdbcCommand cm = new OdbcCommand();
                cm.Connection = con;
                cm.CommandText = string.Format(@"
SELECT comp_b as comp
from comps.incompatibility i
where comp_a = {0}
union
select comp_a as comp
from comps.incompatibility i
where comp_b = {0}", CompID);
                OdbcDataReader dr = cm.ExecuteReader();

                IncompatibilityInfoClass r;
                if (_Cache.ContainsKey(CompID))
                {
                    r = _Cache[CompID];
                }
                else
                {
                    r = new IncompatibilityInfoClass(CompID);
                    _Cache.Add(CompID, r);
                }

                while (dr.Read())
                {
                    int id = (int)dr["comp"];
                    r.Comps.Add(CompInfo.Load(id));
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

                OdbcCommand cm = new OdbcCommand("", con, tr);
                cm.CommandText = string.Format(@"
SELECT comp_b as comp
from comps.incompatibility i
where comp_a = {0}
union
select comp_a as comp
from comps.incompatibility i
where comp_b = {0}", CompID);

                OdbcDataReader dr = cm.ExecuteReader();

                List<int> inDB = new List<int>();
                while (dr.Read())
                {
                    int id = (int)dr["comp"];
                    inDB.Add(id);
                }
                dr.Close();

                foreach (var comp in Comps)
                {
                    if (!inDB.Contains(comp.ID))
                    {
                        string Q = @"INSERT INTO comps.incompatibility (comp_a, comp_b) VALUES ({0}, {1})";
                        cm.CommandText = string.Format(Q, CompID, comp.ID);
                        cm.ExecuteNonQuery();
                    }
                    else
                    {
                        inDB.Remove(comp.ID);
                    }
                }
                foreach (var id in inDB)
                {
                    string Q = @"DELETE FROM comps.incompatibility WHERE (comp_a = {0} AND comp_b = {1}) OR (comp_a = {1} AND comp_b = {0})";
                    cm.CommandText = string.Format(Q, CompID, id);
                    cm.ExecuteNonQuery();
                }
                _Saved = true;
            }
            /// <summary>
            /// удалить из базы
            /// </summary>
            /// <param name="compid"></param>
            /// <param name="con"></param>
            /// <param name="tr"></param>
            static public void Delete(int compid, OdbcConnection con, OdbcTransaction tr)
            {
                OdbcCommand cm = new OdbcCommand("", con, tr);
                cm.CommandText = string.Format(@"DELETE FROM comps.incompatibility WHERE (comp_a = {0}) OR (comp_a = {1})", compid);
                cm.ExecuteNonQuery();
            }

        }

    }
}