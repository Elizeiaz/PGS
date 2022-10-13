namespace PGS.DMS.UI
{
    partial class UserControlDocumentEdit
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbGroupSelect = new System.Windows.Forms.TextBox();
            this.labelGroupName = new System.Windows.Forms.Label();
            this.labelDocName = new System.Windows.Forms.Label();
            this.tbDocName = new System.Windows.Forms.TextBox();
            this.labelDocNumber = new System.Windows.Forms.Label();
            this.labelDocCreator = new System.Windows.Forms.Label();
            this.tbDocNumber = new System.Windows.Forms.TextBox();
            this.labelDocDateIn = new System.Windows.Forms.Label();
            this.labelDocDateRevisionNext = new System.Windows.Forms.Label();
            this.timePickerDocDateRevisionNext = new System.Windows.Forms.DateTimePicker();
            this.labelDocDateOut = new System.Windows.Forms.Label();
            this.timePickerDocDateOut = new System.Windows.Forms.DateTimePicker();
            this.labelDocCreatorValue = new System.Windows.Forms.Label();
            this.labelDocResponsible = new System.Windows.Forms.Label();
            this.labelDocPlace = new System.Windows.Forms.Label();
            this.checkBoxIsPhysicalMedium = new System.Windows.Forms.CheckBox();
            this.timePickerDateIn = new System.Windows.Forms.DateTimePicker();
            this.labelDocDateRevision = new System.Windows.Forms.Label();
            this.timePickerDocDateRevision = new System.Windows.Forms.DateTimePicker();
            this.tbDocResponsible = new System.Windows.Forms.TextBox();
            this.tbDocPlace = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbGroupSelect
            // 
            this.tbGroupSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGroupSelect.BackColor = System.Drawing.SystemColors.Window;
            this.tbGroupSelect.Location = new System.Drawing.Point(130, 7);
            this.tbGroupSelect.MinimumSize = new System.Drawing.Size(121, 20);
            this.tbGroupSelect.Name = "tbGroupSelect";
            this.tbGroupSelect.ReadOnly = true;
            this.tbGroupSelect.Size = new System.Drawing.Size(121, 20);
            this.tbGroupSelect.TabIndex = 3;
            // 
            // labelGroupName
            // 
            this.labelGroupName.AutoSize = true;
            this.labelGroupName.Location = new System.Drawing.Point(88, 10);
            this.labelGroupName.Name = "labelGroupName";
            this.labelGroupName.Size = new System.Drawing.Size(42, 13);
            this.labelGroupName.TabIndex = 4;
            this.labelGroupName.Text = "Группа";
            // 
            // labelDocName
            // 
            this.labelDocName.AutoSize = true;
            this.labelDocName.Location = new System.Drawing.Point(73, 34);
            this.labelDocName.Name = "labelDocName";
            this.labelDocName.Size = new System.Drawing.Size(57, 13);
            this.labelDocName.TabIndex = 5;
            this.labelDocName.Text = "Название";
            // 
            // tbDocName
            // 
            this.tbDocName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDocName.BackColor = System.Drawing.SystemColors.Window;
            this.tbDocName.Location = new System.Drawing.Point(130, 31);
            this.tbDocName.MinimumSize = new System.Drawing.Size(121, 20);
            this.tbDocName.Name = "tbDocName";
            this.tbDocName.ReadOnly = true;
            this.tbDocName.Size = new System.Drawing.Size(121, 20);
            this.tbDocName.TabIndex = 6;
            // 
            // labelDocNumber
            // 
            this.labelDocNumber.AutoSize = true;
            this.labelDocNumber.Location = new System.Drawing.Point(85, 58);
            this.labelDocNumber.Name = "labelDocNumber";
            this.labelDocNumber.Size = new System.Drawing.Size(41, 13);
            this.labelDocNumber.TabIndex = 7;
            this.labelDocNumber.Text = "Номер";
            // 
            // labelDocCreator
            // 
            this.labelDocCreator.AutoSize = true;
            this.labelDocCreator.Location = new System.Drawing.Point(69, 90);
            this.labelDocCreator.Name = "labelDocCreator";
            this.labelDocCreator.Size = new System.Drawing.Size(61, 13);
            this.labelDocCreator.TabIndex = 8;
            this.labelDocCreator.Text = "Создатель";
            // 
            // tbDocNumber
            // 
            this.tbDocNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDocNumber.BackColor = System.Drawing.SystemColors.Window;
            this.tbDocNumber.Location = new System.Drawing.Point(130, 55);
            this.tbDocNumber.MinimumSize = new System.Drawing.Size(121, 20);
            this.tbDocNumber.Name = "tbDocNumber";
            this.tbDocNumber.ReadOnly = true;
            this.tbDocNumber.Size = new System.Drawing.Size(121, 20);
            this.tbDocNumber.TabIndex = 11;
            // 
            // labelDocDateIn
            // 
            this.labelDocDateIn.AutoSize = true;
            this.labelDocDateIn.Location = new System.Drawing.Point(58, 120);
            this.labelDocDateIn.Name = "labelDocDateIn";
            this.labelDocDateIn.Size = new System.Drawing.Size(70, 13);
            this.labelDocDateIn.TabIndex = 12;
            this.labelDocDateIn.Text = "Действует с";
            // 
            // labelDocDateRevisionNext
            // 
            this.labelDocDateRevisionNext.AutoSize = true;
            this.labelDocDateRevisionNext.Location = new System.Drawing.Point(19, 211);
            this.labelDocDateRevisionNext.Name = "labelDocDateRevisionNext";
            this.labelDocDateRevisionNext.Size = new System.Drawing.Size(111, 13);
            this.labelDocDateRevisionNext.TabIndex = 14;
            this.labelDocDateRevisionNext.Text = "Следующая ревизия";
            // 
            // timePickerDocDateRevisionNext
            // 
            this.timePickerDocDateRevisionNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.timePickerDocDateRevisionNext.Location = new System.Drawing.Point(130, 206);
            this.timePickerDocDateRevisionNext.Name = "timePickerDocDateRevisionNext";
            this.timePickerDocDateRevisionNext.Size = new System.Drawing.Size(121, 20);
            this.timePickerDocDateRevisionNext.TabIndex = 15;
            // 
            // labelDocDateOut
            // 
            this.labelDocDateOut.AutoSize = true;
            this.labelDocDateOut.Location = new System.Drawing.Point(107, 146);
            this.labelDocDateOut.Name = "labelDocDateOut";
            this.labelDocDateOut.Size = new System.Drawing.Size(21, 13);
            this.labelDocDateOut.TabIndex = 16;
            this.labelDocDateOut.Text = "По";
            // 
            // timePickerDocDateOut
            // 
            this.timePickerDocDateOut.Location = new System.Drawing.Point(130, 142);
            this.timePickerDocDateOut.Name = "timePickerDocDateOut";
            this.timePickerDocDateOut.Size = new System.Drawing.Size(121, 20);
            this.timePickerDocDateOut.TabIndex = 17;
            // 
            // labelDocCreatorValue
            // 
            this.labelDocCreatorValue.AutoSize = true;
            this.labelDocCreatorValue.Location = new System.Drawing.Point(138, 90);
            this.labelDocCreatorValue.Name = "labelDocCreatorValue";
            this.labelDocCreatorValue.Size = new System.Drawing.Size(34, 13);
            this.labelDocCreatorValue.TabIndex = 18;
            this.labelDocCreatorValue.Text = "ФИО";
            // 
            // labelDocResponsible
            // 
            this.labelDocResponsible.AutoSize = true;
            this.labelDocResponsible.Location = new System.Drawing.Point(41, 250);
            this.labelDocResponsible.Name = "labelDocResponsible";
            this.labelDocResponsible.Size = new System.Drawing.Size(86, 13);
            this.labelDocResponsible.TabIndex = 19;
            this.labelDocResponsible.Text = "Ответственный";
            // 
            // labelDocPlace
            // 
            this.labelDocPlace.AutoSize = true;
            this.labelDocPlace.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDocPlace.Location = new System.Drawing.Point(42, 319);
            this.labelDocPlace.Name = "labelDocPlace";
            this.labelDocPlace.Size = new System.Drawing.Size(89, 13);
            this.labelDocPlace.TabIndex = 22;
            this.labelDocPlace.Text = "Место хранения";
            // 
            // checkBoxIsPhysicalMedium
            // 
            this.checkBoxIsPhysicalMedium.AutoSize = true;
            this.checkBoxIsPhysicalMedium.Location = new System.Drawing.Point(4, 293);
            this.checkBoxIsPhysicalMedium.Name = "checkBoxIsPhysicalMedium";
            this.checkBoxIsPhysicalMedium.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxIsPhysicalMedium.Size = new System.Drawing.Size(143, 17);
            this.checkBoxIsPhysicalMedium.TabIndex = 23;
            this.checkBoxIsPhysicalMedium.Text = "Физический носитель ";
            this.checkBoxIsPhysicalMedium.UseVisualStyleBackColor = true;
            this.checkBoxIsPhysicalMedium.CheckedChanged += new System.EventHandler(this.checkBoxIsPhysicalMedium_CheckedChanged);
            // 
            // timePickerDateIn
            // 
            this.timePickerDateIn.Location = new System.Drawing.Point(130, 117);
            this.timePickerDateIn.Name = "timePickerDateIn";
            this.timePickerDateIn.Size = new System.Drawing.Size(121, 20);
            this.timePickerDateIn.TabIndex = 26;
            // 
            // labelDocDateRevision
            // 
            this.labelDocDateRevision.AutoSize = true;
            this.labelDocDateRevision.Location = new System.Drawing.Point(28, 183);
            this.labelDocDateRevision.Name = "labelDocDateRevision";
            this.labelDocDateRevision.Size = new System.Drawing.Size(96, 13);
            this.labelDocDateRevision.TabIndex = 27;
            this.labelDocDateRevision.Text = "Актуализировано";
            // 
            // timePickerDocDateRevision
            // 
            this.timePickerDocDateRevision.Location = new System.Drawing.Point(130, 181);
            this.timePickerDocDateRevision.Name = "timePickerDocDateRevision";
            this.timePickerDocDateRevision.Size = new System.Drawing.Size(121, 20);
            this.timePickerDocDateRevision.TabIndex = 28;
            // 
            // tbDocResponsible
            // 
            this.tbDocResponsible.BackColor = System.Drawing.SystemColors.Window;
            this.tbDocResponsible.Location = new System.Drawing.Point(133, 247);
            this.tbDocResponsible.Name = "tbDocResponsible";
            this.tbDocResponsible.ReadOnly = true;
            this.tbDocResponsible.Size = new System.Drawing.Size(118, 20);
            this.tbDocResponsible.TabIndex = 29;
            // 
            // tbDocPlace
            // 
            this.tbDocPlace.BackColor = System.Drawing.SystemColors.Window;
            this.tbDocPlace.Location = new System.Drawing.Point(133, 316);
            this.tbDocPlace.Name = "tbDocPlace";
            this.tbDocPlace.ReadOnly = true;
            this.tbDocPlace.Size = new System.Drawing.Size(118, 20);
            this.tbDocPlace.TabIndex = 30;
            // 
            // UserControlDocumentEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tbDocPlace);
            this.Controls.Add(this.tbDocResponsible);
            this.Controls.Add(this.timePickerDocDateRevision);
            this.Controls.Add(this.labelDocDateRevision);
            this.Controls.Add(this.timePickerDateIn);
            this.Controls.Add(this.checkBoxIsPhysicalMedium);
            this.Controls.Add(this.labelDocPlace);
            this.Controls.Add(this.labelDocResponsible);
            this.Controls.Add(this.labelDocCreatorValue);
            this.Controls.Add(this.timePickerDocDateOut);
            this.Controls.Add(this.labelDocDateOut);
            this.Controls.Add(this.timePickerDocDateRevisionNext);
            this.Controls.Add(this.labelDocDateRevisionNext);
            this.Controls.Add(this.labelDocDateIn);
            this.Controls.Add(this.tbDocNumber);
            this.Controls.Add(this.labelDocCreator);
            this.Controls.Add(this.labelDocNumber);
            this.Controls.Add(this.tbDocName);
            this.Controls.Add(this.labelDocName);
            this.Controls.Add(this.labelGroupName);
            this.Controls.Add(this.tbGroupSelect);
            this.DoubleBuffered = true;
            this.Name = "UserControlDocumentEdit";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.Size = new System.Drawing.Size(263, 345);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbGroupSelect;
        private System.Windows.Forms.Label labelGroupName;
        private System.Windows.Forms.Label labelDocName;
        private System.Windows.Forms.TextBox tbDocName;
        private System.Windows.Forms.Label labelDocNumber;
        private System.Windows.Forms.Label labelDocCreator;
        private System.Windows.Forms.TextBox tbDocNumber;
        private System.Windows.Forms.Label labelDocDateIn;
        private System.Windows.Forms.Label labelDocDateRevisionNext;
        private System.Windows.Forms.DateTimePicker timePickerDocDateRevisionNext;
        private System.Windows.Forms.Label labelDocDateOut;
        private System.Windows.Forms.DateTimePicker timePickerDocDateOut;
        private System.Windows.Forms.Label labelDocCreatorValue;
        private System.Windows.Forms.Label labelDocResponsible;
        private System.Windows.Forms.Label labelDocPlace;
        private System.Windows.Forms.CheckBox checkBoxIsPhysicalMedium;
        private System.Windows.Forms.DateTimePicker timePickerDateIn;
        private System.Windows.Forms.Label labelDocDateRevision;
        private System.Windows.Forms.DateTimePicker timePickerDocDateRevision;
        private System.Windows.Forms.TextBox tbDocResponsible;
        private System.Windows.Forms.TextBox tbDocPlace;
    }
}
