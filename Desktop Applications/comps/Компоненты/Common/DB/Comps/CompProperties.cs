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
    public interface ICompProperties
    {
        /// <summary>
        /// ID коммпонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// Плотность жидкой фазы, г/см3
        /// </summary>
        double MassDensity { get; }
        /// <summary>
        /// Молярная масса, г/моль
        /// </summary>
        double MolarMass { get; }
        /// <summary>
        /// Погрешность молярной массы, г/моль
        /// </summary>
        double dMolarMass { get; }
        /// <summary>
        /// Критическое давление, Па
        /// </summary>
        double Pkr { get; }
        /// <summary>
        /// Темпратура кипения, К
        /// </summary>
        double Tkip { get; }
        /// <summary>
        /// Критическая температура, К
        /// </summary>
        double Tkr { get; }
        /// <summary>
        /// Ацетрический фактор
        /// </summary>
        double w { get; }
        /// <summary>
        /// Коэф. сжимаемости для Н.У.
        /// </summary>
        double Z { get; }
        /// <summary>
        /// Расчет Z для Н.У.
        /// </summary>
        /// <returns></returns>
        double CalcZ();
        /// <summary>
        /// Расчет Z для заданных P и T
        /// </summary>
        /// <param name="P"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        double CalcZ(double P, double T);
        /// <summary>
        /// Информация о давления насыщеных паров
        /// </summary>
        IPsaturation Psat { get; }
        /// <summary>
        /// Воспламеняющийся компонент
        /// </summary>
        bool IsInflammable { get; }
        /// <summary>
        /// Информация о воспламеняемости
        /// </summary>
        IFlammabilityInfoClass FlammabilityInfo { get; }
        /// <summary>
        /// Токсичный компонент
        /// </summary>
        bool IsToxic { get; }
        /// <summary>
        /// Информация о токсичности
        /// </summary>
        IToxicityInfoClass ToxicityInfo { get; }
        /// <summary>
        /// Коэф. расхода
        /// </summary>
        IKFlowInfoClass KFlow { get; }
        /// <summary>
        /// Информация о несовместимости компонентов
        /// </summary>
        IIncompatibilityInfoClass incompatibility { get; }
    }

    /// <summary>
    /// Свойства компонента
    /// </summary>
    public partial class CompProperties : ICompProperties
    {
        /// <summary>
        /// Источник значения
        /// </summary>
        public enum ValueSource
        {
            /// <summary>
            /// Значение по умолчанию
            /// </summary>
            Default,
            /// <summary>
            /// Расчетное значение
            /// </summary>
            Calc,
            /// <summary>
            /// Заданно
            /// </summary>
            Set
        }

        /// <summary>
        /// Событие свойство изменено
        /// </summary>
        public event Action Changed;
        /// <summary>
        /// Метод обработки изменения свойства
        /// </summary>
        private void Change()
        {
            Changed?.Invoke();
        }
        /// <summary>
        /// Поле сохранено
        /// </summary>
        private bool _Saved = true;
        /// <summary>
        /// Сохранено
        /// </summary>
        public bool Saved
        {
            get
            {
                return _Saved &&
                    Psat.Saved &&
                    ToxicityInfo.Saved &&
                    FlammabilityInfo.Saved &&
                    Incompatibility.Saved &&
                    KFlow.Saved &&
                    GOST_31369.Saved;
            }
        }
        /// <summary>
        /// Поле есть в базе
        /// </summary>
        private bool InBase = false;
        /// <summary>
        /// Поле ID компонента в базе
        /// </summary>
        private int _CompID;
        /// <summary>
        /// Общая информация о компоненте
        /// </summary>
        private CompInfo _CompInfo = null;
        /// <summary>
        /// ID компонента в базе Comps.Comps
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
        /// Поле Молярная масса, г/моль
        /// </summary>
        private double _MolarMass = double.NaN;
        /// <summary>
        /// Молярная масса, г/моль
        /// </summary>
        public double MolarMass
        {
            get
            {
                return _MolarMass;
            }
            set
            {
                if (_MolarMass != value)
                {
                    _MolarMass = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Неопределенность молярной массы, г/моль
        /// </summary>
        private double _dMolarMass = double.NaN;
        /// <summary>
        /// Неопределенность молярной массы, г/моль
        /// </summary>
        public double dMolarMass
        {
            get
            {
                return _dMolarMass;
            }
            set
            {
                if (_dMolarMass != value)
                {
                    _dMolarMass = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Плотность жидкой фазы, г/см3
        /// </summary>
        private double _MassDensity = double.NaN;
        /// <summary>
        /// Плотность жидкой фазы, г/см3
        /// </summary>
        public double MassDensity
        {
            get
            {
                return _MassDensity;
            }
            set
            {
                if (_MassDensity != value)
                {
                    _MassDensity = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Температура кипения, К
        /// </summary>
        private double _Tkip = double.NaN;
        /// <summary>
        /// Температура кипения, К
        /// </summary>
        public double Tkip
        {
            get
            {
                return _Tkip;
            }
            set
            {
                if (_Tkip != value)
                {
                    _Tkip = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Критическая температура, К
        /// </summary>
        private double _Tkr = double.NaN;
        /// <summary>
        /// Критическая температура, К
        /// </summary>
        public double Tkr
        {
            get
            {
                return _Tkr;
            }
            set
            {
                if (_Tkr != value)
                {
                    _Tkr = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Критическое давление, Па
        /// </summary>
        private double _Pkr = double.NaN;
        /// <summary>
        /// Критическое давление, Па
        /// </summary>
        public double Pkr
        {
            get
            {
                return _Pkr;
            }
            set
            {
                if (_Pkr != value)
                {
                    _Pkr = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Ацетрический фактор
        /// </summary>
        private double _w = double.NaN;
        /// <summary>
        /// Ацетрический фактор
        /// </summary>
        public double w
        {
            get
            {
                return _w;
            }
            set
            {
                if (_w != value)
                {
                    _w = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Поле Коэф. сжимаемости для НУ
        /// </summary>
        private double _Z = 1;
        /// <summary>
        /// Коэф. сжимаемости для НУ
        /// </summary>
        public double Z
        {
            get
            {
                if (double.IsNaN(_Z))
                {
                    return 1;
                }
                else return _Z;
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
        /// Поле Источник значения Z
        /// </summary>
        private ValueSource _ZSource = ValueSource.Default;
        /// <summary>
        /// Источник значения Z
        /// </summary>
        public ValueSource ZSource
        {
            get
            {
                if (double.IsNaN(_Z))
                {
                    return ValueSource.Default;
                }
                else return _ZSource;
            }
            set
            {
                if (_ZSource != value)
                {
                    _ZSource = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }

        /// <summary>
        /// Расчет Z для НУ (P = 101325Па T = 293.15K)
        /// </summary>
        /// <returns></returns>
        public double CalcZ()
        {
            return CalcZ(101325, 293.15);
        }

        /// <summary>
        /// Расчет Z для заданного давления и температуры
        /// </summary>
        /// <param name="P">Давление, Па</param>
        /// <param name="T">Температура, К</param>
        /// <returns></returns>
        public double CalcZ(double P, double T)
        {
            double Tr = T / Tkr;

            double g0 = 0.1445 - 0.330 / Tr - 0.1385 / Tr / Tr - 0.0121 / Math.Pow(Tr, 3);
            double g1 = 0.073 + 0.46 / Tr - 0.50 / Tr / Tr - 0.097 / Math.Pow(Tr, 3) - 0.0073 / Math.Pow(Tr, 8);
            double g2 = 0.1042 - 0.2717 / Tr + 0.2388 / Tr / Tr - 0.0716 / Math.Pow(Tr, 3) + 1.502E-4 / Math.Pow(Tr, 8);

            double wr = Math.Pow(Tkip, 1.72) / MolarMass - 263;
            if (wr < 0)
                wr = 0;
            double B = (g0 + w * g1 + wr * g2) * Constants.GasConst.R * Tkr / Pkr;

            return 1 + (B * P) / (Constants.GasConst.R * T);
        }

        /// <summary>
        /// Пересчитать значение Z
        /// </summary>
        public void ReCalcZ()
        {
            Z = CalcZ();
            ZSource = ValueSource.Calc;
        }

        /// <summary>
        /// Поле Давление насыщеных паров от температуры. 
        /// Подробнее см. свойство
        /// </summary>
        private Psaturation _Psat = null;
        /// <summary>
        /// Давление насыщеных паров от температуры. 
        /// Табличные значения от -30°C до 20°C с шагом в 10°C.
        /// </summary>
        /// <returns>Давление насыщеных паров, ат</returns>
        public Psaturation Psat
        {
            get
            {
                if (_Psat == null)
                {
                    _Psat = Psaturation.Load(CompID);
                    _Psat.Changed += Change;
                }
                return _Psat;
            }
        }
        /// <summary>
        /// Реализация интерфейса ICompProperties
        /// </summary>
        IPsaturation ICompProperties.Psat
        {
            get
            {
                return Psat;
            }
        }

        /// <summary>
        /// Поле Токсичный компонент
        /// </summary>
        private bool _IsToxic = false;
        /// <summary>
        /// Токсичный компонент
        /// </summary>
        public bool IsToxic
        {
            get
            {
                return _IsToxic;
            }
            set
            {
                if (_IsToxic != value)
                {
                    _IsToxic = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }


        /// <summary>
        /// Поле Информация о токсичных свойствах. null если информация отсутствует
        /// </summary>
        private ToxicityInfoClass _ToxicityInfo = null;
        /// <summary>
        /// Информация о токсичных свойствах. null если информация отсутствует
        /// </summary>
        public ToxicityInfoClass ToxicityInfo
        {
            get
            {
                if (_ToxicityInfo == null)
                {
                    _ToxicityInfo = ToxicityInfoClass.Load(CompID);
                    _ToxicityInfo.Changed += Change;
                }
                return _ToxicityInfo;
            }
        }
        /// <summary>
        /// Реализация интерфейса ICompProperties
        /// </summary>
        IToxicityInfoClass ICompProperties.ToxicityInfo
        {
            get
            {
                return ToxicityInfo;
            }
        }

        /// <summary>
        /// Поле Горючий компонент
        /// </summary>
        private bool _IsInflammable = false;
        /// <summary>
        /// Горючий компонент
        /// </summary>
        public bool IsInflammable
        {
            get
            {
                return _IsInflammable;
            }
            set
            {
                if (_IsInflammable != value)
                {
                    _IsInflammable = value;
                    _Saved = false;
                    Changed?.Invoke();
                }
            }
        }
        /// <summary>
        /// Поле Информация о воспломеняемости. Если информации нет, то null
        /// </summary>
        private FlammabilityInfoClass _FlammabilityInfo = null;
        /// <summary>
        /// Информация о воспломеняемости. Если информации нет, то null
        /// </summary>
        public FlammabilityInfoClass FlammabilityInfo
        {
            get
            {
                if (_FlammabilityInfo == null)
                {
                    _FlammabilityInfo = FlammabilityInfoClass.Load(CompID);
                    _FlammabilityInfo.Changed += Change;
                }
                return _FlammabilityInfo;
            }
        }
        /// <summary>
        /// Реализация интерфейса ICompProperties
        /// </summary>
        IFlammabilityInfoClass ICompProperties.FlammabilityInfo
        {
            get
            {
                return FlammabilityInfo;
            }
        }



        /// <summary>
        /// Поле Информация о KFlow
        /// </summary>
        private KFlowInfoClass _KFlow = null;
        /// <summary>
        /// Информация о KFlow
        /// </summary>
        public KFlowInfoClass KFlow
        {
            get
            {
                if (_KFlow == null)
                {
                    _KFlow = KFlowInfoClass.Load(CompID);
                    _KFlow.Changed += Change;
                }
                return _KFlow;
            }
        }
        /// <summary>
        /// Реализация интерфейса ICompProperties
        /// </summary>
        IKFlowInfoClass ICompProperties.KFlow
        {
            get
            {
                return KFlow;
            }
        }

        /// <summary>
        /// Поле информация о несовместимости компонентов
        /// </summary>
        private IncompatibilityInfoClass _Incompatibility = null;
        /// <summary>
        /// Информация о несовместимости компонентов
        /// </summary>
        public IncompatibilityInfoClass Incompatibility
        {
            get
            {
                if (_Incompatibility == null)
                {
                    _Incompatibility = IncompatibilityInfoClass.Load(CompID);
                    _Incompatibility.Changed += Change;
                }
                return _Incompatibility;
            }
        }
        /// <summary>
        /// Реализация интерфейса ICompProperties
        /// </summary>
        IIncompatibilityInfoClass ICompProperties.incompatibility
        {
            get
            {
                return Incompatibility;
            }
        }

        /// <summary>
        /// Поле Данные по ГОСТ 31369
        /// </summary>
        private GOST_31369_Class _GOST_31369 = null;
        /// <summary>
        /// Данные по ГОСТ 31369
        /// </summary>
        public GOST_31369_Class GOST_31369
        {
            get
            {
                if (_GOST_31369 == null)
                {
                    _GOST_31369 = GOST_31369_Class.Load(CompID);
                    _GOST_31369.Changed += Change;
                }
                return _GOST_31369;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="CompID">ID компонента в базе Comps.Comps</param>
        private CompProperties(int CompID)
        {
            _CompID = CompID;
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="compinfo"></param>
        public CompProperties(CompInfo compinfo)
        {
            _CompInfo = compinfo;
            _Psat = new Psaturation(compinfo);
            _Psat.Changed += Change;

            _ToxicityInfo = new ToxicityInfoClass(compinfo);
            _ToxicityInfo.Changed += Change;

            _FlammabilityInfo = new FlammabilityInfoClass(compinfo);
            _FlammabilityInfo.Changed += Change;

            _Incompatibility = new IncompatibilityInfoClass(compinfo);
            _Incompatibility.Changed += Change;

            _KFlow = new KFlowInfoClass(compinfo);
            _KFlow.Changed += Change;

            _GOST_31369 = new GOST_31369_Class(compinfo);
            _GOST_31369.Changed += Change;
        }
        /// <summary>
        /// Кэш свойств загруженых из базы
        /// </summary>
        static private Dictionary<int, CompProperties> Cache = new Dictionary<int, CompProperties>();
        /// <summary>
        /// Загрузить свойства
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="DataSet"></param>
        /// <returns></returns>
        static public CompProperties Load(int ID, DataSets DataSet = DataSets.Default)
        {
            if (Cache.ContainsKey(ID) && (DataSet == DataSets.Default))
            {
                return Cache[ID];
            }
            else
            {
                OdbcConnection con = DB.Connect();
                try
                {
                    return Load(ID, con, DataSet);
                }
                finally
                {
                    con.Disconnect();
                }
            }
        }
        /// <summary>
        /// Загрузить свойства
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="con"></param>
        /// <param name="DataSet"></param>
        /// <returns></returns>
        static public CompProperties Load(int ID, OdbcConnection con, DataSets DataSet = DataSets.Default)
        {
            OdbcCommand cm = new OdbcCommand("Select * From Comps.Properties where compid = " + ID.ToString(), con);
            OdbcDataReader dr = cm.ExecuteReader();

            CompProperties r;
            if (Cache.ContainsKey(ID))
            {
                r = Cache[ID];
            }
            else
            {
                r = new CompProperties(ID);
                Cache.Add(ID, r);
            }

            if (dr.Read())
            {


                r._MolarMass = (double)dr["MolarMass"];
                r._dMolarMass = (double)dr["dMolarMass"];
                r._MassDensity = dr["MassDensity"] is DBNull ? double.NaN : (double)dr["MassDensity"];

                r._Tkip = dr.Double("Tkip");

                r._Tkr = dr.Double("Tkr");
                r._Pkr = dr.Double("Pkr");
                r._w = dr.Double("w");
                double Z = dr.Double("Z");
                r._IsToxic = (bool)dr["toxic"];
                r._IsInflammable = (bool)dr["inflammable"];

                if (double.IsNaN(Z))
                {
                    r.ReCalcZ();
                }
                else
                {
                    r.ZSource = ValueSource.Set;
                    r._Z = Z;
                }
                r.InBase = true;
                r._Saved = true;
            }

            if (DataSet.HasFlag(DataSets.Psaturation))
                Psaturation.Load(ID, true);
            if (DataSet.HasFlag(DataSets.Toxicity))
                ToxicityInfoClass.Load(ID, true);
            if (DataSet.HasFlag(DataSets.Flammability))
                FlammabilityInfoClass.Load(ID, true);
            if (DataSet.HasFlag(DataSets.Incompatibility))
                IncompatibilityInfoClass.Load(ID, true);
            if (DataSet.HasFlag(DataSets.KFlow))
                KFlowInfoClass.Load(ID, true);
#warning Всегда грузим - нет запроса к данным. остается не загруженым до самого сохранения.
            //if (DataSet.HasFlag(DataSets.GOST_31369_2008))
            GOST_31369_Class.Load(ID, true);

            return r;
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
                if (isEmpty)
                {
                    Q = @"DELETE FROM comps.properties WHERE compid = {0}";
                }
                else
                {
                    if (!InBase)
                    {
                        Q = @"INSERT INTO comps.properties
    (compid, molarmass, dmolarmass, massdensity, tkip, tkr, pkr, w, z, toxic, inflammable)
VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})";
                    }
                    else
                    {
                        Q = @"UPDATE comps.properties
SET molarmass = {1}, dmolarmass = {2}, massdensity = {3}, tkip = {4}, tkr = {5}, pkr = {6}, w = {7}, z = {8}, toxic = {9}, inflammable = {10} WHERE compid = {0}";
                    }
                }
                OdbcCommand cm = new OdbcCommand(string.Format(Q,
                      CompID,
                      DB.ToDBString(!double.IsNaN(MolarMass) ? MolarMass : (double?)null),
                    DB.ToDBString(!double.IsNaN(dMolarMass) ? dMolarMass : (double?)null),
                    DB.ToDBString(!double.IsNaN(MassDensity) ? MassDensity : (double?)null),
                    DB.ToDBString(!double.IsNaN(Tkip) ? Tkip : (double?)null),
                    DB.ToDBString(!double.IsNaN(Tkr) ? Tkr : (double?)null),
                    DB.ToDBString(!double.IsNaN(Pkr) ? Pkr : (double?)null),
                    DB.ToDBString(!double.IsNaN(w) ? w : (double?)null),
                    DB.ToDBString((ZSource == ValueSource.Set) ? Z : (double?)null),
                    IsToxic, IsInflammable), con, tr);
                cm.ExecuteNonQuery();
                InBase = !isEmpty;
            }

            Psat.Save(con, tr);
            ToxicityInfo.Save(con, tr);
            FlammabilityInfo.Save(con, tr);
            Incompatibility.Save(con, tr);
            KFlow.Save(con, tr);
            GOST_31369.Save(con, tr);

            _Saved = true;
        }

        /// <summary>
        /// Удалить запись о свойствах из базы
        /// </summary>
        /// <param name="compID"></param>
        /// <param name="con"></param>
        /// <param name="tr"></param>
        public static void Delete(int compID, OdbcConnection con, OdbcTransaction tr)
        {
            OdbcCommand cm = new OdbcCommand("", con, tr);
            cm.CommandText = string.Format(@"DELETE FROM comps.properties WHERE compid = {0}", compID);
            cm.ExecuteNonQuery();
        }
        /// <summary>
        /// Объект пустой
        /// </summary>
        public bool isEmpty
        {
            get
            {
                return double.IsNaN(MolarMass) &&
                    double.IsNaN(dMolarMass) &&
                    double.IsNaN(MassDensity) &&
                    double.IsNaN(Tkip) &&
                    double.IsNaN(Tkr) &&
                    double.IsNaN(Pkr) &&
                    double.IsNaN(w) &&
                    ((ZSource == ValueSource.Calc) || (ZSource == ValueSource.Default));
            }
        }


    }

}