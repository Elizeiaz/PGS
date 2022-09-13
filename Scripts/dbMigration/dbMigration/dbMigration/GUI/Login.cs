using PGS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dbMigration
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        //Вход
        private void button1_Click(object sender, EventArgs e)
        {
            //Логин
            if (PGS.DB.doLogin() == DialogResult.Cancel) this.Close();
            
            Form connectToDBForm = new ConnectToDB();
            connectToDBForm.Show(this);
            Hide();
        }
    }
}
