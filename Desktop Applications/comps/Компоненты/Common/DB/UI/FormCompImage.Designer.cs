namespace PGS.Comps.UI
{
    partial class FormCompImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCompImage));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.BtnImageLoad = new System.Windows.Forms.ToolStripButton();
            this.BtnImagePaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.BtnImageDelete = new System.Windows.Forms.ToolStripButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.BtnImageLoad,
            this.BtnImagePaste,
            this.toolStripSeparator1,
            this.BtnImageDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(689, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // BtnImageLoad
            // 
            this.BtnImageLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnImageLoad.Enabled = false;
            this.BtnImageLoad.Image = global::PGS.Common.Resources.document_open;
            this.BtnImageLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnImageLoad.Name = "BtnImageLoad";
            this.BtnImageLoad.Size = new System.Drawing.Size(23, 22);
            this.BtnImageLoad.Text = "Загрузить из файла";
            this.BtnImageLoad.Click += new System.EventHandler(this.BtnImageLoad_Click);
            // 
            // BtnImagePaste
            // 
            this.BtnImagePaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnImagePaste.Enabled = false;
            this.BtnImagePaste.Image = global::PGS.Common.Resources.edit_paste;
            this.BtnImagePaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnImagePaste.Name = "BtnImagePaste";
            this.BtnImagePaste.Size = new System.Drawing.Size(23, 22);
            this.BtnImagePaste.Text = "Вставить";
            this.BtnImagePaste.Click += new System.EventHandler(this.BtnImagePaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // BtnImageDelete
            // 
            this.BtnImageDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.BtnImageDelete.Enabled = false;
            this.BtnImageDelete.Image = global::PGS.Common.Resources.edit_clear_list;
            this.BtnImageDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.BtnImageDelete.Name = "BtnImageDelete";
            this.BtnImageDelete.Size = new System.Drawing.Size(23, 22);
            this.BtnImageDelete.Text = "Удалить";
            this.BtnImageDelete.Click += new System.EventHandler(this.BtnImageDelete_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::PGS.Common.Resources.insertImg;
            this.pictureBox1.Location = new System.Drawing.Point(0, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(689, 465);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // FormCompImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 490);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "FormCompImage";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Структурная формула";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCompImage_KeyDown);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton BtnImageLoad;
        private System.Windows.Forms.ToolStripButton BtnImagePaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton BtnImageDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}