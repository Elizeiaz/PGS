using System.Collections.Generic;
using System.Data.Odbc;

namespace PGS.Common
{
    /// <summary>
    /// Единицы измерения количества
    /// </summary>
    public static class Quantity
    {

        /// <summary>
        /// Единиц измерения количетства согласно базе common.quantity_units
        /// </summary>
        public enum Units
        {
            /// <summary>
            /// Грамм
            /// </summary>
            g = 0,  //	"г"	"грамм"
            /// <summary>
            /// Килограмм
            /// </summary>
            kg = 1, //	"кг"	"килограмм"
            /// <summary>
            /// милилитр
            /// </summary>
            ml = 2, //	"мл"	"милилитр"
            /// <summary>
            /// куб. метр
            /// </summary>
            m3 = 4, //	"м3"	"куб.метр"
            /// <summary>
            /// атмосфера техническая
            /// </summary>
            at = 5, //	"ат"	"атмосфера техническая"
        }
        /// <summary>
        /// Информация о единице измерения
        /// </summary>
        public class UnitInfo
        {
            /// <summary>
            /// ID в базе
            /// </summary>
            public int ID { get; }
            /// <summary>
            /// Краткое название
            /// </summary>
            public string ShortName { get; }
            /// <summary>
            /// Длинное название
            /// </summary>
            public string LongName { get; }
            /// <summary>
            /// Значение
            /// </summary>
            public Units Units { get; }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="ID"></param>
            /// <param name="ShortName"></param>
            /// <param name="LongName"></param>
            public UnitInfo(int ID, string ShortName, string LongName)
            {
                this.ID = ID;
                this.ShortName = ShortName;
                this.LongName = LongName;
                Units = (Units)ID;
            }

        }
        /// <summary>
        /// Поле список единиц
        /// </summary>
        private static List<UnitInfo> _UnitsList = null;
        /// <summary>
        /// Список единиц измерения
        /// </summary>
        public static List<UnitInfo> UnitsList
        {
            get
            {
                if (_UnitsList == null)
                {
                    OdbcConnection con = DB.Connect();
                    try
                    {
                        _UnitsList = new List<UnitInfo>();

                        OdbcCommand cm = new OdbcCommand("", con);
                        cm.CommandText = @"select * from common.quantity_units";
                        OdbcDataReader dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            _UnitsList.Add(new UnitInfo((int)dr["id"], (string)dr["short_name"], (string)dr["long_name"]));
                        }
                    }
                    finally
                    {
                        con.Disconnect();
                    }
                }
                return new List<UnitInfo>(_UnitsList);
            }
        }
    }

}