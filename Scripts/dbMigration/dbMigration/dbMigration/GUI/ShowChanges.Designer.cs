using System.ComponentModel;

namespace dbMigration.GUI
{
    partial class FormShowChanges
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.primRawAdd = new System.Windows.Forms.Label();
            this.primRawChange = new System.Windows.Forms.Label();
            this.primMixAdd = new System.Windows.Forms.Label();
            this.primMixChange = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.gasRawAdd = new System.Windows.Forms.Label();
            this.gasRawChange = new System.Windows.Forms.Label();
            this.gasMixAdd = new System.Windows.Forms.Label();
            this.gasMixChange = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cylAdd = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.spendingAdd = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.label1.Location = new System.Drawing.Point(82, -3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(233, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "Добавлено компонентов";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.label2.Location = new System.Drawing.Point(143, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "primMixGas";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(28, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Raws";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(236, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Mixes";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // primRawAdd
            // 
            this.primRawAdd.Location = new System.Drawing.Point(28, 89);
            this.primRawAdd.Name = "primRawAdd";
            this.primRawAdd.Size = new System.Drawing.Size(122, 23);
            this.primRawAdd.TabIndex = 4;
            this.primRawAdd.Text = "Добавлено:";
            this.primRawAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // primRawChange
            // 
            this.primRawChange.Location = new System.Drawing.Point(28, 112);
            this.primRawChange.Name = "primRawChange";
            this.primRawChange.Size = new System.Drawing.Size(122, 23);
            this.primRawChange.TabIndex = 5;
            this.primRawChange.Text = "Изменено:";
            this.primRawChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // primMixAdd
            // 
            this.primMixAdd.Location = new System.Drawing.Point(236, 89);
            this.primMixAdd.Name = "primMixAdd";
            this.primMixAdd.Size = new System.Drawing.Size(126, 23);
            this.primMixAdd.TabIndex = 6;
            this.primMixAdd.Text = "Добавлено:";
            this.primMixAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // primMixChange
            // 
            this.primMixChange.Location = new System.Drawing.Point(236, 112);
            this.primMixChange.Name = "primMixChange";
            this.primMixChange.Size = new System.Drawing.Size(126, 23);
            this.primMixChange.TabIndex = 7;
            this.primMixChange.Text = "Изменено";
            this.primMixChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.label9.Location = new System.Drawing.Point(143, 135);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 23);
            this.label9.TabIndex = 8;
            this.label9.Text = "gasAdm";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(28, 161);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Raws";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(236, 161);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(100, 23);
            this.label11.TabIndex = 10;
            this.label11.Text = "Mixes";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gasRawAdd
            // 
            this.gasRawAdd.Location = new System.Drawing.Point(28, 184);
            this.gasRawAdd.Name = "gasRawAdd";
            this.gasRawAdd.Size = new System.Drawing.Size(122, 23);
            this.gasRawAdd.TabIndex = 11;
            this.gasRawAdd.Text = "Добавлено:";
            this.gasRawAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gasRawChange
            // 
            this.gasRawChange.Location = new System.Drawing.Point(28, 207);
            this.gasRawChange.Name = "gasRawChange";
            this.gasRawChange.Size = new System.Drawing.Size(122, 23);
            this.gasRawChange.TabIndex = 12;
            this.gasRawChange.Text = "Изменено:";
            this.gasRawChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gasMixAdd
            // 
            this.gasMixAdd.Location = new System.Drawing.Point(236, 184);
            this.gasMixAdd.Name = "gasMixAdd";
            this.gasMixAdd.Size = new System.Drawing.Size(126, 23);
            this.gasMixAdd.TabIndex = 13;
            this.gasMixAdd.Text = "Добавлено:";
            this.gasMixAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // gasMixChange
            // 
            this.gasMixChange.Location = new System.Drawing.Point(236, 207);
            this.gasMixChange.Name = "gasMixChange";
            this.gasMixChange.Size = new System.Drawing.Size(126, 23);
            this.gasMixChange.TabIndex = 14;
            this.gasMixChange.Text = "Изменено:";
            this.gasMixChange.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.label16.Location = new System.Drawing.Point(50, 255);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(100, 23);
            this.label16.TabIndex = 15;
            this.label16.Text = "cyls";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cylAdd
            // 
            this.cylAdd.Location = new System.Drawing.Point(50, 278);
            this.cylAdd.Name = "cylAdd";
            this.cylAdd.Size = new System.Drawing.Size(127, 23);
            this.cylAdd.TabIndex = 16;
            this.cylAdd.Text = "Добавлено:";
            this.cylAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.label18.Location = new System.Drawing.Point(236, 255);
            this.label18.Name = "label18";
            this.label18.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label18.Size = new System.Drawing.Size(100, 23);
            this.label18.TabIndex = 17;
            this.label18.Text = "spending";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // spendingAdd
            // 
            this.spendingAdd.Location = new System.Drawing.Point(236, 278);
            this.spendingAdd.Name = "spendingAdd";
            this.spendingAdd.Size = new System.Drawing.Size(126, 23);
            this.spendingAdd.TabIndex = 18;
            this.spendingAdd.Text = "Добавлено:";
            this.spendingAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.button1.Location = new System.Drawing.Point(128, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 32);
            this.button1.TabIndex = 19;
            this.button1.Text = "ОК";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ShowChanges
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 392);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.spendingAdd);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.cylAdd);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.gasMixChange);
            this.Controls.Add(this.gasMixAdd);
            this.Controls.Add(this.gasRawChange);
            this.Controls.Add(this.gasRawAdd);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.primMixChange);
            this.Controls.Add(this.primMixAdd);
            this.Controls.Add(this.primRawChange);
            this.Controls.Add(this.primRawAdd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ShowChanges";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добавлено компонентов";
            this.Load += new System.EventHandler(this.ShowChanges_Load);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label gasMixChange;
        private System.Windows.Forms.Label spendingAdd;
        private System.Windows.Forms.Label cylAdd;
        private System.Windows.Forms.Label gasMixAdd;
        private System.Windows.Forms.Label gasRawChange;
        private System.Windows.Forms.Label gasRawAdd;
        private System.Windows.Forms.Label primMixChange;
        private System.Windows.Forms.Label primMixAdd;
        private System.Windows.Forms.Label primRawChange;
        private System.Windows.Forms.Label primRawAdd;
        private System.Windows.Forms.Button button1;
    }
}