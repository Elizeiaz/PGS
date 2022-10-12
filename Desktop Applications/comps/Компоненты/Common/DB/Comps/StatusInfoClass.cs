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
    public partial class CompInfo : ICompInfo
    {
        /// <summary>
        /// Информация о статусе наличия компонента
        /// </summary>
        public class StatusInfo
        {
            private int _CompID;
            /// <summary>
            /// ID компонента в базе
            /// </summary>
            public int CompID
            {
                get
                {
                    return _CompID;
                }
            }

            /// <summary>
            /// Дата обновления данных
            /// </summary>
            public DateTime? Updated { get; private set; } = DateTime.Now;
            /// <summary>
            /// Ест в сырье
            /// </summary>
            public bool InRaws { get; private set; } = false;
            /// <summary>
            /// Есть в эталонах
            /// </summary>
            public bool InReferences { get; private set; } = false;
            /// <summary>
            /// Минимальная концентрация в эталоне
            /// </summary>
            public double MinReferenceConc { get; private set; } = double.NaN;
            /// <summary>
            /// Максимальная концентрация в эталоне
            /// </summary>
            public double MaxReferenceConc { get; private set; } = double.NaN;

            private StatusInfo(int CompID)
            {
                _CompID = CompID;
            }

            /// <summary>
            /// Кэш загрузки из базы
            /// </summary>
            static private List<StatusInfo> _Cache = new List<StatusInfo>();
            /// <summary>
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="CompID"></param>
            /// <param name="Forced"></param>
            /// <returns></returns>
            static public StatusInfo Load(int CompID, bool Forced = false)
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
            static public StatusInfo Load(int CompID, OdbcConnection con)
            {
                StatusInfo r = _Cache.Find((x) => { return x.CompID == CompID; });

                OdbcCommand cm = new OdbcCommand();
                cm.Connection = con;
                cm.CommandText = string.Format("SELECT * FROM comps.comp_statuses WHERE compid = {0}", CompID);
                OdbcDataReader dr = cm.ExecuteReader();

                if (dr.Read())
                {
                    r = new StatusInfo(CompID);
                    r.InRaws = (bool)dr["in_raw"];
                    r.InReferences = (bool)dr["in_references"];
                    r.MinReferenceConc = dr.Double("min_references");
                    r.MaxReferenceConc = dr.Double("max_references");
                    OdbcCommand cm2 = new OdbcCommand("select obj_description('comps.comp_statuses'::regclass)", con);
                    string date = cm2.ExecuteScalar() as string;
                    if ((date != null) && (date != ""))
                    {
                        r.Updated = DateTime.Parse(date);
                    }
                    else r.Updated = null;
                    _Cache.Add(r);
                }
                return r;
            }
        }
    }
}