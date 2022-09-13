using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace fillComplectation.Properties
{
    public class PostgreSqlHandler
    {
        public void InsertComplectationInDb(List<ExcelHandler.Complectation> complectations)
        {
            using (var dbConnection = PGS.DB.Connect())
            {
                OdbcTransaction transaction = null;
                try
                {
                    transaction = dbConnection.BeginTransaction();

                    OdbcCommand command = new OdbcCommand();
                    command.Connection = dbConnection;
                    command.Connection = dbConnection;
                    command.Transaction = transaction;                 

                    Console.WriteLine("Добавлено:");
                    var insertedCounter = 0;
                    
                    //Команды на выполнение
                    foreach (var complectation in complectations)
                    {
                        command.CommandText =
                            "INSERT INTO cyls.complectations_1c(kod_1c, name_1c, cyltype, valvetype, arhive) " +
                            $"VALUES ({PGS.DB.ToDBString(complectation.kod)}, " +
                            $"{PGS.DB.ToDBString(complectation.name)}, " +
                            $"{PGS.DB.ToDBString(complectation.cylType)}, " +
                            $"{PGS.DB.ToDBString(complectation.valveType)}, " +
                            "true)";

                        try
                        {
                            command.ExecuteNonQuery();
                            Console.WriteLine($"    {complectation.ToString()}");
                            insertedCounter++;
                        }
                        catch (OdbcException ex)
                        {
                            throw ex;
                        }
                    }

                    transaction.Commit();
                    Console.WriteLine($"Всего добавлено: {insertedCounter}");
                }
                catch (Exception ex)
                {
                    //Заменить на сообщение в окне
                    Console.WriteLine("Комплектации не добавлены");
                    
                    //Откат транзакции
                    try
                    {
                        if (transaction != null) transaction.Rollback();
                    }
                    catch
                    {
                        
                    }

                    throw ex;
                }
            }
        }
    }
}