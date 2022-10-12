using System.Windows.Forms;

namespace PGS.UI.Comps
{
    partial class FormCompsList
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle52 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle32 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle33 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle34 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle35 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle36 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle37 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle38 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle39 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle40 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle41 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle42 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle43 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle44 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle45 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle46 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle47 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle48 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle49 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle50 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle51 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompsList));
            this.dgv = new System.Windows.Forms.DataGridView();
            this.dgv_column_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_NameAlt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Isomer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgv_column_Formula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_toxic = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgv_column_flammability = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgv_column_MolarMass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_dMolarMass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_MassDensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Pss243k = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Pss253k = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Pss263k = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Pss273k = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Pss283k = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Pss293k = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_Kflow = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_toxicity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_units = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_max_conc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_nkpr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_vkpr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_tkip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_tkr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_column_pkr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelGroup = new System.Windows.Forms.ToolStripLabel();
            this.cbGroup = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelSearch = new System.Windows.Forms.ToolStripLabel();
            this.tbFind = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelShow = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.BtnAdd = new System.Windows.Forms.ToolStripButton();
            this.BtnEdit = new System.Windows.Forms.ToolStripButton();
            this.BtnRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsDropDownBtnFilter = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItemAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemInRow = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemInReferences = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnReGroup = new System.Windows.Forms.ToolStripButton();
            this.BtnFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_molar = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_density = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_rnp = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_kflow = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_toxicity = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_flammability = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_critical = new System.Windows.Forms.ToolStripButton();
            this.BtnShowLog = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle27.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle27.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle27.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle27.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle27.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle27.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle27;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_column_id,
            this.dgv_column_Name,
            this.dgv_column_NameAlt,
            this.dgv_column_Isomer,
            this.dgv_column_Formula,
            this.dgv_column_toxic,
            this.dgv_column_flammability,
            this.dgv_column_MolarMass,
            this.dgv_column_dMolarMass,
            this.dgv_column_MassDensity,
            this.dgv_column_Pss243k,
            this.dgv_column_Pss253k,
            this.dgv_column_Pss263k,
            this.dgv_column_Pss273k,
            this.dgv_column_Pss283k,
            this.dgv_column_Pss293k,
            this.dgv_column_Kflow,
            this.dgv_column_toxicity,
            this.dgv_column_units,
            this.dgv_column_max_conc,
            this.dgv_column_nkpr,
            this.dgv_column_vkpr,
            this.dgv_column_tkip,
            this.dgv_column_tkr,
            this.dgv_column_pkr});
            dataGridViewCellStyle52.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle52.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle52.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle52.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle52.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle52.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle52.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.DefaultCellStyle = dataGridViewCellStyle52;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 25);
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.Size = new System.Drawing.Size(1069, 501);
            this.dgv.TabIndex = 1;
            this.dgv.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgv_CellFormatting);
            this.dgv.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_CellDoubleClick);
            this.dgv.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dgv_PreviewKeyDown);
            // 
            // dgv_column_id
            // 
            this.dgv_column_id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dgv_column_id.DataPropertyName = "id";
            this.dgv_column_id.HeaderText = "id";
            this.dgv_column_id.Name = "dgv_column_id";
            this.dgv_column_id.ReadOnly = true;
            this.dgv_column_id.Visible = false;
            // 
            // dgv_column_Name
            // 
            this.dgv_column_Name.DataPropertyName = "Name";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgv_column_Name.DefaultCellStyle = dataGridViewCellStyle28;
            this.dgv_column_Name.HeaderText = "Название";
            this.dgv_column_Name.Name = "dgv_column_Name";
            this.dgv_column_Name.ReadOnly = true;
            this.dgv_column_Name.Width = 120;
            // 
            // dgv_column_NameAlt
            // 
            this.dgv_column_NameAlt.DataPropertyName = "NameAlt";
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgv_column_NameAlt.DefaultCellStyle = dataGridViewCellStyle29;
            this.dgv_column_NameAlt.HeaderText = "Название2";
            this.dgv_column_NameAlt.Name = "dgv_column_NameAlt";
            this.dgv_column_NameAlt.ReadOnly = true;
            this.dgv_column_NameAlt.Width = 120;
            // 
            // dgv_column_Isomer
            // 
            this.dgv_column_Isomer.DataPropertyName = "Isomer";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle30.NullValue = false;
            this.dgv_column_Isomer.DefaultCellStyle = dataGridViewCellStyle30;
            this.dgv_column_Isomer.HeaderText = "Изомер";
            this.dgv_column_Isomer.Name = "dgv_column_Isomer";
            this.dgv_column_Isomer.ReadOnly = true;
            this.dgv_column_Isomer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgv_column_Isomer.Width = 25;
            // 
            // dgv_column_Formula
            // 
            this.dgv_column_Formula.DataPropertyName = "Formula";
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Formula.DefaultCellStyle = dataGridViewCellStyle31;
            this.dgv_column_Formula.HeaderText = "Формула";
            this.dgv_column_Formula.Name = "dgv_column_Formula";
            this.dgv_column_Formula.ReadOnly = true;
            this.dgv_column_Formula.Width = 90;
            // 
            // dgv_column_toxic
            // 
            this.dgv_column_toxic.DataPropertyName = "toxic";
            dataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle32.NullValue = false;
            this.dgv_column_toxic.DefaultCellStyle = dataGridViewCellStyle32;
            this.dgv_column_toxic.HeaderText = "Токсичен";
            this.dgv_column_toxic.Name = "dgv_column_toxic";
            this.dgv_column_toxic.ReadOnly = true;
            this.dgv_column_toxic.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_column_toxic.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgv_column_toxic.Width = 25;
            // 
            // dgv_column_flammability
            // 
            this.dgv_column_flammability.DataPropertyName = "inflammable";
            dataGridViewCellStyle33.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle33.NullValue = false;
            this.dgv_column_flammability.DefaultCellStyle = dataGridViewCellStyle33;
            this.dgv_column_flammability.HeaderText = "Воспламеняем";
            this.dgv_column_flammability.Name = "dgv_column_flammability";
            this.dgv_column_flammability.ReadOnly = true;
            this.dgv_column_flammability.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_column_flammability.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgv_column_flammability.Width = 25;
            // 
            // dgv_column_MolarMass
            // 
            this.dgv_column_MolarMass.DataPropertyName = "MolarMass";
            dataGridViewCellStyle34.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_MolarMass.DefaultCellStyle = dataGridViewCellStyle34;
            this.dgv_column_MolarMass.HeaderText = "Мол. масса, г/моль";
            this.dgv_column_MolarMass.Name = "dgv_column_MolarMass";
            this.dgv_column_MolarMass.ReadOnly = true;
            // 
            // dgv_column_dMolarMass
            // 
            this.dgv_column_dMolarMass.DataPropertyName = "dMolarMass";
            dataGridViewCellStyle35.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_dMolarMass.DefaultCellStyle = dataGridViewCellStyle35;
            this.dgv_column_dMolarMass.HeaderText = "± г/моль";
            this.dgv_column_dMolarMass.Name = "dgv_column_dMolarMass";
            this.dgv_column_dMolarMass.ReadOnly = true;
            this.dgv_column_dMolarMass.Width = 80;
            // 
            // dgv_column_MassDensity
            // 
            this.dgv_column_MassDensity.DataPropertyName = "MassDensity";
            dataGridViewCellStyle36.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_MassDensity.DefaultCellStyle = dataGridViewCellStyle36;
            this.dgv_column_MassDensity.HeaderText = "Плотность, г/см³";
            this.dgv_column_MassDensity.Name = "dgv_column_MassDensity";
            this.dgv_column_MassDensity.ReadOnly = true;
            this.dgv_column_MassDensity.Width = 75;
            // 
            // dgv_column_Pss243k
            // 
            this.dgv_column_Pss243k.DataPropertyName = "Pss243k";
            dataGridViewCellStyle37.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Pss243k.DefaultCellStyle = dataGridViewCellStyle37;
            this.dgv_column_Pss243k.HeaderText = "Pнп (-30°C)";
            this.dgv_column_Pss243k.Name = "dgv_column_Pss243k";
            this.dgv_column_Pss243k.ReadOnly = true;
            this.dgv_column_Pss243k.ToolTipText = "Давление насыщеных паров, ат";
            this.dgv_column_Pss243k.Width = 60;
            // 
            // dgv_column_Pss253k
            // 
            this.dgv_column_Pss253k.DataPropertyName = "Pss253k";
            dataGridViewCellStyle38.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Pss253k.DefaultCellStyle = dataGridViewCellStyle38;
            this.dgv_column_Pss253k.HeaderText = "Pнп (-20°C)";
            this.dgv_column_Pss253k.Name = "dgv_column_Pss253k";
            this.dgv_column_Pss253k.ReadOnly = true;
            this.dgv_column_Pss253k.ToolTipText = "Давление насыщеных паров, ат";
            this.dgv_column_Pss253k.Width = 60;
            // 
            // dgv_column_Pss263k
            // 
            this.dgv_column_Pss263k.DataPropertyName = "Pss263k";
            dataGridViewCellStyle39.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Pss263k.DefaultCellStyle = dataGridViewCellStyle39;
            this.dgv_column_Pss263k.HeaderText = "Pнп (-10°C)";
            this.dgv_column_Pss263k.Name = "dgv_column_Pss263k";
            this.dgv_column_Pss263k.ReadOnly = true;
            this.dgv_column_Pss263k.ToolTipText = "Давление насыщеных паров, ат";
            this.dgv_column_Pss263k.Width = 60;
            // 
            // dgv_column_Pss273k
            // 
            this.dgv_column_Pss273k.DataPropertyName = "Pss273k";
            dataGridViewCellStyle40.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Pss273k.DefaultCellStyle = dataGridViewCellStyle40;
            this.dgv_column_Pss273k.HeaderText = "Pнп (0°C)";
            this.dgv_column_Pss273k.Name = "dgv_column_Pss273k";
            this.dgv_column_Pss273k.ReadOnly = true;
            this.dgv_column_Pss273k.ToolTipText = "Давление насыщеных паров, ат";
            this.dgv_column_Pss273k.Width = 60;
            // 
            // dgv_column_Pss283k
            // 
            this.dgv_column_Pss283k.DataPropertyName = "Pss283k";
            dataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Pss283k.DefaultCellStyle = dataGridViewCellStyle41;
            this.dgv_column_Pss283k.HeaderText = "Pнп (10°C)";
            this.dgv_column_Pss283k.Name = "dgv_column_Pss283k";
            this.dgv_column_Pss283k.ReadOnly = true;
            this.dgv_column_Pss283k.ToolTipText = "Давление насыщеных паров, ат";
            this.dgv_column_Pss283k.Width = 60;
            // 
            // dgv_column_Pss293k
            // 
            this.dgv_column_Pss293k.DataPropertyName = "Pss293k";
            dataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Pss293k.DefaultCellStyle = dataGridViewCellStyle42;
            this.dgv_column_Pss293k.HeaderText = "Pнп (20°C)";
            this.dgv_column_Pss293k.Name = "dgv_column_Pss293k";
            this.dgv_column_Pss293k.ReadOnly = true;
            this.dgv_column_Pss293k.ToolTipText = "Давление насыщеных паров, ат";
            this.dgv_column_Pss293k.Width = 60;
            // 
            // dgv_column_Kflow
            // 
            this.dgv_column_Kflow.DataPropertyName = "K";
            dataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_Kflow.DefaultCellStyle = dataGridViewCellStyle43;
            this.dgv_column_Kflow.HeaderText = "Kрасх.";
            this.dgv_column_Kflow.Name = "dgv_column_Kflow";
            this.dgv_column_Kflow.ReadOnly = true;
            this.dgv_column_Kflow.ToolTipText = "Коэффициент расхода";
            this.dgv_column_Kflow.Width = 65;
            // 
            // dgv_column_toxicity
            // 
            this.dgv_column_toxicity.DataPropertyName = "pdk";
            dataGridViewCellStyle44.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_toxicity.DefaultCellStyle = dataGridViewCellStyle44;
            this.dgv_column_toxicity.HeaderText = "ПДК";
            this.dgv_column_toxicity.Name = "dgv_column_toxicity";
            this.dgv_column_toxicity.ReadOnly = true;
            this.dgv_column_toxicity.ToolTipText = "Предельно допустимая концентрация";
            this.dgv_column_toxicity.Width = 60;
            // 
            // dgv_column_units
            // 
            this.dgv_column_units.DataPropertyName = "units_name";
            dataGridViewCellStyle45.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgv_column_units.DefaultCellStyle = dataGridViewCellStyle45;
            this.dgv_column_units.HeaderText = "ед. изм.";
            this.dgv_column_units.Name = "dgv_column_units";
            this.dgv_column_units.ReadOnly = true;
            this.dgv_column_units.ToolTipText = "Единицы измерения";
            this.dgv_column_units.Width = 50;
            // 
            // dgv_column_max_conc
            // 
            this.dgv_column_max_conc.DataPropertyName = "max_conc_air";
            dataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_max_conc.DefaultCellStyle = dataGridViewCellStyle46;
            this.dgv_column_max_conc.HeaderText = "макс. C";
            this.dgv_column_max_conc.Name = "dgv_column_max_conc";
            this.dgv_column_max_conc.ReadOnly = true;
            this.dgv_column_max_conc.ToolTipText = "Максимальная концентрация (в воздухе)";
            this.dgv_column_max_conc.Width = 95;
            // 
            // dgv_column_nkpr
            // 
            this.dgv_column_nkpr.DataPropertyName = "nkpr_air";
            dataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_nkpr.DefaultCellStyle = dataGridViewCellStyle47;
            this.dgv_column_nkpr.HeaderText = "НКПР, мол. %";
            this.dgv_column_nkpr.Name = "dgv_column_nkpr";
            this.dgv_column_nkpr.ReadOnly = true;
            this.dgv_column_nkpr.ToolTipText = "Нижний концентрационный предел распространения в воздухе";
            this.dgv_column_nkpr.Width = 75;
            // 
            // dgv_column_vkpr
            // 
            this.dgv_column_vkpr.DataPropertyName = "vkpr_air";
            dataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_vkpr.DefaultCellStyle = dataGridViewCellStyle48;
            this.dgv_column_vkpr.HeaderText = "ВКПР, мол. %";
            this.dgv_column_vkpr.Name = "dgv_column_vkpr";
            this.dgv_column_vkpr.ReadOnly = true;
            this.dgv_column_vkpr.ToolTipText = "Верхний концентрационный предел распространения в воздухе";
            this.dgv_column_vkpr.Width = 75;
            // 
            // dgv_column_tkip
            // 
            this.dgv_column_tkip.DataPropertyName = "tkip";
            dataGridViewCellStyle49.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_tkip.DefaultCellStyle = dataGridViewCellStyle49;
            this.dgv_column_tkip.HeaderText = "Ткип, К";
            this.dgv_column_tkip.Name = "dgv_column_tkip";
            this.dgv_column_tkip.ReadOnly = true;
            this.dgv_column_tkip.ToolTipText = "Температура кипения";
            // 
            // dgv_column_tkr
            // 
            this.dgv_column_tkr.DataPropertyName = "tkr";
            dataGridViewCellStyle50.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_tkr.DefaultCellStyle = dataGridViewCellStyle50;
            this.dgv_column_tkr.HeaderText = "Ткр, К";
            this.dgv_column_tkr.Name = "dgv_column_tkr";
            this.dgv_column_tkr.ReadOnly = true;
            this.dgv_column_tkr.ToolTipText = "Температура критическая";
            // 
            // dgv_column_pkr
            // 
            this.dgv_column_pkr.DataPropertyName = "pkr";
            dataGridViewCellStyle51.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_column_pkr.DefaultCellStyle = dataGridViewCellStyle51;
            this.dgv_column_pkr.HeaderText = "Pкр, МПа";
            this.dgv_column_pkr.Name = "dgv_column_pkr";
            this.dgv_column_pkr.ReadOnly = true;
            this.dgv_column_pkr.ToolTipText = "Критическая плотность";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelGroup
            // 
            this.toolStripLabelGroup.Name = "toolStripLabelGroup";
            this.toolStripLabelGroup.Size = new System.Drawing.Size(46, 22);
            this.toolStripLabelGroup.Text = "Группа";
            // 
            // cbGroup
            // 
            this.cbGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGroup.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.Size = new System.Drawing.Size(121, 25);
            this.cbGroup.SelectedIndexChanged += new System.EventHandler(this.cbGroup_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelSearch
            // 
            this.toolStripLabelSearch.Name = "toolStripLabelSearch";
            this.toolStripLabelSearch.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabelSearch.Text = "Поиск";
            // 
            // tbFind
            // 
            this.tbFind.AutoSize = false;
            this.tbFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbFind.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbFind.Margin = new System.Windows.Forms.Padding(1, 2, 1, 0);
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(100, 23);
            this.tbFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbFind_KeyPress);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabelShow
            // 
            this.toolStripLabelShow.Name = "toolStripLabelShow";
            this.toolStripLabelShow.Size = new System.Drawing.Size(60, 22);
            this.toolStripLabelShow.Text = "Показать:";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnAdd,
            this.BtnEdit,
            this.BtnRemove,
            this.toolStripSeparator6,
            this.tsDropDownBtnFilter,
            this.toolStripSeparator1,
            this.toolStripLabelGroup,
            this.cbGroup,
            this.BtnReGroup,
            this.toolStripSeparator2,
            this.toolStripLabelSearch,
            this.tbFind,
            this.BtnFind,
            this.toolStripSeparator3,
            this.toolStripSeparator5,
            this.toolStripLabelShow,
            this.toolStripButton_molar,
            this.toolStripButton_density,
            this.toolStripButton_rnp,
            this.toolStripButton_kflow,
            this.toolStripButton_toxicity,
            this.toolStripButton_flammability,
            this.toolStripButton_critical,
            this.toolStripSeparator4,
            this.BtnShowLog});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1069, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // BtnAdd
            // 
            this.BtnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnAdd.Image = global::PGS.Common.Resources.add;
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(23, 22);
            this.BtnAdd.Text = "Добавить компонент";
            this.BtnAdd.Visible = false;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnEdit
            // 
            this.BtnEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnEdit.Image = global::PGS.Common.Resources.edit;
            this.BtnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnEdit.Name = "BtnEdit";
            this.BtnEdit.Size = new System.Drawing.Size(23, 22);
            this.BtnEdit.Text = "Просмотр";
            this.BtnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // BtnRemove
            // 
            this.BtnRemove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnRemove.Image = global::PGS.Common.Resources.remove;
            this.BtnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(23, 22);
            this.BtnRemove.Text = "Удалить компонент";
            this.BtnRemove.Visible = false;
            this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsDropDownBtnFilter
            // 
            this.tsDropDownBtnFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAll,
            this.toolStripSeparator7,
            this.toolStripMenuItemInRow,
            this.toolStripMenuItemInReferences});
            this.tsDropDownBtnFilter.Image = global::PGS.Common.Resources.view_filter;
            this.tsDropDownBtnFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsDropDownBtnFilter.Name = "tsDropDownBtnFilter";
            this.tsDropDownBtnFilter.Size = new System.Drawing.Size(86, 22);
            this.tsDropDownBtnFilter.Text = "Показать";
            this.tsDropDownBtnFilter.ToolTipText = "Показать по наличию везде/в сырье/в весовых";
            // 
            // toolStripMenuItemAll
            // 
            this.toolStripMenuItemAll.Checked = true;
            this.toolStripMenuItemAll.CheckOnClick = true;
            this.toolStripMenuItemAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemAll.Name = "toolStripMenuItemAll";
            this.toolStripMenuItemAll.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuItemAll.Text = "Все";
            this.toolStripMenuItemAll.Click += new System.EventHandler(this.toolStripMenuItemAll_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(127, 6);
            // 
            // toolStripMenuItemInRow
            // 
            this.toolStripMenuItemInRow.CheckOnClick = true;
            this.toolStripMenuItemInRow.Name = "toolStripMenuItemInRow";
            this.toolStripMenuItemInRow.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuItemInRow.Text = "В сырье";
            this.toolStripMenuItemInRow.Click += new System.EventHandler(this.toolStripMenuItemInRow_Click);
            // 
            // toolStripMenuItemInReferences
            // 
            this.toolStripMenuItemInReferences.CheckOnClick = true;
            this.toolStripMenuItemInReferences.Name = "toolStripMenuItemInReferences";
            this.toolStripMenuItemInReferences.Size = new System.Drawing.Size(130, 22);
            this.toolStripMenuItemInReferences.Text = "В весовых";
            this.toolStripMenuItemInReferences.Click += new System.EventHandler(this.toolStripMenuItemInReferences_Click);
            // 
            // BtnReGroup
            // 
            this.BtnReGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnReGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnReGroup.Image = global::PGS.Common.Resources.edit;
            this.BtnReGroup.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnReGroup.Name = "BtnReGroup";
            this.BtnReGroup.Size = new System.Drawing.Size(23, 22);
            this.BtnReGroup.Text = "Редактирование групп";
            this.BtnReGroup.Visible = false;
            this.BtnReGroup.Click += new System.EventHandler(this.BtnReGroup_Click);
            // 
            // BtnFind
            // 
            this.BtnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnFind.Image = global::PGS.Common.Resources.find;
            this.BtnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnFind.Name = "BtnFind";
            this.BtnFind.Size = new System.Drawing.Size(23, 22);
            this.BtnFind.Text = "Искать";
            this.BtnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_molar
            // 
            this.toolStripButton_molar.Checked = true;
            this.toolStripButton_molar.CheckOnClick = true;
            this.toolStripButton_molar.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_molar.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_molar.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_molar.Image")));
            this.toolStripButton_molar.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_molar.Name = "toolStripButton_molar";
            this.toolStripButton_molar.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_molar.Text = "М";
            this.toolStripButton_molar.ToolTipText = "Молярная масса";
            this.toolStripButton_molar.CheckedChanged += new System.EventHandler(this.toolStripButton1_CheckedChanged);
            // 
            // toolStripButton_density
            // 
            this.toolStripButton_density.Checked = true;
            this.toolStripButton_density.CheckOnClick = true;
            this.toolStripButton_density.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_density.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_density.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_density.Image")));
            this.toolStripButton_density.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_density.Name = "toolStripButton_density";
            this.toolStripButton_density.Size = new System.Drawing.Size(25, 22);
            this.toolStripButton_density.Text = "Ро";
            this.toolStripButton_density.ToolTipText = "Плотность жидкой фазы";
            this.toolStripButton_density.CheckedChanged += new System.EventHandler(this.toolStripButton2_CheckedChanged);
            // 
            // toolStripButton_rnp
            // 
            this.toolStripButton_rnp.CheckOnClick = true;
            this.toolStripButton_rnp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_rnp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_rnp.Image")));
            this.toolStripButton_rnp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_rnp.Name = "toolStripButton_rnp";
            this.toolStripButton_rnp.Size = new System.Drawing.Size(35, 22);
            this.toolStripButton_rnp.Text = "Рнп.";
            this.toolStripButton_rnp.ToolTipText = "Давление насыщеных паров";
            this.toolStripButton_rnp.CheckedChanged += new System.EventHandler(this.toolStripButton3_CheckedChanged);
            // 
            // toolStripButton_kflow
            // 
            this.toolStripButton_kflow.Checked = true;
            this.toolStripButton_kflow.CheckOnClick = true;
            this.toolStripButton_kflow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButton_kflow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_kflow.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_kflow.Image")));
            this.toolStripButton_kflow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_kflow.Name = "toolStripButton_kflow";
            this.toolStripButton_kflow.Size = new System.Drawing.Size(46, 22);
            this.toolStripButton_kflow.Text = "Красх.";
            this.toolStripButton_kflow.ToolTipText = "Коэффициент расхода";
            this.toolStripButton_kflow.CheckedChanged += new System.EventHandler(this.toolStripButton4_CheckedChanged);
            // 
            // toolStripButton_toxicity
            // 
            this.toolStripButton_toxicity.CheckOnClick = true;
            this.toolStripButton_toxicity.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_toxicity.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_toxicity.Image")));
            this.toolStripButton_toxicity.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_toxicity.Name = "toolStripButton_toxicity";
            this.toolStripButton_toxicity.Size = new System.Drawing.Size(39, 22);
            this.toolStripButton_toxicity.Text = "Токс.";
            this.toolStripButton_toxicity.ToolTipText = "Токсичность";
            this.toolStripButton_toxicity.CheckedChanged += new System.EventHandler(this.toolStripButton5_CheckedChanged);
            // 
            // toolStripButton_flammability
            // 
            this.toolStripButton_flammability.CheckOnClick = true;
            this.toolStripButton_flammability.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_flammability.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_flammability.Image")));
            this.toolStripButton_flammability.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_flammability.Name = "toolStripButton_flammability";
            this.toolStripButton_flammability.Size = new System.Drawing.Size(48, 22);
            this.toolStripButton_flammability.Text = "Воспл.";
            this.toolStripButton_flammability.ToolTipText = "Воспламеняемость";
            this.toolStripButton_flammability.CheckedChanged += new System.EventHandler(this.toolStripButton6_CheckedChanged);
            // 
            // toolStripButton_critical
            // 
            this.toolStripButton_critical.CheckOnClick = true;
            this.toolStripButton_critical.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton_critical.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_critical.Image")));
            this.toolStripButton_critical.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_critical.Name = "toolStripButton_critical";
            this.toolStripButton_critical.Size = new System.Drawing.Size(93, 22);
            this.toolStripButton_critical.Text = "Кр. параметры";
            this.toolStripButton_critical.ToolTipText = "Критические параметры";
            this.toolStripButton_critical.CheckedChanged += new System.EventHandler(this.toolStripButton7_CheckedChanged);
            // 
            // BtnShowLog
            // 
            this.BtnShowLog.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.BtnShowLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnShowLog.Image = global::PGS.Common.Resources.view_history;
            this.BtnShowLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnShowLog.Name = "BtnShowLog";
            this.BtnShowLog.Size = new System.Drawing.Size(23, 22);
            this.BtnShowLog.Text = "История изменений";
            this.BtnShowLog.Click += new System.EventHandler(this.BtnShowLog_Click);
            // 
            // FormCompsList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1069, 526);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormCompsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Компоненты";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCompParamList_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCompsList_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton_critical;
        private System.Windows.Forms.ToolStripButton toolStripButton_flammability;
        private System.Windows.Forms.ToolStripButton toolStripButton_toxicity;
        private System.Windows.Forms.ToolStripButton toolStripButton_kflow;
        private System.Windows.Forms.ToolStripButton toolStripButton_rnp;
        private System.Windows.Forms.ToolStripButton toolStripButton_density;
        private System.Windows.Forms.ToolStripButton toolStripButton_molar;
        private System.Windows.Forms.ToolStripLabel toolStripLabelShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton BtnFind;
        private System.Windows.Forms.ToolStripTextBox tbFind;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox cbGroup;
        private System.Windows.Forms.ToolStripLabel toolStripLabelGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton BtnRemove;
        private System.Windows.Forms.ToolStripButton BtnEdit;
        private System.Windows.Forms.ToolStripButton BtnAdd;
        private System.Windows.Forms.ToolStripButton BtnReGroup;
        private DataGridViewTextBoxColumn dgv_column_id;
        private DataGridViewTextBoxColumn dgv_column_Name;
        private DataGridViewTextBoxColumn dgv_column_NameAlt;
        private DataGridViewCheckBoxColumn dgv_column_Isomer;
        private DataGridViewTextBoxColumn dgv_column_Formula;
        private DataGridViewCheckBoxColumn dgv_column_toxic;
        private DataGridViewCheckBoxColumn dgv_column_flammability;
        private DataGridViewTextBoxColumn dgv_column_MolarMass;
        private DataGridViewTextBoxColumn dgv_column_dMolarMass;
        private DataGridViewTextBoxColumn dgv_column_MassDensity;
        private DataGridViewTextBoxColumn dgv_column_Pss243k;
        private DataGridViewTextBoxColumn dgv_column_Pss253k;
        private DataGridViewTextBoxColumn dgv_column_Pss263k;
        private DataGridViewTextBoxColumn dgv_column_Pss273k;
        private DataGridViewTextBoxColumn dgv_column_Pss283k;
        private DataGridViewTextBoxColumn dgv_column_Pss293k;
        private DataGridViewTextBoxColumn dgv_column_Kflow;
        private DataGridViewTextBoxColumn dgv_column_toxicity;
        private DataGridViewTextBoxColumn dgv_column_units;
        private DataGridViewTextBoxColumn dgv_column_max_conc;
        private DataGridViewTextBoxColumn dgv_column_nkpr;
        private DataGridViewTextBoxColumn dgv_column_vkpr;
        private DataGridViewTextBoxColumn dgv_column_tkip;
        private DataGridViewTextBoxColumn dgv_column_tkr;
        private DataGridViewTextBoxColumn dgv_column_pkr;
        private ToolStripButton BtnShowLog;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripDropDownButton tsDropDownBtnFilter;
        private ToolStripMenuItem toolStripMenuItemAll;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem toolStripMenuItemInRow;
        private ToolStripMenuItem toolStripMenuItemInReferences;
    }
}

