namespace PGS.DMS.UI
{
    partial class FormFileEdit
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
            this.label3 = new System.Windows.Forms.Label();
            this.linkfile = new System.Windows.Forms.LinkLabel();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BtnRemoveLink = new System.Windows.Forms.PictureBox();
            this.labelCreator = new System.Windows.Forms.Label();
            this.labelCreatorValue = new System.Windows.Forms.Label();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelSource = new System.Windows.Forms.Label();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.tbSource = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BtnRemoveLink)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Файл";
            // 
            // linkfile
            // 
            this.linkfile.AutoSize = true;
            this.linkfile.Location = new System.Drawing.Point(72, 18);
            this.linkfile.Name = "linkfile";
            this.linkfile.Size = new System.Drawing.Size(80, 13);
            this.linkfile.TabIndex = 4;
            this.linkfile.TabStop = true;
            this.linkfile.Text = "Выбрать файл";
            this.linkfile.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Location = new System.Drawing.Point(175, 130);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 5;
            this.BtnOk.Text = "ОК";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Location = new System.Drawing.Point(256, 130);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "Отмена";
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.button2_Click);
            // 
            // BtnRemoveLink
            // 
            this.BtnRemoveLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRemoveLink.Image = global::PGS.DMS.Properties.Resources.edit_delete;
            this.BtnRemoveLink.Location = new System.Drawing.Point(297, 16);
            this.BtnRemoveLink.Name = "BtnRemoveLink";
            this.BtnRemoveLink.Size = new System.Drawing.Size(16, 16);
            this.BtnRemoveLink.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.BtnRemoveLink.TabIndex = 8;
            this.BtnRemoveLink.TabStop = false;
            this.toolTip1.SetToolTip(this.BtnRemoveLink, "Удалить ссылку");
            this.BtnRemoveLink.Visible = false;
            this.BtnRemoveLink.Click += new System.EventHandler(this.BtnRemoveLink_Click);
            // 
            // labelCreator
            // 
            this.labelCreator.AutoSize = true;
            this.labelCreator.Location = new System.Drawing.Point(12, 45);
            this.labelCreator.Name = "labelCreator";
            this.labelCreator.Size = new System.Drawing.Size(52, 13);
            this.labelCreator.TabIndex = 9;
            this.labelCreator.Text = "Добавил";
            // 
            // labelCreatorValue
            // 
            this.labelCreatorValue.AutoSize = true;
            this.labelCreatorValue.Location = new System.Drawing.Point(72, 45);
            this.labelCreatorValue.Name = "labelCreatorValue";
            this.labelCreatorValue.Size = new System.Drawing.Size(34, 13);
            this.labelCreatorValue.TabIndex = 10;
            this.labelCreatorValue.Text = "ФИО";
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(12, 72);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(33, 13);
            this.labelDate.TabIndex = 11;
            this.labelDate.Text = "Дата";
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Location = new System.Drawing.Point(12, 99);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(55, 13);
            this.labelSource.TabIndex = 12;
            this.labelSource.Text = "Источник";
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(75, 68);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(137, 20);
            this.dateTimePicker.TabIndex = 13;
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // tbSource
            // 
            this.tbSource.Location = new System.Drawing.Point(75, 96);
            this.tbSource.Name = "tbSource";
            this.tbSource.Size = new System.Drawing.Size(228, 20);
            this.tbSource.TabIndex = 14;
            this.tbSource.TextChanged += new System.EventHandler(this.tbSource_TextChanged);
            // 
            // FormFileEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(343, 167);
            this.Controls.Add(this.tbSource);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.labelSource);
            this.Controls.Add(this.labelDate);
            this.Controls.Add(this.labelCreatorValue);
            this.Controls.Add(this.labelCreator);
            this.Controls.Add(this.BtnRemoveLink);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.linkfile);
            this.Controls.Add(this.label3);
            this.MinimumSize = new System.Drawing.Size(278, 177);
            this.Name = "FormFileEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Файл";
            this.Load += new System.EventHandler(this.FormFileEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BtnRemoveLink)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel linkfile;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox BtnRemoveLink;
        private System.Windows.Forms.Label labelCreator;
        private System.Windows.Forms.Label labelCreatorValue;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.TextBox tbSource;
    }
}