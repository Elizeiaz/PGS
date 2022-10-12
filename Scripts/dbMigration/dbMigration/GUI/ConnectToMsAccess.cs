using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbMigration.GUI
{
    public partial class ConnectToMsAccess : Form
    {
        private MsAccessHandler MsAccessHandler;
        private string _filePath;

        public ConnectToMsAccess()
        {
            InitializeComponent();
        }


        //Выбор .mdb файла
        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = @"C:\Users\lab.PGS-SERVICE12\Desktop\Работа";
                openFileDialog.Filter = "mdb files (*.mdb)|*.mdb";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = openFileDialog.FileName;
                }
            }
        }

        //Соединение
        private void button2_Click(object sender, EventArgs e)
        {
            if (this._filePath == null)
            {
                this.button1.BackColor = Color.LightCoral;
            }
            else
            {
                this.button1.BackColor = Color.WhiteSmoke;

                //База с паролем/без
                MsAccessHandler = this.textBox1.Text == "" ? new MsAccessHandler(_filePath)
                    : new MsAccessHandler(_filePath, this.textBox1.Text);

                if (MsAccessHandler.CheckConnection())
                {
                    MessageBox.Show("Подключение успешно установлено", "", MessageBoxButtons.OK);
                    Close();
                }
                else
                {
                    MessageBox.Show("Подключение не установлено", "", MessageBoxButtons.OK);
                }
            }    
        }

        private void ConnectToMsAccess_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public MsAccessHandler getMSAHandler()
        {
            return this.MsAccessHandler;
        }
    }
}
