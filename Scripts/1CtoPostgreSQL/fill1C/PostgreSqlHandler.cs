using System;
using System.Collections.Generic;
using System.Data.Odbc;

namespace fill1C
{
    public class PostgreSqlHandler
    {
        public void InsertComplectationInDb(Dictionary<double, double> comps)
        {
            using (var dbConnection = PGS.DB.Connect())
            {
                OdbcTransaction transaction = null;
                try
                {
                    transaction = dbConnection.BeginTransaction();

                    OdbcCommand command = new OdbcCommand();
                    command.Connection = dbConnection;
                    command.Transaction = transaction;

                    Console.WriteLine("Добавлено:");
                    var insertedCounter = 0;

                    //Команды на выполнение
                    foreach (var comp in comps)
                    {
                        command.CommandText = string.Format(
                            "UPDATE comps.comps SET kod_1c = {0} WHERE id = {1} AND NOT (kod_1c = {0})",
                            PGS.DB.ToDBString(comp.Value),
                            PGS.DB.ToDBString(comp.Key));

                        try
                        {
                            if (command.ExecuteNonQuery() == 1)
                            {
                                Console.WriteLine($"    {comp.Key}: {comp.Value}");
                                insertedCounter++;
                            }
                            else
                            {
                                Console.WriteLine($"    Компонент {comp.Key}: {comp.Value} уже есть");
                            }
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