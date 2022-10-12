using System;
using System.Windows.Forms;

namespace dbMigration.GUI
{
    public partial class FormShowChanges : Form
    {
        private PostgreSQLHandler.Counter primMixGasRawsCounter;
        private PostgreSQLHandler.Counter primMixGasMixesCounter;
        private PostgreSQLHandler.Counter cylsCounter;
        private PostgreSQLHandler.Counter gasAdmRawsCounter;
        private PostgreSQLHandler.Counter gasAdmMixesCounter;
        private PostgreSQLHandler.Counter spendingCounter;
        
        public FormShowChanges(
            PostgreSQLHandler.Counter primMixGasRawsCounter,
            PostgreSQLHandler.Counter primMixGasMixesCounter,
            PostgreSQLHandler.Counter cylsCounter,
            PostgreSQLHandler.Counter gasAdmRawsCounter,
            PostgreSQLHandler.Counter gasAdmMixesCounter,
            PostgreSQLHandler.Counter spendingCounter)
        {
            InitializeComponent();
            this.primMixGasRawsCounter = primMixGasRawsCounter;
            this.primMixGasMixesCounter = primMixGasMixesCounter;
            this.cylsCounter = cylsCounter;
            this.gasAdmRawsCounter = gasAdmRawsCounter;
            this.gasAdmMixesCounter = gasAdmMixesCounter;
            this.spendingCounter = spendingCounter;
        }

        private void ShowChanges_Load(object sender, EventArgs e)
        {
            this.primMixAdd.Text = string.Format("Добавлено: {0}", primMixGasMixesCounter.Inserted);
            this.primMixChange.Text = string.Format("Изменено: {0}", primMixGasMixesCounter.Updated);
            
            this.primRawAdd.Text = string.Format("Добавлено: {0}", primMixGasRawsCounter.Inserted);
            this.primRawChange.Text = string.Format("Изменено: {0}", primMixGasRawsCounter.Updated);
            
            this.gasMixAdd.Text = string.Format("Добавлено: {0}", gasAdmMixesCounter.Inserted);
            this.gasMixChange.Text = string.Format("Изменено: {0}", gasAdmMixesCounter.Updated);
            
            this.gasRawAdd.Text = string.Format("Добавлено: {0}", gasAdmRawsCounter.Inserted);
            this.gasRawChange.Text = string.Format("Изменено: {0}", gasAdmRawsCounter.Updated);
            
            this.cylAdd.Text = string.Format("Добавлено: {0}", cylsCounter.Inserted);
            
            this.spendingAdd.Text = string.Format("Добавлено: {0}", spendingCounter.Inserted);
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }
    }
}