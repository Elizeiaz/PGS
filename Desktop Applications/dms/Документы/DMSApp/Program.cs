using System;
using System.Windows.Forms;
using PGS.DMS.UI;


namespace DMS
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormDmsBrowser());
        }
    }
}
