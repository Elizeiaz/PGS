using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;

namespace PGS.Mixes
{
    /// <summary>
    /// Префик смеси согласно БД customers.mix_prefixes
    /// </summary>
    public enum MixPrefixes
    {
        /// <summary>
        /// Цех
        /// </summary>
        П = 0,
        /// <summary>
        /// ИПГ
        /// </summary>
        И = 1,
        /// <summary>
        /// ГС
        /// </summary>
        ГС = 2,
        /// <summary>
        /// Транс
        /// </summary>
        Т = 3,
        /// <summary>
        /// Зоопарк
        /// </summary>
        Ф = 4,
        /// <summary>
        /// Меркаптаны
        /// </summary>
        М = 5
    }

    /// <summary>
    /// Класс префикс смеси
    /// </summary>
    public class MixPrefix
    {
        /// <summary>
        /// Поле ID типа баллона в базе
        /// </summary>
        private int? _ID = null;
        /// <summary>
        /// ID типа баллона в базе
        /// </summary>
        public int ID
        {
            get
            {
                return (int)_ID;
            }
        }
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Префикс
        /// </summary>
        public string Prefix { get; private set; }


        /// <summary>
        /// Кэш
        /// </summary>
        static private Dictionary<int, MixPrefix> _cache = new Dictionary<int, MixPrefix>();
        /// <summary>
        /// Загрузить кэш
        /// </summary>
        static private void LoadCache()
        {
            _cache.Clear();
            OdbcConnection con = DB.Connect();
            try
            {
                OdbcCommand cm = new OdbcCommand("SELECT * FROM customers.mix_prefixes", con);
                OdbcDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    MixPrefix prefix = new MixPrefix();
                    prefix._ID = (int)dr["id"];
                    prefix.Name = (string)dr["name"];
                    prefix.Prefix = (string)dr["prefix"];
                    _cache.Add(prefix.ID, prefix);
                }
            }
            finally
            {
                con.Disconnect();
            }

        }
        /// <summary>
        /// Констркуктор
        /// </summary>
        static MixPrefix()
        {
            LoadCache();
        }
        /// <summary>
        /// Констркуктор
        /// </summary>
        internal MixPrefix()
        {
        }
        /// <summary>
        /// Список префиксов
        /// </summary>
        static public MixPrefix[] List
        {
            get
            {
                return _cache.Values.ToArray();
            }
        }

        /// <summary>
        /// Префикс по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public MixPrefix ByID(int? id)
        {
            if (id == null)
                return null;

            if (_cache.ContainsKey((int)id))
            {
                return _cache[(int)id];
            }
            else throw new Exception(string.Format("Префикс с id = {0} не найден!", id));
        }
        /// <summary>
        /// Превикс по строке 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static public MixPrefix Parse(string value)
        {
            foreach (MixPrefix prefix in _cache.Values)
            {
                if (prefix.Prefix == value)
                    return prefix;
            }
            return null;
        }
        /// <summary>
        /// Приведение к строке
        /// </summary>
        /// <returns></returns>
        new public string ToString()
        {
            return Prefix;
        }

    }
}
