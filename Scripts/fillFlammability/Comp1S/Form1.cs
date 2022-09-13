using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Comp1S
{
    public partial class Form1 : Form
    {
        private string pathToExcel = Environment.CurrentDirectory + "\\Горючесть.xls";


        /// <summary>
        /// Возвращает лист добавленных компонентов 
        /// </summary>
        /// <param name="components"></param>
        private List<ExcelHandler.MyComponent> InsertCompanentsInDB(List<ExcelHandler.MyComponent> components)
        {
            var insertedComponents = new List<ExcelHandler.MyComponent>();

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

                    //Команды на выполнение
                    foreach (ExcelHandler.MyComponent component in components)
                    {
                        command.CommandText = string.Format(
                        "INSERT INTO comps.flammability(compid, nkpr_air, vkpr_air, max_conc_air) VALUES ({0}, {1}, {2}, {3});",
                        component.compopnentID,
                        PGS.DB.ToDBString(component.nkprAir),
                        PGS.DB.ToDBString(component.vkprAir),
                        PGS.DB.ToDBString(component.maxConcAir)
                        );

                        try
                        {
                            command.ExecuteNonQuery();
                            insertedComponents.Add(component);
                        }
                        catch (OdbcException ex)
                        {
                            continue;
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    //Заменить на сообщение в окне
                    Console.WriteLine(ex.StackTrace);

                    //Откат транзакции
                    try
                    {   
                        if (transaction != null) transaction.Rollback();
                    }
                    catch { }
                }
            }

            return insertedComponents;
        }


        public Form1()
        {
            InitializeComponent();
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "Перенести значения?", "Диалоговое окно",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                this.Close();
            }
        }   

        private void Form1_Shown(object sender, EventArgs e)
        {
            var components = ExcelHandler.ParseComponents(pathToExcel);

            //Добавляю ID к элементу
            foreach (ExcelHandler.MyComponent component in components)
            {
                component.compopnentID = PGS.DB.FindCompInfoBase(component.componentName, false).ID;
                //Console.WriteLine(component);
            }

            //Работа с бд
            List<ExcelHandler.MyComponent> insertedComponents = InsertCompanentsInDB(components);

            if (insertedComponents.Count > 0)
            {
                this.label1.Text = string.Format("Новых компонентов: {0}", insertedComponents.Count);
            }
        }
    }
}
