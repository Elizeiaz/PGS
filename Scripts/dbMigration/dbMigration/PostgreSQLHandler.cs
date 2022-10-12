using System;
using System.Data.Odbc;

namespace dbMigration
{
    public class PostgreSQLHandler
    {
        /// <summary>
        /// Хранит колличество добавленных и изменных записей
        /// </summary>
        public class Counter
        {
            public int Inserted { get; private set; }
            public int Updated { get; private set; }
            public int Error { get; private set; }

            public Counter(int inserted, int updated, int error)
            {
                this.Inserted = inserted;
                this.Updated = updated;
                this.Error = error;
            }
            
            public Counter()
            {
                this.Inserted = 0;
                this.Updated = 0;
                this.Error = 0;
            }
                
            /// <summary>
            /// Метод обновляет значения счётчика в зависимости от RequestType.
            /// Возвращает false если ни один счётчик не был увеличен.
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            public bool UpdateCounter(RequestType request)
            {
                switch (request)
                {
                    case RequestType.Insert:
                        this.Inserted++;
                        return true;
                    case RequestType.Update:
                        this.Updated++;
                        return true;
                    case RequestType.Error:
                        this.Error++;
                        return true;
                }

                return false;
            }
        }
        
        /// <summary>
        /// Тип записи в таблицу
        /// </summary>
        public enum RequestType
        {
            Insert,
            Update,
            WithoutChanges,
            Error
        }

        public OdbcConnection GetConnection()
        {
            return PGS.DB.Connect();
        }

        public OdbcCommand GetCommand(OdbcConnection connection)
        {
            return connection.CreateCommand();
        }

        public bool CheckConnection()
        {
            try
            {
                var con = PGS.DB.Connect();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
                return false;
            }
        }

        /// <summary>
        /// Замена "= null" конвертируются в "is NULL"
        /// </summary>
        /// <returns></returns>
        public string NullConverter(string strWithNull)
        {
            return strWithNull.Replace("= null", "is NULL");
            
        }

        /// <summary>
        /// Проверяет на наличие primMixGas запись
        /// </summary>
        /// <param name="command"></param>
        /// <param name="mixType">Тип смеси (Raw/Mix)</param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckPrimMixGas(OdbcCommand command, int mixType ,int id)
        {
            command.CommandText = mixType == (int) MsAccessHandler.MixType.Mix 
                ? $"SELECT id FROM lab_references.mixes WHERE id = {id}" 
                : $"SELECT id FROM raw_materials.raws WHERE id = {id}";

            try
            {
                return command.ExecuteNonQuery() == 1;
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
            }

            return false;
        }
        
        /// <summary>
        /// Производит проверку на наличие элемента, вставляет его в случае отсутствия.
        /// Если элемент присутствует в таблице, но поля его отличаются, но значения обновляются
        /// </summary>
        public RequestType InsertPrimMixGas(OdbcCommand command, MsAccessHandler.PrimMixGas item, int cylId)
        {
            string selectFromIdString;
            string insertIntoString;
            string updateString;

            //Формирование строк в зависимости от типа смеси
            #region stringFormat

            if (item.TypMix == (int) MsAccessHandler.MixType.Raw)
            {
                selectFromIdString = string.Format("SELECT * FROM raw_materials.raws WHERE id = {0}", item.Id);
                         
                insertIntoString = string.Format(
                    "INSERT INTO raw_materials.raws" +
                    "(id, date, ended_date, conc_units, cylid, manufacturer, comment, quantity, quantity_units, tare, net, gross, ownerid) " +
                    "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12})",
                    item.Id,
                    PGS.DB.ToDBDate(item.Date),
                    PGS.DB.ToDBDate(item.EndedDate),
                    item.ConcUnit,
                    PGS.DB.ToDBString(cylId),
                    PGS.DB.ToDBString(item.ManCyl),
                    PGS.DB.ToDBString(item.Comment),
                    PGS.DB.ToDBString(item.Quantity),
                    PGS.DB.ToDBString(item.QuantityUnit),
                    PGS.DB.ToDBString(item.Tare),
                    PGS.DB.ToDBString(item.Net),
                    PGS.DB.ToDBString(item.Gross),
                    item.Owner);

                var whereString = string.Format
                ("WHERE id = {0} AND NOT (date = {1} and ended_date = {2} and conc_units = {3} and cylid = {4} and manufacturer = {5} " +
                 "and comment = {6} and quantity = {7} and quantity_units = {8} and tare = {9} and net = {10} and gross = {11} and ownerid = {12})",
                    item.Id,
                    PGS.DB.ToDBDate(item.Date),
                    PGS.DB.ToDBDate(item.EndedDate),
                    item.ConcUnit,
                    PGS.DB.ToDBString(cylId),
                    PGS.DB.ToDBString(item.ManCyl),
                    PGS.DB.ToDBString(item.Comment),
                    PGS.DB.ToDBString(item.Quantity),
                    PGS.DB.ToDBString(item.QuantityUnit),
                    PGS.DB.ToDBString(item.Tare),
                    PGS.DB.ToDBString(item.Net),
                    PGS.DB.ToDBString(item.Gross),
                    item.Owner);

                whereString = NullConverter(whereString);
                
                updateString = string.Format(
                    "UPDATE raw_materials.raws " +
                    "SET id = {0}, date = {1}, ended_date = {2}, conc_units = {3}, cylid = {4}, manufacturer = {5}, comment = {6}, " +
                    "quantity = {7}, quantity_units = {8}, tare = {9}, net = {10}, gross = {11}, ownerid = {12}" + whereString,
                    item.Id,
                    PGS.DB.ToDBDate(item.Date),
                    PGS.DB.ToDBDate(item.EndedDate),
                    item.ConcUnit,
                    PGS.DB.ToDBString(cylId),
                    PGS.DB.ToDBString(item.ManCyl),
                    PGS.DB.ToDBString(item.Comment),
                    PGS.DB.ToDBString(item.Quantity),
                    PGS.DB.ToDBString(item.QuantityUnit),
                    PGS.DB.ToDBString(item.Tare),
                    PGS.DB.ToDBString(item.Net),
                    PGS.DB.ToDBString(item.Gross),
                    item.Owner);
            }
            else
            {
                selectFromIdString = string.Format("SELECT * FROM lab_references.mixes WHERE id = {0}", item.Id);
                         
                insertIntoString = string.Format(
                    "INSERT INTO lab_references.mixes" +
                    "(id, date, ended_date, mixnumber, cylid, manufacturer, basecompid, ownerid, pressure, pressuredate, comment) " +
                    "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})",
                    item.Id,
                    PGS.DB.ToDBDate(item.Date),
                    PGS.DB.ToDBDate(item.EndedDate),
                    PGS.DB.ToDBString(item.NumMix),
                    PGS.DB.ToDBString(cylId),
                    PGS.DB.ToDBString(item.ManCyl),
                    PGS.DB.ToDBString(item.IdGas),
                    item.Owner,
                    PGS.DB.ToDBString(item.Quantity),
                    PGS.DB.ToDBDateTime(item.QuantityDate),
                    PGS.DB.ToDBString(item.Comment));
                
                var whereString = string.Format(
                    "WHERE id = {0} AND NOT (date = {1} AND ended_date = {2} AND mixnumber = {3} AND cylid = {4} AND manufacturer = {5} " +
                    "AND basecompid = {6} AND ownerid = {7} AND pressure = {8} AND pressuredate = {9} AND comment = {10})",
                    item.Id,
                    PGS.DB.ToDBDate(item.Date),
                    PGS.DB.ToDBDate(item.EndedDate),
                    PGS.DB.ToDBString(item.NumMix),
                    PGS.DB.ToDBString(cylId),
                    PGS.DB.ToDBString(item.ManCyl),
                    PGS.DB.ToDBString(item.IdGas),
                    item.Owner,
                    PGS.DB.ToDBString(item.Quantity),
                    PGS.DB.ToDBDateTime(item.QuantityDate),
                    PGS.DB.ToDBString(item.Comment));

                whereString = NullConverter(whereString);
                
                updateString = string.Format(
                    "UPDATE lab_references.mixes " +
                    "SET id = {0}, date = {1}, ended_date = {2}, mixnumber = {3}, cylid = {4}, manufacturer = {5}, basecompid = {6}, " +
                    "ownerid = {7}, pressure = {8}, pressuredate = {9}, comment = {10} " + whereString,
                    item.Id,
                    PGS.DB.ToDBDate(item.Date),
                    PGS.DB.ToDBDate(item.EndedDate),
                    PGS.DB.ToDBString(item.NumMix),
                    PGS.DB.ToDBString(cylId),
                    PGS.DB.ToDBString(item.ManCyl),
                    PGS.DB.ToDBString(item.IdGas),
                    item.Owner,
                    PGS.DB.ToDBString(item.Quantity),
                    PGS.DB.ToDBDateTime(item.QuantityDate),
                    PGS.DB.ToDBString(item.Comment));
            }

            #endregion
                     
            //Проверка на отсутствие записи
            command.CommandText = selectFromIdString;
            if (command.ExecuteNonQuery() == 0)
            {
                command.CommandText = insertIntoString;
         
                try
                {
                    command.ExecuteNonQuery();
                    return RequestType.Insert;
                }
                catch (Exception e)
                {
                    Program.ErrorMessage(e);
                    return RequestType.WithoutChanges;
                }
            }
                     
            //Проверка на отличия и замена в случае таковых
            command.CommandText = updateString;
            try
            {
                return command.ExecuteNonQuery() == 0 ? RequestType.WithoutChanges : RequestType.Update;
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
                return RequestType.WithoutChanges;    
            }
        }

        /// <summary>
        /// Производит проверку на наличие элемента, вставляет его в случае отсутствия.
        /// Если элемент присутствует в таблице, но поля его отличаются, но значения обновляются
        /// </summary>
        /// <param name="command"></param>
        /// <param name="item"></param>
        /// <param name="mixType"></param>
        /// <returns></returns>
        public RequestType InsertGasAdm(OdbcCommand command, MsAccessHandler.GasAdm item, MsAccessHandler.MixType mixType)
        {
            // Обработка разных mixType
            string tableName;
            string idCylName;
            
            if (mixType == MsAccessHandler.MixType.Raw)
            {
                tableName = "raw_materials.raw_comps";
                idCylName = "rawid";
            }    
            else
            {
                tableName = "lab_references.mix_comps";
                idCylName = "mixid";
            }

            var insertInto = tableName + "(id, " + idCylName + ", compid, target, c, dcabs, dcrel)";
            
            //Проверка на отсутствие записи
            command.CommandText = string.Format("SELECT id FROM " + tableName + " WHERE id = {0}", item.Id);
            if (command.ExecuteNonQuery() == 0)
            {
                command.CommandText = string.Format(
                    "INSERT INTO " + insertInto + " " +
                    "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                    item.Id, item.IdCyl, item.IdGas, item.Target,
                    PGS.DB.ToDBString(item.Conc), PGS.DB.ToDBString(item.DCabs), PGS.DB.ToDBString(item.DCrel));

                try
                {
                    command.ExecuteNonQuery();
                    return RequestType.Insert;
                }
                catch (Exception e)
                {
                    Program.ErrorMessage(e);
                    return RequestType.WithoutChanges;
                }
            }
            
            //Проверка на отличия
            string whereString = string.Format("WHERE id = {0} AND NOT (" + idCylName + " = {1} and compid = {2} and target = {3} " +
                                               "and c = {4} and dcabs = {5} and dcrel = {6})",
                item.Id, item.IdCyl, item.IdGas, item.Target,
                PGS.DB.ToDBString(item.Conc), PGS.DB.ToDBString(item.DCabs), PGS.DB.ToDBString(item.DCrel));

            whereString = NullConverter(whereString);
            
            string updateCommand = string.Format(
                "UPDATE " + tableName + " " +
                "SET id = {0}, " + idCylName + " = {1}, compid = {2}, target = {3}, c = {4}, dcabs = {5}, dcrel = {6} " +
                whereString,
                item.Id, item.IdCyl, item.IdGas, item.Target,
                PGS.DB.ToDBString(item.Conc), PGS.DB.ToDBString(item.DCabs), PGS.DB.ToDBString(item.DCrel));
            command.CommandText = updateCommand;

            try
            {
                if (command.ExecuteNonQuery() == 0)
                {
                    return RequestType.WithoutChanges;
                }

                return RequestType.Update;
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
                return RequestType.WithoutChanges;
            }
        }

        /// <summary>
        /// Производит проверку на наличие элемента, вставляет его в случае отсутствия
        /// </summary>
        /// <param name="command"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public RequestType InsertSpending(OdbcCommand command, MsAccessHandler.Spending item)
        {
            //Проверка на наличие
            command.CommandText = string.Format("SELECT * FROM raw_materials.spendings WHERE id = {0}", item.Id);
            if (command.ExecuteNonQuery() > 0) return RequestType.WithoutChanges;
            
            //Добавление в таблицу
            command.CommandText = string.Format(
                "INSERT INTO raw_materials.spendings(id, rawid, date, optype, quantity, userid, comment) " +
                "VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                item.Id, item.MixId,
                PGS.DB.ToDBDate(item.Date),
                item.OpType,
                PGS.DB.ToDBString(item.Quantity),
                item.UserId,
                PGS.DB.ToDBString(item.Order));
                
            try
            {
                command.ExecuteNonQuery();
                return RequestType.Insert;
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
                return RequestType.WithoutChanges;
            }
        }

        /// <summary>
        /// Возвращает ID записи или null если элемент primMixGas с пустым NumCyl уже добавлен
        /// </summary>
        /// <param name="command"></param>
        /// <param name="item"></param>
        /// <param name="cylsCounter"></param>
        /// <returns></returns>
        public int? InsertCyls(OdbcCommand command, MsAccessHandler.PrimMixGas item, Counter cylsCounter)
        {
            var insertCommand = string.Format(
                "INSERT INTO cyls.cyls(cyltype, valvetype, cylnum, volume, colorid) " +
                "VALUES ({0}, {1}, {2}, {3}, {4}) RETURNING id",
                item.TypCyl, PGS.DB.ToDBString(item.TypValve),
                PGS.DB.ToDBString(item.NumCyl), PGS.DB.ToDBString(item.VolCyl), 1);
            
            //Если NumCyl пустой и элемент primMixGas ещё не был добавлен, то создаёт записи и возвращает её id
            if (string.IsNullOrEmpty(item.NumCyl))
            {
                // Если запись о баллоне ещё не была перенесена
                // Если она уже есть => и запись в cyls есть
                if (!CheckPrimMixGas(command, item.TypMix, item.Id))
                {
                    command.CommandText = insertCommand;
                    try
                    {
                        var id = (int) command.ExecuteScalar();
                        cylsCounter.UpdateCounter(RequestType.Insert);
                        return id;
                    }
                    catch (Exception e)
                    {
                        Program.ErrorMessage(e);
                    }
                }
                else
                {
                    return null;
                }
            }
            
            //Если запись с непустым NumCyl уже есть, то выводим её id
            var selectCommand = string.Format(
                "SELECT id FROM cyls.cyls WHERE cyltype = {0} AND valvetype = {1} AND cylnum = {2} AND volume = {3}",
                PGS.DB.ToDBString(item.TypCyl), PGS.DB.ToDBString(item.TypValve),
                PGS.DB.ToDBString(item.NumCyl), PGS.DB.ToDBString(item.VolCyl));

            selectCommand = NullConverter(selectCommand);

            command.CommandText = selectCommand;

            try
            {
                var id = command.ExecuteScalar() as int?;
                if (id != null)
                {
                    cylsCounter.UpdateCounter(RequestType.WithoutChanges);
                    return (int) id;
                }
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
            }

            //Добавляем запись
            command.CommandText = insertCommand;

            try
            {
                var id = (int) command.ExecuteScalar();
                cylsCounter.UpdateCounter(RequestType.Insert);
                return id;
            }
            catch (Exception e)
            {
                Program.ErrorMessage(e);
                throw;
            }
        }
    }
}