using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbMigration
{
    public class MsAccessHandler
    {
        private readonly string _pathToMdb;
        private readonly string _password;

        #region Constructor
        public MsAccessHandler(string pathToMdbFile)
        {
            this._pathToMdb = pathToMdbFile;
        }

        public MsAccessHandler(string pathToMdbFile, string password)
        {
            this._pathToMdb = pathToMdbFile;
            this._password = password;
        }
        #endregion

        #region Components
        
        public enum MixType
        {
            Raw = 1,
            Mix = 2
        }

        public class PrimMixGas    
        {
            public int Id { get; private set; }
            public DateTime? Date { get; private set; }
            public DateTime? EndedDate { get; private set; }
            public int TypMix { get; private set; }
            public int ConcUnit { get; private set; }
            public string NumMix { get; private set; }
            public string NumCyl { get; private set; }
            public string ManCyl { get; private set; }
            public double? VolCyl { get; private set; }
            public int? TypCyl { get; private set; }
            public int? TypValve { get; private set; }
            public string Comment { get; private set; }
            public int? IdBaseAdm { get; set; }
            public double? Quantity { get; private set; }
            public int? QuantityUnit { get; private set; }
            public DateTime? QuantityDate { get; private set; }
            public double? Tare { get; private set; }
            public double? Net { get; private set; }
            public double? Gross { get; private set; }
            public int Owner { get; private set; }
            
            public int? IdGas { get; private set; }

            public PrimMixGas(
                int id, DateTime? date, DateTime? endedDate, int typMix, int concUnit, string numMix, string numCyl,
                string manCyl, double? volCyl, int? typCyl, int? typValve, string comment, int? idBaseAdm,
                double? quantity, int? quantityUnit, DateTime? quantityDate, double? tare, double? net, double? gross,
                int owner, int? idGas)
            {
                this.Id = id;
                this.Date = date;
                this.EndedDate = endedDate;
                this.TypMix = typMix;
                this.ConcUnit = concUnit;
                this.NumMix = numMix;
                this.NumCyl = numCyl;
                this.ManCyl = manCyl;
                this.VolCyl = volCyl;
                this.TypCyl = typCyl;
                this.TypValve = typValve;
                this.Comment = comment;
                this.IdBaseAdm = idBaseAdm;
                this.Quantity = quantity;
                this.QuantityUnit = quantityUnit;
                this.QuantityDate = quantityDate;
                this.Tare = tare;
                this.Net = net;
                this.Gross = gross;
                this.Owner = owner;
                this.IdGas = idGas;
            }
            
            /// <summary>
            /// Корректирует пустые поля
            /// </summary>
            public void PrimMixGasValidator()
            {
                if (this.VolCyl == null)
                {
                    this.VolCyl = 4;
                }

                if (this.Date == null)
                {
                    this.Date = new DateTime();
                }
            }
        }

        public class GasAdm
        {
            public int Id { get; private set; }
            public int IdCyl { get; private set; }
            public bool Target { get; private set; }
            public int IdGas { get; private set; }
            public double? Conc { get; private set; }
            public double? DCabs { get; private set; }
            public double? DCrel { get; private set; }
            
            public int MixType { get; private set; }

            public GasAdm(int id, int idCyl, bool target, int idGas, double? conc, double? dCabs, double? dCrel, int mixType)
            {
                this.Id = id;
                this.IdCyl = idCyl;
                this.Target = target;
                this.IdGas = idGas;
                this.Conc = conc;
                this.DCabs = dCabs;
                this.DCrel = dCrel;
                this.MixType = mixType;
            }
            
        }
        
        public class Spending
        {
            public int Id { get; private set; }
            public int MixId { get; private set; }
            public DateTime Date { get; private set; }
            public string Order { get; private set; }
            public int OpType { get; private set; }
            public double Quantity { get; private set; }
            public int UserId { get; private set; }

            public Spending(int id, int mixId, DateTime date, string order, int opType, double quantity, int userId)
            {
                this.Id = id;
                this.MixId = mixId;
                this.Date = date;
                this.Order = order;
                this.OpType = opType;
                this.Quantity = quantity;
                this.UserId = userId;
            }
        }
        
        //Класс описывающий тип баллона и тип вентеля
        private class Cyl
        {
            /*
                1 "Черный"
                2 "Эмаль"
                3 "Luxfer"
                4 "БМК"
                5 "Поиск"
                6 "лакс дл."
                7 "лакс Китай 
                8 "Нерж."
                9 "Алюминий"
                10 "Элина-Т"
                11 "Нерж. гвоздик"
                12 "Стеклотара"
            */
            public int? CylType { get; private set; }
            /*
                1 "Латунь"
                2 "Нерж"
                3 "H12"
                4 "Латунь (Левый)"
                5 "Нерж (Левый)"
            */
            public int? ValveType { get; private set; }
            

            private Cyl(int? cylType, int? valveType)
            {
                this.CylType = cylType;
                this.ValveType = valveType;
            }

            static public Cyl CylConverter(int typCyl)
            {
                switch (typCyl)
                {
                    case 1:// ЭлинаТ + латунь
                        return new Cyl(10,1);
                    case 2:     //Lux + H12
                        return new Cyl(3, 3);
                    case 3: //	черный + латунь
                        return new Cyl(1, 1);
                    case 4://	черный + нерж.
                        return new Cyl(1, 2);
                    case 5: // черный + Н12
                        return new Cyl(1, 3);
                    case 6: // Lux + латунь
                        return new Cyl(3, 1);
                    case 7: // Lux + нерж.
                        return new Cyl(3, 2);
                    case 8: // алюминий + латунь
                        return new Cyl(9, 1);
                    case 9: // алюминий + нерж.
                        return new Cyl(9, 2);
                    case 10: // алюминий + Н12
                        return new Cyl(9, 3);
                    case 11: // БМК + латунь
                        return new Cyl(4, 1);
                    case 12: // БМК + нерж.
                        return new Cyl(4, 2);
                    case 13: // БМК + Н12
                        return new Cyl(4, 3);
                    case 18: // Нерж. гвоздик + Н12
                        return new Cyl(11, 3);
                    case 19: // Нержавейка + нерж.
                        return new Cyl(8, 2);
                    case 20: // Эмаль + латунь
                        return new Cyl(2, 1);
                    case 21: // Стеклотара
                        return new Cyl(12, null);
                    case 22: // черный
                        return new Cyl(1, 1);
                    default:
                        return new Cyl(null, null);
                }
            }
        }
        #endregion
        
        #region Connect methods
        
        /// <summary>
        /// Создаёт подключение с указанным фалом базы данных
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private OleDbConnection Connect(string path)
        {
            var connectionStr = string.Format(
                "provider=Microsoft.Jet.OLEDB.4.0;data source={0}", path);

            var connection = new OleDbConnection(connectionStr);
            connection.Open();

            return connection;
        }

        /// <summary>
        /// Создаёт подключение с указанным фалом базы данных с паролем
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private OleDbConnection Connect(string path, string password)
        {
            var connectionStr = string.Format(
                "provider=Microsoft.Jet.OLEDB.4.0;data source={0}; " +
                "Jet OLEDB:Database Password = {1}", path, password);

            var connection = new OleDbConnection(connectionStr);
            connection.Open();

            return connection;
        }
        
        public OleDbConnection GetConnection()
        {
            return _password == "" ? Connect(_pathToMdb) : Connect(_pathToMdb, _password);
        }
        
        /// <summary>
        /// Проверка соединения путём выполнения запроса "SELECT 1"
        /// </summary>
        /// <returns></returns>
        public bool CheckConnection()
        {
            var connection = GetConnection();
            var command = GetCommand(connection);
            command.CommandText = "SELECT 1";

            try
            {
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
                return false;
            }
        }
        #endregion
        
        public OleDbCommand GetCommand(OleDbConnection msAccessConnection)
        {
            return msAccessConnection.CreateCommand();
        }

        /// <summary>
        /// Возвращает тип баллона (Сырьё/весовые). 
        /// Медленная версия, есть возможность оптимизации
        /// </summary>
        /// <returns></returns>
        [Obsolete("Метод неэффективен для большого количества записей")]
        public MixType? GetMixType(OleDbConnection connection, int idCyl)
        {
            var command = GetCommand(connection);
            command.CommandText = string.Format("SELECT TypMix FROM PrimMixGas WHERE ID = {0}", idCyl);
            try
            {
                using (var r = command.ExecuteReader())
                {
                    r.Read();
                    var mixType = (int) r[0];
                    
                    switch (mixType)
                    {
                        case 1:
                            return MixType.Raw;
                        case 2:
                            return MixType.Mix;
                        default:
                            return null;
                    }
                }
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
                return null;
            }
        }

        #region Read methods

        /// <summary>
        /// Возвращает лист записей PrimMixGas
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public List<PrimMixGas> ReadPrimMixGasTable(OleDbConnection connection)
        {
            var command = GetCommand(connection);
            
            var primMixGasList = new List<PrimMixGas>();

            command.CommandText = "SELECT PrimMixGas.ID, PrimMixGas.Date, PrimMixGas.EndedDate, PrimMixGas.TypMix, " +
                                  "PrimMixGas.ConcUnit, PrimMixGas.NumMix, PrimMixGas.NumMix, PrimMixGas.NumCyl, PrimMixGas.ManCyl, " +
                                  "PrimMixGas.VolCyl, PrimMixGas.TypCyl, PrimMixGas.TypValve, PrimMixGas.Coment, PrimMixGas.IDBaseAdm, " +
                                  "PrimMixGas.Quantity, PrimMixGas.QuantityUnit, PrimMixGas.QuantityDate, PrimMixGas.Tare, PrimMixGas.Net, PrimMixGas.Gross, " +
                                  "PrimMixGas.Owner, GasAdm.IDGas FROM PrimMixGas LEFT JOIN GasAdm ON PrimMixGas.IDBaseAdm=GasAdm.ID";

            using (var r = command.ExecuteReader())
            {
                while (r.Read())
                {
                    var cyl = Cyl.CylConverter((int)  r["TypCyl"]);
                    
                    primMixGasList.Add(new PrimMixGas(
                        (int) r["ID"], r["Date"] as DateTime?, r["EndedDate"] as DateTime?,
                        (int)  r["TypMix"], (int)  r["ConcUnit"], r["NumMix"] as string,
                        r["NumCyl"] as string, r["ManCyl"] as string, r["VolCyl"] as double?,
                        cyl.CylType, cyl.ValveType, r["Coment"] as string,
                         r["IDBaseAdm"] as int?, r["Quantity"] as double?,
                        r["QuantityUnit"] as int?,r["QuantityDate"] as DateTime?,
                        r["Tare"] as double?, r["net"] as double?, r["Gross"] as double?,
                        (int)  r["Owner"], r["IDGas"] as int?));
                }
            }

            return primMixGasList;
        }

        /// <summary>
        /// Возвращает итерируемый объект GasAdm записей
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public IEnumerable<GasAdm> ReadGasAdmTable(OleDbConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = "SELECT GasAdm.ID, GasAdm.IDCyl, GasAdm.Target, GasAdm.IDGas, GasAdm.Conc, GasAdm.dCabs, " +
                                  "GasAdm.dCrel, PrimMixGas.TypMix FROM GasAdm LEFT JOIN PrimMixGas ON GasAdm.IDCyl = PrimMixGas.ID";

            using (var r = command.ExecuteReader())
            {
                while (r.Read())
                {
                    yield return new GasAdm(
                        (int) r["ID"], (int) r["IDCyl"], (bool) r["Target"],
                        (int) r["IDGas"], r["Conc"] as double?,r["dCabs"] as double?,
                        r["dCrel"] as double?, (int) r["TypMix"]);
                }
            }
        }
        
        /// <summary>
        /// Возвращает итерируемый объект GasAdm записей для определённого IDCyl
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public IEnumerable<GasAdm> ReadGasAdmTable(OleDbConnection connection, int IDCyl)
        {
            var command = connection.CreateCommand();

            command.CommandText = string.Format(
                "SELECT GasAdm.ID, GasAdm.IDCyl, GasAdm.Target, GasAdm.IDGas, GasAdm.Conc, GasAdm.dCabs, " +
                "GasAdm.dCrel PrimMixGas.TypMix FROM GasAdm LEFT JOIN PrimMixGas ON PrimMixGas GasAdm.IDCyl = PrimMixGas.ID WHERE IdCyl = {0}",
                IDCyl);

            using (var r = command.ExecuteReader())
            {
                while (r.Read())
                {
                    yield return new GasAdm(
                        (int) r["ID"], (int) r["IDCyl"], (bool) r["Target"],
                        (int) r["IDGas"], r["Conc"] as double?,r["dCabs"] as double?,
                        r["dCrel"] as double?, (int) r["TypMix"]);
                }
            }
        }

        /// <summary>
        /// Возвращает итерируемый объект Spending записей
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public IEnumerable<Spending> ReadSpendingTable(OleDbConnection connection)
        {
            var command = connection.CreateCommand();

            command.CommandText = "SELECT ID, MixID, Date, Order, OpType, Quantity, UserID FROM Spending";

            using (var r = command.ExecuteReader())
            {
                while (r.Read())
                {
                    yield return new Spending(
                        (int) r["ID"], (int) r["MixID"], (DateTime) r["Date"], (string) r["Order"],
                        (int) r["OpType"], (double) r["Quantity"], (int) r["UserID"]);
                }
            }
        }

        #endregion
    }
}   
    