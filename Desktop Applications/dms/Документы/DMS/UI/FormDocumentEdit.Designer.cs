namespace PGS.DMS.UI
{
    partial class FormDocumentEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsBtnFileAdd = new System.Windows.Forms.ToolStripButton();
            this.tsBtnFileRemove = new System.Windows.Forms.ToolStripButton();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxIsPhysicalMedium = new System.Windows.Forms.CheckBox();
            this.labelDocPlace = new System.Windows.Forms.Label();
            this.labelDocResponsible = new System.Windows.Forms.Label();
            this.labelDocCreatorValue = new System.Windows.Forms.Label();
            this.tbDocDateOut = new System.Windows.Forms.DateTimePicker();
            this.labelDocDateOut = new System.Windows.Forms.Label();
            this.tbDocNextRevisionDate = new System.Windows.Forms.DateTimePicker();
            this.labelDocDateRevision = new System.Windows.Forms.Label();
            this.labelDocDateIn = new System.Windows.Forms.Label();
            this.tbDocNumber = new System.Windows.Forms.TextBox();
            this.labelDocCreator = new System.Windows.Forms.Label();
            this.labelDocNumber = new System.Windows.Forms.Label();
            this.tbDocName = new System.Windows.Forms.TextBox();
            this.labelDocName = new System.Windows.Forms.Label();
            this.labelGroupName = new System.Windows.Forms.Label();
            this.tbGroupSelect = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tbDocDateIn = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.cbDocResponsible = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDocRevisionDate = new System.Windows.Forms.DateTimePicker();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.cbDocPlace = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDocComment = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnFileAdd,
            this.tsBtnFileRemove});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(821, 33);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsBtnFileAdd
            // 
            this.tsBtnFileAdd.AutoSize = false;
            this.tsBtnFileAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnFileAdd.Image = global::PGS.DMS.Properties.Resources.list_add;
            this.tsBtnFileAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnFileAdd.Name = "tsBtnFileAdd";
            this.tsBtnFileAdd.Size = new System.Drawing.Size(30, 30);
            this.tsBtnFileAdd.Text = "Добавить файл";
            this.tsBtnFileAdd.Click += new System.EventHandler(this.btnFileAdd_Click);
            // 
            // tsBtnFileRemove
            // 
            this.tsBtnFileRemove.AutoSize = false;
            this.tsBtnFileRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBtnFileRemove.Image = global::PGS.DMS.Properties.Resources.list_remove;
            this.tsBtnFileRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBtnFileRemove.Name = "tsBtnFileRemove";
            this.tsBtnFileRemove.Size = new System.Drawing.Size(30, 30);
            this.tsBtnFileRemove.Text = "Удалить файл";
            this.tsBtnFileRemove.Click += new System.EventHandler(this.btnFileRemove_Click);
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.name,
            this.date,
            this.creator,
            this.source});
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(3, 36);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowHeadersWidth = 51;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(821, 226);
            this.dgv.TabIndex = 0;
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellDoubleClick);
            this.dgv.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgv_CellValidating);
            this.dgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellValueChanged);
            this.dgv.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgv_KeyDown);
            this.dgv.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgv_MouseDown);
            // 
            // id
            // 
            this.id.HeaderText = "id";
            this.id.MinimumWidth = 6;
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            this.id.Width = 35;
            // 
            // name
            // 
            this.name.HeaderText = "Название";
            this.name.MinimumWidth = 6;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 175;
            // 
            // date
            // 
            this.date.HeaderText = "Дата";
            this.date.MinimumWidth = 6;
            this.date.Name = "date";
            this.date.Width = 85;
            // 
            // creator
            // 
            this.creator.HeaderText = "Создал";
            this.creator.MinimumWidth = 6;
            this.creator.Name = "creator";
            this.creator.ReadOnly = true;
            this.creator.Width = 125;
            // 
            // source
            // 
            this.source.HeaderText = "Источник";
            this.source.MinimumWidth = 6;
            this.source.Name = "source";
            this.source.Width = 250;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(646, 524);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 29);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "Сохранить";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(745, 524);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 29);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // checkBoxIsPhysicalMedium
            // 
            this.checkBoxIsPhysicalMedium.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.checkBoxIsPhysicalMedium.AutoSize = true;
            this.checkBoxIsPhysicalMedium.Location = new System.Drawing.Point(4, 5);
            this.checkBoxIsPhysicalMedium.Name = "checkBoxIsPhysicalMedium";
            this.checkBoxIsPhysicalMedium.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxIsPhysicalMedium.Size = new System.Drawing.Size(143, 17);
            this.checkBoxIsPhysicalMedium.TabIndex = 42;
            this.checkBoxIsPhysicalMedium.Text = "Физический носитель ";
            this.checkBoxIsPhysicalMedium.UseVisualStyleBackColor = true;
            this.checkBoxIsPhysicalMedium.CheckedChanged += new System.EventHandler(this.checkBoxIsPhysicalMedium_CheckedChanged);
            // 
            // labelDocPlace
            // 
            this.labelDocPlace.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocPlace.AutoSize = true;
            this.labelDocPlace.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelDocPlace.Location = new System.Drawing.Point(178, 7);
            this.labelDocPlace.Name = "labelDocPlace";
            this.labelDocPlace.Size = new System.Drawing.Size(89, 13);
            this.labelDocPlace.TabIndex = 41;
            this.labelDocPlace.Text = "Место хранения";
            // 
            // labelDocResponsible
            // 
            this.labelDocResponsible.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocResponsible.AutoSize = true;
            this.labelDocResponsible.Location = new System.Drawing.Point(471, 7);
            this.labelDocResponsible.Name = "labelDocResponsible";
            this.labelDocResponsible.Size = new System.Drawing.Size(86, 13);
            this.labelDocResponsible.TabIndex = 40;
            this.labelDocResponsible.Text = "Ответственный";
            // 
            // labelDocCreatorValue
            // 
            this.labelDocCreatorValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDocCreatorValue.AutoSize = true;
            this.labelDocCreatorValue.Location = new System.Drawing.Point(563, 7);
            this.labelDocCreatorValue.Name = "labelDocCreatorValue";
            this.labelDocCreatorValue.Size = new System.Drawing.Size(254, 13);
            this.labelDocCreatorValue.TabIndex = 39;
            this.labelDocCreatorValue.Text = "ФИО";
            // 
            // tbDocDateOut
            // 
            this.tbDocDateOut.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbDocDateOut.CustomFormat = " ";
            this.tbDocDateOut.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tbDocDateOut.Location = new System.Drawing.Point(273, 4);
            this.tbDocDateOut.Name = "tbDocDateOut";
            this.tbDocDateOut.Size = new System.Drawing.Size(84, 20);
            this.tbDocDateOut.TabIndex = 38;
            this.tbDocDateOut.ValueChanged += new System.EventHandler(this.timePickerDocDateOut_ValueChanged);
            this.tbDocDateOut.KeyDown += new System.Windows.Forms.KeyEventHandler(this.timePickerDocDateOut_KeyDown);
            // 
            // labelDocDateOut
            // 
            this.labelDocDateOut.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocDateOut.AutoSize = true;
            this.labelDocDateOut.Location = new System.Drawing.Point(248, 7);
            this.labelDocDateOut.Name = "labelDocDateOut";
            this.labelDocDateOut.Size = new System.Drawing.Size(19, 13);
            this.labelDocDateOut.TabIndex = 37;
            this.labelDocDateOut.Text = "по";
            // 
            // tbDocNextRevisionDate
            // 
            this.tbDocNextRevisionDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbDocNextRevisionDate.CustomFormat = " ";
            this.tbDocNextRevisionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tbDocNextRevisionDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbDocNextRevisionDate.Location = new System.Drawing.Point(363, 4);
            this.tbDocNextRevisionDate.Name = "tbDocNextRevisionDate";
            this.tbDocNextRevisionDate.Size = new System.Drawing.Size(84, 20);
            this.tbDocNextRevisionDate.TabIndex = 36;
            this.tbDocNextRevisionDate.ValueChanged += new System.EventHandler(this.timePickerDocDateRevision_ValueChanged);
            this.tbDocNextRevisionDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.timePickerDocDateRevision_KeyDown);
            // 
            // labelDocDateRevision
            // 
            this.labelDocDateRevision.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocDateRevision.AutoSize = true;
            this.labelDocDateRevision.Location = new System.Drawing.Point(246, 7);
            this.labelDocDateRevision.Name = "labelDocDateRevision";
            this.labelDocDateRevision.Size = new System.Drawing.Size(111, 13);
            this.labelDocDateRevision.TabIndex = 35;
            this.labelDocDateRevision.Text = "Следующая ревизия";
            // 
            // labelDocDateIn
            // 
            this.labelDocDateIn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocDateIn.AutoSize = true;
            this.labelDocDateIn.Location = new System.Drawing.Point(77, 7);
            this.labelDocDateIn.Name = "labelDocDateIn";
            this.labelDocDateIn.Size = new System.Drawing.Size(70, 13);
            this.labelDocDateIn.TabIndex = 33;
            this.labelDocDateIn.Text = "Действует с";
            // 
            // tbDocNumber
            // 
            this.tbDocNumber.BackColor = System.Drawing.SystemColors.Window;
            this.tbDocNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDocNumber.Location = new System.Drawing.Point(624, 3);
            this.tbDocNumber.Name = "tbDocNumber";
            this.tbDocNumber.ReadOnly = true;
            this.tbDocNumber.Size = new System.Drawing.Size(193, 20);
            this.tbDocNumber.TabIndex = 32;
            this.tbDocNumber.TextChanged += new System.EventHandler(this.tbDocNumber_TextChanged);
            // 
            // labelDocCreator
            // 
            this.labelDocCreator.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocCreator.AutoSize = true;
            this.labelDocCreator.Location = new System.Drawing.Point(496, 7);
            this.labelDocCreator.Name = "labelDocCreator";
            this.labelDocCreator.Size = new System.Drawing.Size(61, 13);
            this.labelDocCreator.TabIndex = 31;
            this.labelDocCreator.Text = "Создатель";
            // 
            // labelDocNumber
            // 
            this.labelDocNumber.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocNumber.AutoSize = true;
            this.labelDocNumber.Location = new System.Drawing.Point(573, 7);
            this.labelDocNumber.Name = "labelDocNumber";
            this.labelDocNumber.Size = new System.Drawing.Size(45, 13);
            this.labelDocNumber.TabIndex = 30;
            this.labelDocNumber.Text = "Док. №";
            // 
            // tbDocName
            // 
            this.tbDocName.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.SetColumnSpan(this.tbDocName, 3);
            this.tbDocName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbDocName.Location = new System.Drawing.Point(153, 31);
            this.tbDocName.Name = "tbDocName";
            this.tbDocName.ReadOnly = true;
            this.tbDocName.Size = new System.Drawing.Size(664, 20);
            this.tbDocName.TabIndex = 29;
            this.tbDocName.TextChanged += new System.EventHandler(this.tbDocName_TextChanged);
            // 
            // labelDocName
            // 
            this.labelDocName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelDocName.AutoSize = true;
            this.labelDocName.Location = new System.Drawing.Point(90, 35);
            this.labelDocName.Name = "labelDocName";
            this.labelDocName.Size = new System.Drawing.Size(57, 13);
            this.labelDocName.TabIndex = 28;
            this.labelDocName.Text = "Название";
            // 
            // labelGroupName
            // 
            this.labelGroupName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelGroupName.AutoSize = true;
            this.labelGroupName.Location = new System.Drawing.Point(105, 7);
            this.labelGroupName.Name = "labelGroupName";
            this.labelGroupName.Size = new System.Drawing.Size(42, 13);
            this.labelGroupName.TabIndex = 27;
            this.labelGroupName.Text = "Группа";
            // 
            // tbGroupSelect
            // 
            this.tbGroupSelect.BackColor = System.Drawing.SystemColors.Window;
            this.tbGroupSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGroupSelect.Location = new System.Drawing.Point(153, 3);
            this.tbGroupSelect.Name = "tbGroupSelect";
            this.tbGroupSelect.ReadOnly = true;
            this.tbGroupSelect.Size = new System.Drawing.Size(390, 20);
            this.tbGroupSelect.TabIndex = 26;
            this.tbGroupSelect.Click += new System.EventHandler(this.tbGroupSelect_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.66666F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.labelGroupName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbGroupSelect, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDocNumber, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbDocNumber, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDocName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbDocName, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(820, 56);
            this.tableLayoutPanel1.TabIndex = 45;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 7;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.labelDocDateIn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelDocDateOut, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbDocDateOut, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelDocCreatorValue, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelDocCreator, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.tbDocDateIn, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 109);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(820, 28);
            this.tableLayoutPanel2.TabIndex = 46;
            // 
            // tbDocDateIn
            // 
            this.tbDocDateIn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tbDocDateIn.CustomFormat = " ";
            this.tbDocDateIn.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tbDocDateIn.Location = new System.Drawing.Point(153, 4);
            this.tbDocDateIn.Name = "tbDocDateIn";
            this.tbDocDateIn.Size = new System.Drawing.Size(84, 20);
            this.tbDocDateIn.TabIndex = 38;
            this.tbDocDateIn.ValueChanged += new System.EventHandler(this.tbDocDateIn_ValueChanged);
            this.tbDocDateIn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocDateIn_KeyDown);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 6;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.cbDocResponsible, 5, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelDocResponsible, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbDocNextRevisionDate, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.labelDocDateRevision, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.tbDocRevisionDate, 1, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(12, 143);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(821, 28);
            this.tableLayoutPanel3.TabIndex = 47;
            // 
            // cbDocResponsible
            // 
            this.cbDocResponsible.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDocResponsible.FormattingEnabled = true;
            this.cbDocResponsible.Location = new System.Drawing.Point(563, 3);
            this.cbDocResponsible.Name = "cbDocResponsible";
            this.cbDocResponsible.Size = new System.Drawing.Size(255, 21);
            this.cbDocResponsible.TabIndex = 44;
            this.cbDocResponsible.SelectedIndexChanged += new System.EventHandler(this.cbDocResponsible_SelectedIndexChanged);
            this.cbDocResponsible.Enter += new System.EventHandler(this.cbDocResponsible_Enter);
            this.cbDocResponsible.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbDocResponsible_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 35;
            this.label2.Text = "Актуализировано";
            // 
            // tbDocRevisionDate
            // 
            this.tbDocRevisionDate.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tbDocRevisionDate.CustomFormat = " ";
            this.tbDocRevisionDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.tbDocRevisionDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbDocRevisionDate.Location = new System.Drawing.Point(153, 4);
            this.tbDocRevisionDate.Name = "tbDocRevisionDate";
            this.tbDocRevisionDate.Size = new System.Drawing.Size(84, 20);
            this.tbDocRevisionDate.TabIndex = 36;
            this.tbDocRevisionDate.ValueChanged += new System.EventHandler(this.tbDocRevisionDate_ValueChanged);
            this.tbDocRevisionDate.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbDocRevisionDate_KeyDown);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 3;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 379F));
            this.tableLayoutPanel4.Controls.Add(this.checkBoxIsPhysicalMedium, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelDocPlace, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.cbDocPlace, 2, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(12, 180);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(649, 28);
            this.tableLayoutPanel4.TabIndex = 48;
            // 
            // cbDocPlace
            // 
            this.cbDocPlace.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbDocPlace.Enabled = false;
            this.cbDocPlace.FormattingEnabled = true;
            this.cbDocPlace.Location = new System.Drawing.Point(273, 3);
            this.cbDocPlace.Name = "cbDocPlace";
            this.cbDocPlace.Size = new System.Drawing.Size(373, 21);
            this.cbDocPlace.TabIndex = 43;
            this.cbDocPlace.Click += new System.EventHandler(this.cbDocPlace_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(5, 227);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(835, 291);
            this.tabControl1.TabIndex = 49;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dgv);
            this.tabPage1.Controls.Add(this.toolStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(827, 265);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Файлы";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.tbDocComment, 1, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(12, 74);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(821, 29);
            this.tableLayoutPanel5.TabIndex = 50;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Комментарий";
            // 
            // tbDocComment
            // 
            this.tbDocComment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDocComment.Location = new System.Drawing.Point(153, 4);
            this.tbDocComment.Name = "tbDocComment";
            this.tbDocComment.Size = new System.Drawing.Size(665, 20);
            this.tbDocComment.TabIndex = 1;
            this.tbDocComment.TextChanged += new System.EventHandler(this.tbDocComment_TextChanged);
            // 
            // FormDocumentEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(844, 565);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(852, 432);
            this.Name = "FormDocumentEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Новый документ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDocumentEdit_FormClosed);
            this.Load += new System.EventHandler(this.FormDocumentEdit_Load);
            this.Shown += new System.EventHandler(this.FormDocumentEdit_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormDocumentEdit_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsBtnFileAdd;
        private System.Windows.Forms.ToolStripButton tsBtnFileRemove;
        private System.Windows.Forms.CheckBox checkBoxIsPhysicalMedium;
        private System.Windows.Forms.Label labelDocPlace;
        private System.Windows.Forms.Label labelDocResponsible;
        private System.Windows.Forms.Label labelDocCreatorValue;
        private System.Windows.Forms.DateTimePicker tbDocDateOut;
        private System.Windows.Forms.Label labelDocDateOut;
        private System.Windows.Forms.DateTimePicker tbDocNextRevisionDate;
        private System.Windows.Forms.Label labelDocDateRevision;
        private System.Windows.Forms.Label labelDocDateIn;
        private System.Windows.Forms.TextBox tbDocNumber;
        private System.Windows.Forms.Label labelDocCreator;
        private System.Windows.Forms.Label labelDocNumber;
        private System.Windows.Forms.TextBox tbDocName;
        private System.Windows.Forms.Label labelDocName;
        private System.Windows.Forms.Label labelGroupName;
        private System.Windows.Forms.TextBox tbGroupSelect;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ComboBox cbDocPlace;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DateTimePicker tbDocDateIn;
        private System.Windows.Forms.ComboBox cbDocResponsible;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker tbDocRevisionDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDocComment;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn creator;
        private System.Windows.Forms.DataGridViewTextBoxColumn source;
    }
}