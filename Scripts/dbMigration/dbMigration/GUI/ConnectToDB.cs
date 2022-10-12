using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using dbMigration.GUI;
using PGS;
using PGS.Mixes;
using PGS.UI.Connection;

namespace dbMigration
{
    public partial class ConnectToDB : Form
    {
        //Криво
        GUI.ConnectToMsAccess connectToMsAccess = new GUI.ConnectToMsAccess();
        //Объявляется в форме connectToMsAccess в методе ValidConnections
        private MsAccessHandler _msAccessHandler;
        
        
        public ConnectToDB()
        {
            InitializeComponent();
            
            //Автоподключение базы Mdb, если она лежит в корне и без пароля (debug)
            if (File.Exists(Environment.CurrentDirectory + @"\WMBD.mdb"))
            {
                try
                {
                    _msAccessHandler = new MsAccessHandler(Environment.CurrentDirectory + @"\WMBD.mdb");
                }
                // Глушу ошибку если не удастся подключится автоматически, например если бд имеет пароль
                catch (Exception e)
                {
                }
            }
        }

        //Подключение MsAccess
        private void button1_Click(object sender, EventArgs e)
        {
            connectToMsAccess.Show();
        }

        private void ConnectToDB_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        
        // Кнопка Перенести данные. Заполнение таблиц
        private void button2_Click(object sender, EventArgs e)
        {
            if (!ValidConnections()) return;
            
            //Соединения
            var pgHandler = new PostgreSQLHandler();;
            var pgCommand = pgHandler.GetCommand(pgHandler.GetConnection());
            var msCon = _msAccessHandler.GetConnection();
            
            //Счётчики
            var primMixGasRawsCounter = new PostgreSQLHandler.Counter();
            var primMixGasMixesCounter = new PostgreSQLHandler.Counter();
            var cylsCounter = new PostgreSQLHandler.Counter();
            var gasAdmRawsCounter = new PostgreSQLHandler.Counter();
            var gasAdmMixesCounter = new PostgreSQLHandler.Counter();
            var spendingCounter = new PostgreSQLHandler.Counter();

            //Заполнение таблиц cyls и raws/mixes происходит в первую очередь, т.к. от них идут зависимости
            foreach (var primMixGas in _msAccessHandler.ReadPrimMixGasTable(msCon))
            {
                primMixGas.PrimMixGasValidator();
                
                if (primMixGas.TypMix == (int) MsAccessHandler.MixType.Raw && checkBox1.Checked)
                {
                    // cylId = null, если элемент с полем NumCyl = null уже был добавлен 
                    var cylId = pgHandler.InsertCyls(pgCommand, primMixGas, cylsCounter);
                    if (cylId != null)
                    {
                        primMixGasRawsCounter.UpdateCounter(pgHandler.InsertPrimMixGas(pgCommand, primMixGas, (int) cylId));
                    }
                }
                else if (primMixGas.TypMix == (int) MsAccessHandler.MixType.Mix && checkBox2.Checked)
                {
                    var cylId = pgHandler.InsertCyls(pgCommand, primMixGas, cylsCounter);
                    if (cylId != null)
                    {
                        primMixGasMixesCounter.UpdateCounter(pgHandler.InsertPrimMixGas(pgCommand, primMixGas, (int) cylId));
                    }
                }
            }
            
            //Заполнение raw_comps/mix_comps
            foreach (var comp in _msAccessHandler.ReadGasAdmTable(msCon))
            {
                if (checkBox1.Checked && (MsAccessHandler.MixType) comp.MixType == MsAccessHandler.MixType.Raw)
                {
                    gasAdmRawsCounter.UpdateCounter(pgHandler.InsertGasAdm(pgCommand, comp, MsAccessHandler.MixType.Raw));
                } else if (checkBox2.Checked && (MsAccessHandler.MixType) comp.MixType == MsAccessHandler.MixType.Mix)
                {
                    gasAdmMixesCounter.UpdateCounter(pgHandler.InsertGasAdm(pgCommand, comp, MsAccessHandler.MixType.Mix));
                }
            }
            
            //Заполнение spending
            if (checkBox1.Checked)
            {
                foreach (var spending in _msAccessHandler.ReadSpendingTable(msCon))
                {
                    spendingCounter.UpdateCounter(pgHandler.InsertSpending(pgCommand, spending));
                }
            }

            //Окно счётчика
            var counterForm = new FormShowChanges(primMixGasRawsCounter, primMixGasMixesCounter, cylsCounter,
                gasAdmRawsCounter, gasAdmMixesCounter, spendingCounter);
            counterForm.Show();
        }

        // Подключение PostgreSQL
        private void button3_Click(object sender, EventArgs e)
        {
            var fc = new FormConnectionSetting();
            fc.ShowDialog();
        }

        /// <summary>
        /// Проверяет корректность соединений к MsAccess и PostgreSQL
        /// </summary>
        /// <returns></returns>
        private bool ValidConnections()
        {
            bool isMsAccessConnected;
            
            var pg = new PostgreSQLHandler();
            var isPgConnected = pg.CheckConnection();
            
            // Если не укажем файл .mdb, то не создастся экземпляр класса MsAccessHandler =>
            // Подключение не установлено
            try
            {
                if (_msAccessHandler is null)
                {
                    _msAccessHandler = connectToMsAccess.getMSAHandler();
                    isMsAccessConnected = _msAccessHandler.CheckConnection();
                }
                else
                {
                    isMsAccessConnected = _msAccessHandler.CheckConnection();
                }
            }
            catch
            {
                isMsAccessConnected = false;
            }

            if (isPgConnected && isMsAccessConnected)
            {
                return true;
            }

            if (!isPgConnected)
            {
                MessageBox.Show(
                    "Не удалось установить соединение с PostgreSQL.\nПроверьте настройки подключения",
                    "Ошибка соединения", MessageBoxButtons.OK);
            }

            if (!isMsAccessConnected)
            {
                MessageBox.Show(
                    "Не удалось установить соединение с MsAccess.\nПроверьте настройки подключения",
                    "Ошибка соединения", MessageBoxButtons.OK);
            }
            
            return false;
        }
    }
}
