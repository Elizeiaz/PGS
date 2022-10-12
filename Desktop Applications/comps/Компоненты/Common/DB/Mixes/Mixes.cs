using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PGS.Comps;

namespace PGS.Mixes
{
    /// <summary>
    /// Типы смеси. Согласно Lab.MixTypes
    /// Определяет таблицу где находится смесь.
    /// </summary>
    public enum MixTypes
    {
        /// <summary>
        /// Смесь
        /// </summary>
        Mix = 4,

        /// <summary>
        /// Эталон
        /// </summary>
        Reference = 1,
        /// <summary>
        /// Опорная точка, репер
        /// </summary>
        CalibrationRefPoint = 2,
        /// <summary>
        /// Смесь ГГС
        /// </summary>
        GenGasMixes = 3,

        /// <summary>
        /// Сырье
        /// </summary>
        SourceMix = 5
    }

    /// <summary>
    /// Идентификация смеси в базе
    /// </summary>
    public class MixBaseID
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="ID">Идентификатор смеси</param>
        /// <param name="MixType">Тип смеси</param>
        public MixBaseID(int? ID, MixTypes MixType)
        {
            this.ID = ID;
            this.MixType = MixType;
        }
        /// <summary>
        /// ID смеси в базе
        /// </summary>
        public int? ID;
        /// <summary>
        /// Тип смеси. Согласно Lab.MixTypes
        /// </summary>
        public MixTypes MixType;
    }


    /// <summary>
    /// Базовый интерфейс смеси
    /// </summary>
    public interface IMix
    {
        /// <summary>
        /// Типы смеси. Согласно Lab.MixTypes
        /// </summary>
        MixTypes MixType { get; }
        /// <summary>
        /// ID смеси в базе
        /// </summary>
        int? MixID { get; }
        /// <summary>
        /// Номер смеси
        /// </summary>
        string MixNumber { get; }
        /// <summary>
        /// Единицы измерения смеси
        /// </summary>
        Concentration.Units Units { get; }
        /// <summary>
        /// Компоненты смеси
        /// </summary>
        IMixComp[] Comps { get; }
        /// <summary>
        /// Метод расчет молярной массы смеси
        /// </summary>
        /// <returns></returns>
        double MolarMass();
    }

    /// <summary>
    /// Интерфейс компонента смеси
    /// </summary>
    public interface IMixComp
    {
        /// <summary>
        /// Интерфейс смеси
        /// </summary>
        IMix InMix { get; }
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// Параметры компонента
        /// </summary>
        PGS.Comps.CompProperties Properties { get; }
        /// <summary>
        /// Целевой компонент
        /// </summary>
        bool Target { get; }
        /// <summary>
        /// Разбавитель
        /// </summary>
        bool Diluent { get; }
        /// <summary>
        /// Концентрация
        /// </summary>
        double Conc { get; }
        /// <summary>
        /// Неопределенность концентрации, абсолютная
        /// </summary>
        double ConcAbsError { get; }
        /// <summary>
        /// Неопределенность концентрации, относительная %
        /// </summary>
        double ConcRelError { get; }
    }

    /// <summary>
    /// Интерфейс компонента смеси заказа
    /// </summary>
    public interface ICustomerMixComp : IMixComp
    {
        /// <summary>
        /// Единиц концентрации заказа
        /// </summary>
        Concentration.Units OrderUnits { get; }
        /// <summary>
        /// Заказаная концентрация, в единицах заказа
        /// </summary>
        double OrderConc { get; }
        /// <summary>
        /// Требуемая концентрация в мол.%
        /// </summary>
        double RequiredConc { get; }
    }
    /// <summary>
    /// Интерфейс компонента смеси сырья
    /// </summary>
    public interface IRawMixComp : IMixComp
    {
        /// <summary>
        /// Единиц концентрации исходные
        /// </summary>
        Concentration.Units OriginalUnits { get; }
        /// <summary>
        /// Заказаная концентрация, в единицах заказа
        /// </summary>
        double OriginalConc { get; }
    }


    /// <summary>
    /// Информация о смеси
    /// </summary>
    public class Mix : IMix
    {
        /// <summary>
        /// Тип смеси
        /// </summary>
        public MixTypes MixType { get; set; }
        /// <summary>
        /// ID смеси в базе
        /// </summary>
        public int? MixID { get; set; }
        /// <summary>
        /// Номер смеси
        /// </summary>
        public string MixNumber { get; set; }
        /// <summary>
        /// Поле единицы измерения смеси
        /// </summary>
        private Concentration.Units _Units = Concentration.Units.Molar;
        /// <summary>
        /// Единицы измерения смеси
        /// </summary>
        public Concentration.Units Units
        {
            set
            {
                _Units = value;
            }
            get
            {
                return _Units;
            }
        }
        /// <summary>
        /// Расчет молярной массы смеси
        /// </summary>
        /// <param name="mix"></param>
        /// <returns></returns>
        static public double MolarMass(IMix mix)
        {
            if (mix.Units != Concentration.Units.Molar)
                mix = Concentration.Convert(Concentration.Units.Molar, mix);

            double M = 0;
            for (int i = 0; i < mix.Comps.Length; i++)
            {
                M += mix.Comps[i].Conc * mix.Comps[i].Properties.MolarMass;
            }
            return M;
        }
        /// <summary>
        /// Расчет молярной массы смеси
        /// </summary>
        /// <returns></returns>
        public double MolarMass()
        {
            return MolarMass(this);
        }

        /// <summary>
        /// Компонент смеси
        /// </summary>
        public class MixComp : IMixComp
        {
            /// <summary>
            /// Интерфейс смеси содержащей этот компонент
            /// </summary>
            public IMix InMix { get; set; }
            /// <summary>
            /// ID компонента в базе
            /// </summary>
            public int CompID { get; set; }
            /// <summary>
            /// Целевой компонент
            /// </summary>
            public bool Target { get; set; } = true;
            /// <summary>
            /// Разбавитель
            /// </summary>
            public bool Diluent { get; set; } = false;
            /// <summary>
            /// Поле концентрация компонента
            /// </summary>
            private double _Conc = double.NaN;
            /// <summary>
            /// Конентрация комопнента
            /// </summary>
            public double Conc
            {
                get
                {
                    if (Diluent && (InMix != null))
                    {
                        double sumC = 0;
                        foreach (MixComp mc in InMix.Comps)
                        {
                            if (!mc.Diluent)
                                sumC += mc.Conc;
                        }
                        return 100 - sumC;
                    }
                    else
                    {
                        return _Conc;
                    }
                }
                set
                {
                    _Conc = value;
                }
            }

            /// <summary>
            /// Абсолютная погрешность компонента
            /// </summary>
            public double ConcAbsError { get; set; }

            /// <summary>
            /// Относительная погрешность компонента
            /// </summary>
            public double ConcRelError { get; set; }

            /// <summary>
            /// Поле физические свойства компонента
            /// </summary>
            private CompProperties _Properties = null;
            /// <summary>
            /// Физические свойства компонента
            /// </summary>
            public CompProperties Properties
            {
                get
                {
                    if (_Properties == null)
                    {
                        _Properties = CompProperties.Load(CompID);
                    }
                    return _Properties;
                }
                set
                {
                    _Properties = value;
                }
            }

            /// <summary>
            /// Конструктор
            /// </summary>
            public MixComp() { }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="InMix"></param>
            /// <param name="Properties"></param>
            public MixComp(IMix InMix, CompProperties Properties)
            {
                this.InMix = InMix;
                CompID = Properties.CompID;
                this.Properties = Properties;
                Conc = double.PositiveInfinity;
                Diluent = true;
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="Properties"></param>
            /// <param name="Conc"></param>
            public MixComp(CompProperties Properties, double Conc) : this(null, Properties, Conc) { }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="InMix"></param>
            /// <param name="Properties"></param>
            /// <param name="Conc"></param>
            public MixComp(IMix InMix, CompProperties Properties, double Conc)
            {
                this.InMix = InMix;
                CompID = Properties.CompID;
                this.Properties = Properties;
                this.Conc = Conc;
            }
        }

        /// <summary>
        /// Список компонентов смеси
        /// </summary>
        public List<IMixComp> Comps = new List<IMixComp>();

        /// <summary>
        /// Реализация интерфейса IMix
        /// </summary>
        IMixComp[] IMix.Comps
        {
            get
            {
                return Comps.Cast<IMixComp>().ToArray();
            }
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        public Mix() { }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="Units"></param>
        /// <param name="Comps"></param>
        public Mix(Concentration.Units Units, IMixComp[] Comps)
        {
            _Units = Units;
            this.Comps.AddRange(Comps);
        }

        /// <summary>
        /// Загрузить смесь из базы по MixBaseID
        /// </summary>
        /// <param name="MixBaseID"></param>
        /// <returns></returns>
        static public Mix LoadMix(MixBaseID MixBaseID)
        {
            if (MixBaseID.ID == null)
            {
                return null;
            }
            else
            {
                return LoadMix(MixBaseID.MixType, (int)MixBaseID.ID);
            }
        }
        /// <summary>
        /// Загрузить смесь из базы по типу и ID
        /// </summary>
        /// <param name="MixType"></param>
        /// <param name="MixID"></param>
        /// <returns></returns>
        static public Mix LoadMix(MixTypes MixType, int MixID)
        {
            switch (MixType)
            {
                case MixTypes.Mix:
                    return LoadCustomerMix(MixID);
                case MixTypes.Reference:
                    return LoadReferenceMix(MixID);
                default:
                    return null;
            }
        }
        /// <summary>
        /// Загрузить смесь из базы по номеру смеси и типу (опция)
        /// </summary>
        /// <param name="MixNumber"></param>
        /// <param name="MixType"></param>
        /// <returns></returns>
        static public Mix FindMix(string MixNumber, MixTypes? MixType = null)
        {
            if (MixType == null)
            {//Не знаем что ищем
#warning Фиксированный поиск по префиксу!!!
                if (MixNumber.StartsWith("И") ||
                    MixNumber.StartsWith("ГС") ||
                    MixNumber.StartsWith("Т") ||
                    MixNumber.StartsWith("П") ||
                    MixNumber.StartsWith("Ф") ||
                    MixNumber.StartsWith("М"))
                {
                    return LoadCustomerMix(MixNumber);
                }
                else
                {
                    return LoadReferenceMix(MixNumber);
                }
            }
            else
            {
                switch ((MixTypes)MixType)
                {
                    case MixTypes.Mix:
                        return LoadCustomerMix(MixNumber);
                    case MixTypes.Reference:
                        return LoadReferenceMix(MixNumber);
                    default:
                        return null;
                }
            }

        }


        /*  Загрузка из старой базц MS. Удалить в 2023
         *  
         *   static private string CS = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\WMBD.mdb; Jet OLEDB:Database Password = 6629948742";
         *     static private OleDbConnection con_old = new OleDbConnection(CS);

             [Obsolete("Старая база", true)]
             static public Mix LoadReferenceMix_Old(string MixNumber)
             {
                 //TODO: Опции загрузки всех компонентов Target

                 if (MixNumber.StartsWith("И") ||
                     MixNumber.StartsWith("ГС") ||
                     MixNumber.StartsWith("Т") ||
                     MixNumber.StartsWith("П") ||
                     MixNumber.StartsWith("Ф") ||
                     MixNumber.StartsWith("М") ||
                     MixNumber.StartsWith("репер"))
                     return null;

                 try
                 {
                     if (con_old.State != ConnectionState.Open)
                     {
                         con_old.Close();
                         CS = CS.Replace("|DataDirectory|", DB.OldDBDir);
                         con_old.ConnectionString = CS;
                         con_old.Open();
                     }

                     IDbCommand cm = con_old.CreateCommand();
                     cm.CommandText = string.Format(@"
     SELECT ID
     FROM PrimMixGas
     WHERE NumMix like '{0}'", MixNumber);
                     int? id = (int?)cm.ExecuteScalar();

                     if (id != null)
                     {
                         Mix mix = LoadReferenceMix((int)id);
                         mix.MixNumber = MixNumber;
                         return mix;
                     }
                     else
                     {
                         MessageBox.Show("Эталон '" + MixNumber + "' не найден!", "Поиск эталона", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         return null;
                     }
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
                 finally
                 {
                     con_old.Close();
                 }
                 return null;
             }

                         [Obsolete("Старая база", true)]
             static public Mix LoadReferenceMix_Old(int id)
             {
                 ConnectionState state = con_old.State;
                 try
                 {
                     if (con_old.State != ConnectionState.Open)
                     {
                         con_old.Close();
                         CS = CS.Replace("|DataDirectory|", DB.OldDBDir);
                         con_old.ConnectionString = CS;
                         con_old.Open();
                     }
                     OleDbCommand cm = new OleDbCommand("", con_old);
                     cm.CommandText = string.Format(@"
     SELECT 
         IDGas, 
         Conc,
         dCabs,
         dCrel
     FROM 
         GasAdm
     WHERE 
         (IDCyl = {0}) and Target", (int)id);
                     IDataReader dr = cm.ExecuteReader();
                     Mix mix = new Mix();
                     mix.MixType = MixTypes.Reference;
                     mix.MixID = (int)id;

                     while (dr.Read())
                     {
                         Mix.MixComp mc = new Mix.MixComp();
                         mc.InMix = mix;
                         mc.CompID = (int)dr["IDGas"];
                         mc.Target = true;
                         mc.Conc = (double)dr["Conc"];
                         mc.ConcAbsError = (double)dr["dCabs"];
                         mc.ConcRelError = (double)dr["dCrel"];
                         mix.Comps.Add(mc);
                     }
                     return mix;
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
                 finally
                 {
                     if (state != ConnectionState.Open)
                     {
                         con_old.Close();
                     }
                 }
                 return null;
             }
             */
        /// <summary>
        /// Загрузить эталон по номеру смеси
        /// </summary>
        /// <param name="MixNumber"></param>
        /// <returns></returns>
        static public Mix LoadReferenceMix(string MixNumber)
        {
            //TODO: Опции загрузки всех компонентов Target

#warning Фиксированные префиксы смесей!!!
            if (MixNumber.StartsWith("И") ||
                MixNumber.StartsWith("ГС") ||
                MixNumber.StartsWith("Т") ||
                MixNumber.StartsWith("П") ||
                MixNumber.StartsWith("Ф") ||
                MixNumber.StartsWith("М") ||
                MixNumber.StartsWith("репер"))
                return null;

            try
            {
                OdbcConnection con = DB.Connect();
                OdbcCommand cm = new OdbcCommand("", con);
                cm.CommandText = string.Format(DB.CultureSQL, @"
SELECT id
FROM lab_references.mixes
WHERE mixnumber like {0}", MixNumber);
                int? id = (int?)cm.ExecuteScalar();

                if (id != null)
                {
                    Mix mix = LoadReferenceMix((int)id);
                    mix.MixNumber = MixNumber;
                    return mix;
                }
                else
                {
                    MessageBox.Show("Эталон '" + MixNumber + "' не найден!", "Поиск эталона", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

            }
            return null;
        }


        /// <summary>
        /// Загрузить эталон по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public Mix LoadReferenceMix(int id)
        {

            OdbcConnection con = DB.Connect();
            try
            {
                OdbcCommand cm = new OdbcCommand("", con);
                cm.CommandText = string.Format(@"
select compid, target, (compid = basecompid) as diluent, c, dcabs, dcrel, mixnumber
from lab_references.mix_comps
left join lab_references.mixes on mixes.id = mix_comps.mixid
where (target or compid = basecompid)   and mixid = {0}
order by (compid = basecompid)
", (int)id);
                OdbcDataReader dr = cm.ExecuteReader();
                Mix mix = new Mix();
                mix.MixType = MixTypes.Reference;
                mix.MixID = (int)id;

                while (dr.Read())
                {
                    mix.MixNumber = (string)dr["mixnumber"];
                    Mix.MixComp mc = new Mix.MixComp();
                    mc.InMix = mix;
                    mc.CompID = (int)dr["compid"];
                    mc.Target = (bool)dr["target"];
                    mc.Diluent = (bool)dr["diluent"];
                    mc.Conc = dr["c"] is double ? (double)dr["c"] : double.NaN;
                    mc.ConcAbsError = dr["dcabs"] is double ? (double)dr["dcabs"] : double.NaN;
                    mc.ConcRelError = dr["dcrel"] is double ? (double)dr["dcrel"] : double.NaN;
                    mix.Comps.Add(mc);
                }
                return mix;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Disconnect();
            }
            return null;
        }
        /// <summary>
        /// Загрузить смесь заказчика по номеру смеси
        /// </summary>
        /// <param name="MixNumber"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        static public Mix LoadCustomerMix(string MixNumber, OdbcConnection connection = null)
        {
            OdbcConnection con = connection;
            if (con == null)
            {
                con = new OdbcConnection(DB.ConnectionString);
            }

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                IDbCommand cm = con.CreateCommand();

                cm.CommandText = string.Format(@"
SELECT mixes.id
FROM Customers.Mixes LEFT JOIN customers.mix_prefixes ON mix_prefixes.id = mixes.mix_prefix
WHERE concat(mix_prefixes.prefix, '-', mixes.mix_serial) like '{0}'", MixNumber);
                int? id = (int?)cm.ExecuteScalar();
                if (id != null)
                {
                    Mix mix = LoadCustomerMix((int)id, con);
                    mix.MixNumber = MixNumber;
                    return mix;
                }
                else
                {
                    MessageBox.Show("Смесь '" + MixNumber + "' не найдена!", "Поиск смеси в базе", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection == null)
                    con.Close();
            }
            return null;
        }
        /// <summary>
        /// Загрузить смесь заказчика по ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        static public Mix LoadCustomerMix(int id, OdbcConnection connection = null)
        {
            OdbcConnection con = connection;
            if (con == null)
            {
                con = new OdbcConnection(DB.ConnectionString);
            }

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                IDbCommand cm = con.CreateCommand();

                cm.CommandText = string.Format(@"
SELECT 
    CompID, 
    Conc,
    diluent
FROM 
    Customers.MixComps
WHERE 
    (MixID = {0})
ORDER BY 
    not diluent", (int)id);
                IDataReader dr = cm.ExecuteReader();
                Mix mix = new Mix();
                mix.MixType = MixTypes.Mix;
                mix.MixID = (int)id;
                while (dr.Read())
                {
                    Mix.MixComp mc = new Mix.MixComp();
                    mc.InMix = mix;
                    mc.CompID = (int)dr["CompID"];
                    mc.Properties = CompInfo.Load(mc.CompID).Properties;
                    mc.Conc = (double)dr["Conc"];
                    mc.Diluent = (bool)dr["diluent"];
                    mc.ConcAbsError = double.NaN;
                    mc.ConcRelError = double.NaN;
                    mix.Comps.Add(mc);
                }
                return mix;
            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection == null)
                    con.Close();
            }
            return null;
        }
        /// <summary>
        /// Загрузить состав исходника по ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        static public Mix LoadRawMix(int id, OdbcConnection connection = null)
        {
            OdbcConnection con = connection;
            if (con == null)
            {
                con = new OdbcConnection(DB.ConnectionString);
            }

            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                IDbCommand cm = con.CreateCommand();
                cm.CommandText = string.Format(@"
SELECT 
    compid, 
    target,
    c,
    dcabs,
    dcrel
FROM 
    raw_materials.raw_comps
WHERE 
    (rawid = {0})", (int)id);
                IDataReader dr = cm.ExecuteReader();
                Mix mix = new Mix();
                mix.MixType = MixTypes.SourceMix;
                mix.MixID = (int)id;
                while (dr.Read())
                {
                    Mix.MixComp mc = new Mix.MixComp();
                    mc.InMix = mix;
                    mc.CompID = (int)dr["compid"];
                    mc.Properties = CompInfo.Load(mc.CompID).Properties;
                    mc.Target = (bool)dr["target"];
                    mc.Conc = (double)dr["c"];
                    mc.ConcAbsError = dr["dcabs"] is double ? (double)dr["dcabs"] : double.NaN;
                    mc.ConcRelError = dr["dcrel"] is double ? (double)dr["dcrel"] : double.NaN;
                    mix.Comps.Add(mc);
                }
                return mix;


            }
            catch (Exception)
            {

            }
            finally
            {
                if (connection == null)
                    con.Close();
            }
            return null;
        }
    }
}
