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
    /// Наборы данных для загрузки
    /// </summary>
    [Flags()]
    public enum DataSets
    {
        /// <summary>
        /// Значение по умолчанию
        /// </summary>
        Default = 0,

        /// <summary>
        /// Все данные
        /// </summary>
        All = -1,

        /// <summary>
        /// Картинка
        /// </summary>
        Image = 1,

        /// <summary>
        /// Физ. свойства
        /// </summary>
        Properties = 2,

        /// <summary>
        /// Давление насыщеных паров
        /// </summary>
        Psaturation = 4,

        /// <summary>
        /// Данные о воспламеняемости
        /// </summary>
        Flammability = 8,

        /// <summary>
        /// Данные о токсичности
        /// </summary>
        Toxicity = 16,

        /// <summary>
        /// Данные о несовместимости компонентов
        /// </summary>
        Incompatibility = 32,

        /// <summary>
        /// Данные о коэф. расхода
        /// </summary>
        KFlow = 64,

        /// <summary>
        /// Данные по ГОСТ 31369-2008
        /// </summary>
        GOST_31369_2008 = 128
    }


    /// <summary>
    /// Информация о компоненте
    /// </summary>
    public interface ICompInfo
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Название компонента
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Вариант названия
        /// </summary>
        string NameAlt { get; }

        /// <summary>
        /// Название для печати (паспорт)
        /// </summary>
        string PrintName { get; }

        /// <summary>
        /// Название для показа в программе
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Формула
        /// </summary>
        string Formula { get; }

        /// <summary>
        /// Изомер
        /// </summary>
        bool Isomer { get; }

        /// <summary>
        /// Индексы 
        /// </summary>
        int[] Indexes { get; }

        /// <summary>
        /// Группа
        /// </summary>
        int? GroupID { get; }

        /// <summary>
        /// Код 1С согласно справочнику "Газы"
        /// </summary>
        int? Kod_1C { get; }

        /// <summary>
        /// Интерфейс свойств компонента
        /// </summary>
        ICompProperties Properties { get; }
    }


    /// <summary>
    /// Информация о компоненте
    /// </summary>
    public partial class CompInfo : ICompInfo
    {
        /// <summary>
        /// Событие компонент изменен
        /// </summary>
        public event Action Changed;

        /// <summary>
        /// Метод изменения компонента
        /// </summary>
        private void Change()
        {
            Changed?.Invoke();
        }

        /// <summary>
        /// Поле сохранено в базу
        /// </summary>
        private bool _Saved = true;

        /// <summary>
        /// Сохранено в базу
        /// </summary>
        public bool Saved => _Saved && Image.Saved && Properties.Saved;

        /// <summary>
        /// Поле ID компонента в базе
        /// </summary>
        private int? _ID = null;

        /// <summary>
        /// ID компонета в базе
        /// </summary>
        public int ID
        {
            get
            {
                if (_ID == null)
                    return -1;
                else return (int)_ID;
            }
        }

        /// <summary>
        /// Поле Название компонента
        /// </summary>
        private string _Name = "";

        /// <summary>
        /// Название компонента
        /// </summary>
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Альтернативные названия
        /// </summary>
        private string _NameAlt = "";

        /// <summary>
        /// Альтернативные названия
        /// </summary>
        public string NameAlt
        {
            get
            {
                return _NameAlt;
            }
            set
            {
                if (_NameAlt != value)
                {
                    _NameAlt = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Название для печати
        /// </summary>
        private string _PrintName = "";

        /// <summary>
        /// Название для печати
        /// </summary>
        public string PrintName
        {
            get
            {
                return _PrintName;
            }
            set
            {
                if (_PrintName != value)
                {
                    _PrintName = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Название для отображения пользователю
        /// </summary>
        private string _DisplayName = "";

        /// <summary>
        /// Название для отображения пользователю
        /// </summary>
        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                if (_DisplayName != value)
                {
                    _DisplayName = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Формула
        /// </summary>
        private string _Formula = "";

        /// <summary>
        /// Формула
        /// </summary>
        public string Formula
        {
            get
            {
                return _Formula;
            }
            set
            {
                if (_Formula != value)
                {
                    _Formula = value;
                    _Indexes = CalcIndexes(_Formula);

                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Флаг изомер
        /// </summary>
        private bool _Isomer = false;

        /// <summary>
        /// Флаг изомер
        /// </summary>
        public bool Isomer
        {
            get
            {
                return _Isomer;
            }
            set
            {
                if (_Isomer != value)
                {
                    _Isomer = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Индексы C, H, O
        /// </summary>
        private int[] _Indexes = new int[0];

        /// <summary>
        /// Индексы C, H, O
        /// </summary>
        public int[] Indexes => _Indexes;

        /// <summary>
        /// Вычисление индексов C, H, O
        /// </summary>
        /// <param name="formula"></param>
        /// <returns></returns>
        public static int[] CalcIndexes(string formula)
        {
            var indexes = new int[3];

            const string regPattern = @"[CHO](?![a-z])\d*";
            var regexp = new Regex(regPattern);

            foreach (Match match in regexp.Matches(formula))
                if (match.Value.Length == 1)
                {
                    switch (match.Value)
                    {
                        case "C":
                            indexes[0]++;
                            break;
                        case "H":
                            indexes[1]++;
                            break;
                        case "O":
                            indexes[2]++;
                            break;
                        default:
                            continue;
                    }
                }
                else
                {
                    var count = int.Parse(match.Value.Substring(1));

                    switch (match.Value[0])
                    {
                        case 'C':
                            indexes[0] += count;
                            break;
                        case 'H':
                            indexes[1] += count;
                            break;
                        case 'O':
                            indexes[2] += count;
                            break;
                        default:
                            continue;
                    }
                }

            return indexes;
        }

        /// <summary>
        /// Поле Группа компонента
        /// </summary>
        private int? _GroupID = null;

        /// <summary>
        /// Группа компонента
        /// </summary>
        public int? GroupID
        {
            get
            {
                return _GroupID;
            }
            set
            {
                if (_GroupID != value)
                {
                    _GroupID = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Код из 1С
        /// </summary>
        private int? _Kod_1C = null;

        /// <summary>
        /// Код из 1С
        /// </summary>
        public int? Kod_1C
        {
            get
            {
                return _Kod_1C;
            }
            set
            {
                if (_Kod_1C != value)
                {
                    _Kod_1C = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Картинка
        /// </summary>
        private CompImage _Image = null;

        /// <summary>
        /// Картинка
        /// </summary>
        public CompImage Image
        {
            get
            {
                if (_Image == null)
                {
                    _Image = CompImage.Load(ID);
                    _Image.Changed += Change;
                }

                return _Image;
            }
        }

        /// <summary>
        /// Статус наличия компоннета 
        /// </summary>
        public StatusInfo Status
        {
            get
            {
                if (_ID != null)
                    return StatusInfo.Load(ID);
                else return null;
            }
        }

        /// <summary>
        /// Поле Свойства компонента
        /// </summary>
        private CompProperties _Properties = null;

        /// <summary>
        /// Свойства компонента
        /// </summary>
        public CompProperties Properties
        {
            get
            {
                if (_Properties == null)
                {
                    _Properties = CompProperties.Load(ID);
                    _Properties.Changed += Change;
                }

                return _Properties;
            }
        }

        /// <summary>
        /// Реализация интерфейса ICompInfo
        /// </summary>
        ICompProperties ICompInfo.Properties => Properties;

        /// <summary>
        /// Конструктор
        /// </summary>
        static CompInfo()
        {
            FillCompCache();
        }

        /// <summary>
        /// Конструктор. Создает новый компонент и все связанные свойства
        /// </summary>
        public CompInfo()
        {
            _Image = new CompImage(this);
            _Image.Changed += Change;

            _Properties = new CompProperties(this);
            _Properties.Changed += Change;
        }

        /// <summary>
        /// Конструктор. Создает только компонент
        /// </summary>
        /// <param name="id"></param>
        private CompInfo(int id)
        {
            _ID = id;
        }


        /// <summary>
        /// Кэш компонентов
        /// </summary>
        private static Dictionary<int, CompInfo> _CompCache = new Dictionary<int, CompInfo>();

        /// <summary>
        /// Компоненты выбраные пользователем.
        /// Ключ - пользовательское название компонента
        /// </summary>
        public static Dictionary<string, CompInfo> UserCompsCache = new Dictionary<string, CompInfo>();

        /// <summary>
        /// Массив всех компонентов. Для списков
        /// </summary>
        public static ICompInfo[] Comps => _CompCache.Values.ToArray();

        /// <summary>
        /// Загрузка кэша компонентов
        /// </summary>
        public static void FillCompCache()
        {
            _CompCache.Clear();

            var con = DB.Connect();
            try
            {
                var cm = new OdbcCommand("", con);
                cm.CommandText = "SELECT * FROM Comps.Comps";
                var dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    var ci = new CompInfo(0);
                    ci.Load(dr);
                    _CompCache.Add(ci.ID, ci);
                }
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Загрузить информацию о компоненте.
        /// </summary>
        /// <param name="id">ID компонента</param>
        /// <param name="DataSet"></param>
        /// <returns></returns>
        public static CompInfo Load(int id, DataSets DataSet = DataSets.Default)
        {
            if (DataSet != DataSets.Default)
            {
                var con = DB.Connect();
                try
                {
                    var cm = con.CreateCommand();
                    cm.CommandText = "SELECT * FROM Comps.Comps WHERE id = " + id;
                    var dr = cm.ExecuteReader();

                    if (!dr.Read())
                        throw new Exception(
                            string.Format("Компонент с ID={0} в базе не найден.\r\nОбратитесь к разработчику", id));

                    CompInfo ci;
                    if (_CompCache.ContainsKey(id))
                        ci = _CompCache[id];
                    else
                        ci = new CompInfo(id);

                    ci.Load(dr);
                    if (DataSet.HasFlag(DataSets.Image))
                        CompImage.Load(id, con);
                    if (DataSet.HasFlag(DataSets.Properties))
                        CompProperties.Load(id, con, DataSet);

                    _CompCache[ci.ID] = ci;
                    return ci;
                }
                finally
                {
                    con.Disconnect();
                }
            }
            else
            {
                if (!_CompCache.ContainsKey(id))
                    throw new Exception(
                        string.Format("Компонент с ID={0} в базе не найден.\r\nОбратитесь к разработчику", id));
                else
                    return _CompCache[id];
            }
        }

        /// <summary>
        /// Заполнить поя из DataReader
        /// </summary>
        /// <param name="dr"></param>
        private void Load(OdbcDataReader dr)
        {
            _ID = (int)dr["ID"];
            _Name = (string)dr["Name"];
            _NameAlt = dr.String("NameAlt");
            _PrintName = (string)dr["PName"];
            _DisplayName = (string)dr["DName"];
            _Formula = (string)dr["Formula"];
            _Isomer = (bool)dr["Isomer"];
            _Indexes = DB.FromDBArrayInt(dr.String("Indexes"));
            _GroupID = dr["GroupID"] as int?;
            _Kod_1C = dr["Kod_1C"] as int?;
            _Saved = true;
            Changed?.Invoke();
        }

        /// <summary>
        /// Сохранить
        /// </summary>
        public void Save()
        {
            var con = DB.Connect();
            var tr = con.BeginTransaction();
            try
            {
                Save(con, tr);
                tr.Commit();
            }
            finally
            {
                if (tr != null)
                    tr.Rollback();
            }

            con.Disconnect();
        }

        /// <summary>
        /// Сохранить
        /// </summary>
        /// <param name="con"></param>
        /// <param name="tr"></param>
        public void Save(OdbcConnection con, OdbcTransaction tr)
        {
            if (Saved)
                return;

            if (!_Saved)
            {
                string Q;
                if (_ID != null)
                    Q = @"
UPDATE comps.comps
SET 
    name = {1}, 
    namealt = {2}, 
    isomer = {3}, 
    formula = {4},
    groupid = {5},
    pname = {6},
    dname = {7}, 
    indexes = {8}, 
    kod_1c = {9}
WHERE ID = {0}
RETURNING ID";
                else
                    Q = @"
INSERT INTO comps.comps 
(name,
    namealt, 
    isomer,
    formula,
    groupid,
    pname,
    dname,
    indexes,
    kod_1c
) 
VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9})
RETURNING ID";
                var cm = new OdbcCommand(string.Format(DB.CultureSQL, Q,
                    ID,
                    Name,
                    NameAlt,
                    Isomer,
                    Formula,
                    GroupID,
                    PrintName,
                    DisplayName,
                    Indexes,
                    Kod_1C), con, tr);

                _ID = (int)cm.ExecuteScalar();
                _Saved = true;
            }

            Image.Save(con, tr);
            Properties.Save(con, tr);

            Changed?.Invoke();
        }

        /// <summary>
        /// удалить
        /// </summary>
        /// <param name="id"></param>
        /// <param name="con"></param>
        /// <param name="tr"></param>
        public static void Delete(int id, OdbcConnection con, OdbcTransaction tr)
        {
            var cm = new OdbcCommand("", con, tr);
            cm.CommandText = string.Format(@"DELETE FROM comps.comps WHERE id = {0}", id);
            cm.ExecuteNonQuery();
        }


        /// <summary>
        /// Поле форма выбора компонента
        /// </summary>
        private static PGS.UI.Comps.FormCompSelect _FormCompSelect = null;

        /// <summary>
        /// Форма выбора компонента
        /// </summary>
        private static PGS.UI.Comps.FormCompSelect FormCompSelect
        {
            get
            {
                if (_FormCompSelect == null) _FormCompSelect = new PGS.UI.Comps.FormCompSelect();
                return _FormCompSelect;
            }
        }

        /// <summary>
        /// Действие в случае если компонент не найден
        /// </summary>
        public enum CompNotFoundAction
        {
            /// <summary>
            /// ничего не делать
            /// </summary>
            No = 0,

            /// <summary>
            /// Спросить пользователя
            /// </summary>
            RequestUser = 1,

            /// <summary>
            /// Показать окно выбора компонента
            /// </summary>
            UserSelect
        }

        /// <summary>
        /// Загрузка информации о компоненте по названию или формуле.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static CompInfo FindCompInfo(string name, CompNotFoundAction action = CompNotFoundAction.No)
        {
            if (UserCompsCache.ContainsKey(name))
            {
                return UserCompsCache[name];
            }
            else
            {
                var compformula = DB.ConvertToLatin(name);

                foreach (var ci in _CompCache.Values)
                    if (ci.Name == name ||
                        ci.PrintName == name ||
                        (!ci.Isomer && ci.Formula == compformula))
                        return ci;

                var msg = "";
                msg = string.Format("Компонент '{0}' в базе не найден.\r\nВыбрать компонент?", name);

                CompInfo comp;
                if (action == CompNotFoundAction.UserSelect ||
                    (action == CompNotFoundAction.RequestUser &&
                     MessageBox.Show(msg, "Поиск компонента", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                     DialogResult.Yes))
                    if (FormCompSelect.ShowDialog() == DialogResult.OK)
                    {
                        comp = Load((int)FormCompSelect.CompID);
                        UserCompsCache.Add(name, comp);
                        return comp;
                    }

                return null;
            }
        }
    }
}