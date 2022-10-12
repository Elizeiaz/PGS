using PGS.Comps;
using PGS.Constants;
using PGS.Mixes;
using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace PGS
{
    /// <summary>
    /// Класс для конвертации единиц измерения концентрации
    /// </summary>
    static public class Concentration
    {
        /// <summary>
        /// Единицы измерения концентрации согласно БД
        /// </summary>
        public enum Units
        {
            /// <summary>
            /// Молярная доля, %
            /// </summary>
            Molar = 0,
            /// <summary>
            /// Массовая доля, %
            /// </summary>
            Mass = 1,
            /// <summary>
            /// Массовая концентрация мг/м3
            /// </summary>
            MassConc = 2,
            /// <summary>
            /// Объемная доля, %
            /// </summary>
            Volume = 3,
            /// <summary>
            /// Молярная доля, ppm
            /// </summary>
            Molar_ppm = 4
        }

        /// <summary>
        /// Информация о единице измерения из БД
        /// </summary>
        public class UnitInfo
        {
            /// <summary>
            /// ID Единицы измерения в базе
            /// </summary>
            public int ID { get; }
            /// <summary>
            /// Название
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Описание
            /// </summary>
            public string Description { get; }
            /// <summary>
            /// Значение
            /// </summary>
            public Units Units { get; }
            /// <summary>
            /// Конструктор
            /// </summary>
            static UnitInfo()
            {
                LoadCache();
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="ID"></param>
            /// <param name="Name"></param>
            /// <param name="Description"></param>
            public UnitInfo(int ID, string Name, string Description)
            {
                this.ID = ID;
                this.Name = Name;
                this.Description = Description;
                Units = (Units)ID;
            }

            /// <summary>
            /// Загрузить кэш
            /// </summary>
            static private void LoadCache()
            {
                OdbcConnection con = DB.Connect();
                try
                {
                    OdbcCommand cm = new OdbcCommand("", con);
                    cm.CommandText = @"select * from common.conc_units";
                    OdbcDataReader dr = cm.ExecuteReader();
                    _cache.Clear();
                    while (dr.Read())
                    {
                        _cache.Add((int)dr["id"], new UnitInfo((int)dr["id"], (string)dr["name"], (string)dr["description"]));
                    }
                }
                finally
                {
                    con.Disconnect();
                }
            }
            /// <summary>
            /// Кэш данных
            /// </summary>
            private static Dictionary<int, UnitInfo> _cache = new Dictionary<int, UnitInfo>();
            /// <summary>
            /// Список единиц измерения
            /// </summary>
            public static List<UnitInfo> List
            {
                get
                {
                    return new List<UnitInfo>(_cache.Values);
                }
            }
            /// <summary>
            /// Загрузить информацию о единице измерения
            /// </summary>
            /// <param name="units"></param>
            /// <returns></returns>
            static public UnitInfo Load(Units units)
            {
                return Load((int)units);
            }
            /// <summary>
            /// Загрузить информацию о единице измерения
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            static public UnitInfo Load(int id)
            {
                if (_cache.ContainsKey(id))
                {
                    return _cache[id];
                }
                else throw new Exception(string.Format("Единиц измерения с id = {0} не найдены", id));
            }
        }




        /// <summary>
        /// ID компонентов согласно базе
        /// </summary>
        public static class Comps
        {
            /// <summary>
            /// Азот
            /// </summary>
            public const int N2 = 154;
            /// <summary>
            /// Кислород
            /// </summary>
            public const int O2 = 155;
            /// <summary>
            /// Воздух
            /// </summary>
            public const int Air = 190;
        }


        /// <summary>
        /// Пересчет единиц измерения для бинарных смесей
        /// </summary>
        /// <param name="To"></param>
        /// <param name="From"></param>
        /// <param name="Conc"></param>
        /// <param name="CompID"></param>
        /// <param name="DiluentID"></param>
        /// <returns></returns>
        static public double BinaryMixConvert(Units To, Units From, double Conc, int CompID, int DiluentID)
        {
            Mix.MixComp cmp = new Mix.MixComp(PGS.Comps.CompProperties.Load(CompID), Conc);
            Mix.MixComp diluent = new Mix.MixComp(PGS.Comps.CompProperties.Load(DiluentID), 100 - Conc);
            return Convert(To, From, cmp, diluent).Comps[0].Conc;
        }

        /// <summary>
        /// Пересчет концентраций смеси в заданные единицы
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Mix"></param>
        /// <returns></returns>
        static public IMix Convert(Units To, IMix Mix)
        {
            return Convert(To, Mix.Units, Mix.Comps);
        }

        /// <summary>
        /// Пересчет концентрация в заданные величины
        /// </summary>
        /// <param name="To"></param>
        /// <param name="From"></param>
        /// <param name="Comps"></param>
        /// <returns></returns>
        static public IMix Convert(Units To, Units From, params IMixComp[] Comps)
        {
            switch (From)
            {
                case Units.Molar:
                    return MolarTo(To, Comps);
                case Units.Mass:
                    return MassTo(To, Comps);
                case Units.MassConc:
                    return MassConcTo(To, Comps);
                case Units.Volume:
                    return VolumeTo(To, Comps);
                default:
                    throw new Exception("Преобразование не поддерживается!");
            }
        }

        /// <summary>
        /// Преобразование из молярных долей в 
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Comps"></param>
        /// <returns></returns>
        static private IMix MolarTo(Units To, IMixComp[] Comps)
        {
            switch (To)
            {
                case Units.Molar:
                    //Без прообразования
                    return new Mix(To, Comps);

                case Units.Mass:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Ms = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Ms += Comps[i].Conc * Comps[i].Properties.MolarMass;
                        }

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                                100 * Comps[i].Conc * Comps[i].Properties.MolarMass / Ms);
                            (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                        }
                        return R;
                    }
                case Units.MassConc:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Zs = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Zs += Comps[i].Conc * Comps[i].Properties.Z;
                        }

                        double a = GasConst.Po / (GasConst.R * GasConst.To);
                        double fs = 1;

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            if (Comps[i].Diluent)
                            {
                                R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties);
                            }
                            else
                            {
                                //1000 - перевод в мг/м3
                                R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                                   1000 * Comps[i].Conc * Comps[i].Properties.MolarMass * a / fs / Zs);
                                (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            }
                        }

                        return R;
                    }
                case Units.Volume:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Zs = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Zs += Comps[i].Conc * Comps[i].Properties.Z;
                        }

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                                100 * Comps[i].Conc * Comps[i].Properties.Z / Zs);
                            (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                        }
                        return R;
                    }
                default:
                    throw new Exception("Преобразование не поддерживается!");
            }
        }

        /// <summary>
        /// Преобразование из массовых долей в 
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Comps"></param>
        /// <returns></returns>
        static private IMix MassTo(Units To, IMixComp[] Comps)
        {
            switch (To)
            {
                case Units.Molar:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Ws = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Ws += Comps[i].Conc / Comps[i].Properties.MolarMass;
                        }

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                                100 * Comps[i].Conc / Comps[i].Properties.MolarMass / Ws);
                            (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                        }
                        return R;
                    }
                case Units.Mass:
                    //Без прообразования
                    return new Mix(To, Comps);
                case Units.MassConc:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Zs = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Zs += (Comps[i].Conc / Comps[i].Properties.MolarMass) * Comps[i].Properties.Z;
                        }

                        double a = GasConst.Po / (GasConst.R * GasConst.To);
                        double fs = 1;

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            if (Comps[i].Diluent)
                            {
                                R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties);
                            }
                            else
                            {
                                R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                                  1000 * Comps[i].Conc * a / fs / Zs);
                                (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            }
                        }

                        return R;
                    }
                case Units.Volume:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Zs = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Zs += (Comps[i].Conc / Comps[i].Properties.MolarMass) * Comps[i].Properties.Z;
                        }

                        for (int i = 0; i < Comps.Length; i++)
                        {

                            R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                              100 * Comps[i].Conc * Comps[i].Properties.Z / Comps[i].Properties.MolarMass / Zs);
                            (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                        }

                        return R;
                    }
                default:
                    throw new Exception("Преобразование не поддерживается!");
            }

        }
        /// <summary>
        /// Преобразование из объемной долей в 
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Comps"></param>
        /// <returns></returns>
        static private IMix VolumeTo(Units To, IMixComp[] Comps)
        {
            switch (To)
            {
                case Units.Molar:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Zs = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Zs += (Comps[i].Conc / Comps[i].Properties.Z);
                        }

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                             100 * Comps[i].Conc / (Comps[i].Properties.Z * Zs));
                            (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                        }

                        return R;
                    }
                case Units.Mass:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double Zs = 0;
                        for (int i = 0; i < Comps.Length; i++)
                        {
                            Zs += (Comps[i].Conc * Comps[i].Properties.MolarMass / Comps[i].Properties.Z);
                        }

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                             100 * Comps[i].Conc * Comps[i].Properties.MolarMass / (Comps[i].Properties.Z * Zs));
                            (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                        }

                        return R;
                    }
                case Units.MassConc:
                    {
                        Mix R = new Mix(To, new Mix.MixComp[Comps.Length]);

                        double a = GasConst.Po / (GasConst.R * GasConst.To);
                        double fs = 1;

                        for (int i = 0; i < Comps.Length; i++)
                        {
                            R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                             1000 * Comps[i].Conc / 100 * a * Comps[i].Properties.MolarMass / (fs * Comps[i].Properties.Z));
                            (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                            (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                        }

                        return R;
                    }
                case Units.Volume:
                    //Без прообразования
                    return new Mix(To, Comps);
                default:
                    throw new Exception("Преобразование не поддерживается!");
            }
        }
        /// <summary>
        /// Преобразование из массовой концентрации
        /// </summary>
        /// <param name="To"></param>
        /// <param name="Comps"></param>
        /// <returns></returns>
        static private IMix MassConcTo(Units To, IMixComp[] Comps)
        {
            if (To != Units.MassConc)
            {
                Mix R = new Mix(Units.Volume, new Mix.MixComp[Comps.Length]);

                double a = GasConst.Po / (GasConst.R * GasConst.To);
                double fs = 1;

                for (int i = 0; i < Comps.Length; i++)
                {
                    if (Comps[i].Diluent)
                    {
                        R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties);
                    }
                    else
                    {
                        R.Comps[i] = new Mix.MixComp(R, Comps[i].Properties,
                         100 * Comps[i].Conc / 1000 * (fs * Comps[i].Properties.Z) / (a * Comps[i].Properties.MolarMass));
                        (R.Comps[i] as Mix.MixComp).ConcAbsError = R.Comps[i].Conc * (Comps[i].ConcAbsError / Comps[i].Conc);
                        (R.Comps[i] as Mix.MixComp).Diluent = Comps[i].Diluent;
                    }
                }
                return Convert(To, R);
            }
            else
            {
                //Без прообразования
                return new Mix(To, Comps);
            }

        }
    }
}