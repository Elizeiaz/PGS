namespace PGS.UI.Comps
{
    partial class FormSelectComp
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.dgv_Column_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_NameAlt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_Column_Isomer = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgv_Column_Formula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BtnOk = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            this.dgv.AllowUserToResizeRows = false;
            this.dgv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_Column_ID,
            this.dgv_Column_Name,
            this.dgv_Column_NameAlt,
            this.dgv_Column_Isomer,
            this.dgv_Column_Formula});
            this.dgv.Location = new System.Drawing.Point(12, 12);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(394, 397);
            this.dgv.TabIndex = 0;
            this.dgv.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dgv.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // dgv_Column_ID
            // 
            this.dgv_Column_ID.DataPropertyName = "ID";
            this.dgv_Column_ID.HeaderText = "ID";
            this.dgv_Column_ID.Name = "dgv_Column_ID";
            this.dgv_Column_ID.ReadOnly = true;
            this.dgv_Column_ID.Visible = false;
            // 
            // dgv_Column_Name
            // 
            this.dgv_Column_Name.DataPropertyName = "Name";
            this.dgv_Column_Name.HeaderText = "Название";
            this.dgv_Column_Name.Name = "dgv_Column_Name";
            this.dgv_Column_Name.ReadOnly = true;
            this.dgv_Column_Name.Width = 150;
            // 
            // dgv_Column_NameAlt
            // 
            this.dgv_Column_NameAlt.DataPropertyName = "NameAlt";
            this.dgv_Column_NameAlt.HeaderText = "Название 2";
            this.dgv_Column_NameAlt.Name = "dgv_Column_NameAlt";
            this.dgv_Column_NameAlt.ReadOnly = true;
            this.dgv_Column_NameAlt.Width = 50;
            // 
            // dgv_Column_Isomer
            // 
            this.dgv_Column_Isomer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.dgv_Column_Isomer.DataPropertyName = "Isomer";
            this.dgv_Column_Isomer.HeaderText = "Изомер";
            this.dgv_Column_Isomer.Name = "dgv_Column_Isomer";
            this.dgv_Column_Isomer.ReadOnly = true;
            this.dgv_Column_Isomer.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_Column_Isomer.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dgv_Column_Isomer.Width = 5;
            // 
            // dgv_Column_Formula
            // 
            this.dgv_Column_Formula.DataPropertyName = "Formula";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgv_Column_Formula.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_Column_Formula.HeaderText = "Формула";
            this.dgv_Column_Formula.Name = "dgv_Column_Formula";
            this.dgv_Column_Formula.ReadOnly = true;
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.Enabled = false;
            this.BtnOk.Image = global::PGS.Common.Resources.dialog_ok_apply;
            this.BtnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnOk.Location = new System.Drawing.Point(160, 421);
            this.BtnOk.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.BtnOk.Size = new System.Drawing.Size(117, 26);
            this.BtnOk.TabIndex = 1;
            this.BtnOk.Text = "Выбрать";
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.Image = global::PGS.Common.Resources.dialog_cancel;
            this.BtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCancel.Location = new System.Drawing.Point(289, 421);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.BtnCancel.Size = new System.Drawing.Size(117, 26);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Отмена";
            this.BtnCancel.UseVisualStyleBackColor = true;
            // 
            // FormSelectComp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.BtnCancel;
            this.ClientSize = new System.Drawing.Size(418, 459);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectComp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выбор компонента";
            this.Load += new System.EventHandler(this.FormSelectComp_Load);
            this.Shown += new System.EventHandler(this.FormSelectComp_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_NameAlt;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgv_Column_Isomer;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_Column_Formula;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button BtnCancel;
    }
}