namespace PGS.DMS.UI
{
    partial class FormDmsSetting
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageForm = new System.Windows.Forms.TabPage();
            this.groupBoxRevision = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelRevisionDay = new System.Windows.Forms.Label();
            this.tbRevisionDay = new System.Windows.Forms.TextBox();
            this.cbRevisionShowAlways = new System.Windows.Forms.CheckBox();
            this.groupBoxFont = new System.Windows.Forms.GroupBox();
            this.btnSelectFont = new System.Windows.Forms.Button();
            this.groupBoxIconSize = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarHeaderIcon = new System.Windows.Forms.TrackBar();
            this.tabPageNetwork = new System.Windows.Forms.TabPage();
            this.groupBoxNetworkFolder = new System.Windows.Forms.GroupBox();
            this.btnCheckNetworkFiles = new System.Windows.Forms.Button();
            this.btnCheckNetworkFolders = new System.Windows.Forms.Button();
            this.groupBoxSuperUser = new System.Windows.Forms.GroupBox();
            this.tbSuperUserDomain = new System.Windows.Forms.TextBox();
            this.tbSuperUserPassword = new System.Windows.Forms.TextBox();
            this.tbSuperUserLogin = new System.Windows.Forms.TextBox();
            this.tbSuperUserPath = new System.Windows.Forms.TextBox();
            this.labelSuperUserDomain = new System.Windows.Forms.Label();
            this.labelSuperUserPassword = new System.Windows.Forms.Label();
            this.labelSuperUserLogin = new System.Windows.Forms.Label();
            this.labelSuperUserPath = new System.Windows.Forms.Label();
            this.btnSuperUserConnect = new System.Windows.Forms.Button();
            this.groupBoxReadOnly = new System.Windows.Forms.GroupBox();
            this.btnReadOnlyConnect = new System.Windows.Forms.Button();
            this.tbReadOnlyDomain = new System.Windows.Forms.TextBox();
            this.tbReadOnlyPassword = new System.Windows.Forms.TextBox();
            this.tbReadOnlyLogin = new System.Windows.Forms.TextBox();
            this.tbReadOnlyPath = new System.Windows.Forms.TextBox();
            this.labelReadOnlyDomain = new System.Windows.Forms.Label();
            this.labelReadOnlyPassword = new System.Windows.Forms.Label();
            this.labelReadOnlyLogin = new System.Windows.Forms.Label();
            this.labelReadOnlyPath = new System.Windows.Forms.Label();
            this.fontDialog = new System.Windows.Forms.FontDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPageForm.SuspendLayout();
            this.groupBoxRevision.SuspendLayout();
            this.groupBoxFont.SuspendLayout();
            this.groupBoxIconSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeaderIcon)).BeginInit();
            this.tabPageNetwork.SuspendLayout();
            this.groupBoxNetworkFolder.SuspendLayout();
            this.groupBoxSuperUser.SuspendLayout();
            this.groupBoxReadOnly.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageForm);
            this.tabControl1.Controls.Add(this.tabPageNetwork);
            this.tabControl1.Location = new System.Drawing.Point(5, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(493, 484);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageForm
            // 
            this.tabPageForm.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabPageForm.Controls.Add(this.groupBoxRevision);
            this.tabPageForm.Controls.Add(this.groupBoxFont);
            this.tabPageForm.Controls.Add(this.groupBoxIconSize);
            this.tabPageForm.Location = new System.Drawing.Point(4, 22);
            this.tabPageForm.Name = "tabPageForm";
            this.tabPageForm.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageForm.Size = new System.Drawing.Size(485, 458);
            this.tabPageForm.TabIndex = 0;
            this.tabPageForm.Text = "Окно";
            // 
            // groupBoxRevision
            // 
            this.groupBoxRevision.Controls.Add(this.label2);
            this.groupBoxRevision.Controls.Add(this.labelRevisionDay);
            this.groupBoxRevision.Controls.Add(this.tbRevisionDay);
            this.groupBoxRevision.Controls.Add(this.cbRevisionShowAlways);
            this.groupBoxRevision.Location = new System.Drawing.Point(6, 292);
            this.groupBoxRevision.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxRevision.Name = "groupBoxRevision";
            this.groupBoxRevision.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxRevision.Size = new System.Drawing.Size(464, 124);
            this.groupBoxRevision.TabIndex = 2;
            this.groupBoxRevision.TabStop = false;
            this.groupBoxRevision.Text = "Ревизия";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(188, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "(дней)";
            // 
            // labelRevisionDay
            // 
            this.labelRevisionDay.AutoSize = true;
            this.labelRevisionDay.Location = new System.Drawing.Point(29, 68);
            this.labelRevisionDay.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRevisionDay.Name = "labelRevisionDay";
            this.labelRevisionDay.Size = new System.Drawing.Size(105, 13);
            this.labelRevisionDay.TabIndex = 2;
            this.labelRevisionDay.Text = "Предупреждать за:";
            // 
            // tbRevisionDay
            // 
            this.tbRevisionDay.Location = new System.Drawing.Point(139, 65);
            this.tbRevisionDay.Margin = new System.Windows.Forms.Padding(2);
            this.tbRevisionDay.Name = "tbRevisionDay";
            this.tbRevisionDay.Size = new System.Drawing.Size(44, 20);
            this.tbRevisionDay.TabIndex = 1;
            this.tbRevisionDay.TextChanged += new System.EventHandler(this.tbRevisionDay_TextChanged);
            this.tbRevisionDay.Leave += new System.EventHandler(this.tbRevisionDay_Leave);
            // 
            // cbRevisionShowAlways
            // 
            this.cbRevisionShowAlways.AutoSize = true;
            this.cbRevisionShowAlways.Location = new System.Drawing.Point(34, 38);
            this.cbRevisionShowAlways.Margin = new System.Windows.Forms.Padding(2);
            this.cbRevisionShowAlways.Name = "cbRevisionShowAlways";
            this.cbRevisionShowAlways.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cbRevisionShowAlways.Size = new System.Drawing.Size(114, 17);
            this.cbRevisionShowAlways.TabIndex = 0;
            this.cbRevisionShowAlways.Text = "Выделять всегда";
            this.cbRevisionShowAlways.UseVisualStyleBackColor = true;
            this.cbRevisionShowAlways.CheckedChanged += new System.EventHandler(this.chRevisionShowAlways_CheckedChanged);
            // 
            // groupBoxFont
            // 
            this.groupBoxFont.Controls.Add(this.btnSelectFont);
            this.groupBoxFont.Location = new System.Drawing.Point(6, 168);
            this.groupBoxFont.Name = "groupBoxFont";
            this.groupBoxFont.Size = new System.Drawing.Size(464, 100);
            this.groupBoxFont.TabIndex = 1;
            this.groupBoxFont.TabStop = false;
            this.groupBoxFont.Text = "Шрифт";
            // 
            // btnSelectFont
            // 
            this.btnSelectFont.Location = new System.Drawing.Point(66, 35);
            this.btnSelectFont.Name = "btnSelectFont";
            this.btnSelectFont.Size = new System.Drawing.Size(127, 32);
            this.btnSelectFont.TabIndex = 0;
            this.btnSelectFont.Text = "Настроить шрифт";
            this.btnSelectFont.UseVisualStyleBackColor = true;
            this.btnSelectFont.Click += new System.EventHandler(this.btnSelectFont_Click);
            // 
            // groupBoxIconSize
            // 
            this.groupBoxIconSize.Controls.Add(this.label1);
            this.groupBoxIconSize.Controls.Add(this.trackBarHeaderIcon);
            this.groupBoxIconSize.Location = new System.Drawing.Point(6, 17);
            this.groupBoxIconSize.Name = "groupBoxIconSize";
            this.groupBoxIconSize.Size = new System.Drawing.Size(464, 132);
            this.groupBoxIconSize.TabIndex = 0;
            this.groupBoxIconSize.TabStop = false;
            this.groupBoxIconSize.Text = "Иконки";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Панель инструментов ";
            // 
            // trackBarHeaderIcon
            // 
            this.trackBarHeaderIcon.AutoSize = false;
            this.trackBarHeaderIcon.LargeChange = 1;
            this.trackBarHeaderIcon.Location = new System.Drawing.Point(34, 70);
            this.trackBarHeaderIcon.Maximum = 32;
            this.trackBarHeaderIcon.Minimum = 18;
            this.trackBarHeaderIcon.Name = "trackBarHeaderIcon";
            this.trackBarHeaderIcon.Size = new System.Drawing.Size(224, 36);
            this.trackBarHeaderIcon.TabIndex = 0;
            this.trackBarHeaderIcon.Value = 20;
            this.trackBarHeaderIcon.Scroll += new System.EventHandler(this.trackBarHeaderIcon_Scroll);
            // 
            // tabPageNetwork
            // 
            this.tabPageNetwork.Controls.Add(this.groupBoxNetworkFolder);
            this.tabPageNetwork.Controls.Add(this.groupBoxSuperUser);
            this.tabPageNetwork.Controls.Add(this.groupBoxReadOnly);
            this.tabPageNetwork.Location = new System.Drawing.Point(4, 22);
            this.tabPageNetwork.Name = "tabPageNetwork";
            this.tabPageNetwork.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNetwork.Size = new System.Drawing.Size(485, 458);
            this.tabPageNetwork.TabIndex = 1;
            this.tabPageNetwork.Text = "Сетевой диск";
            this.tabPageNetwork.UseVisualStyleBackColor = true;
            // 
            // groupBoxNetworkFolder
            // 
            this.groupBoxNetworkFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxNetworkFolder.Controls.Add(this.btnCheckNetworkFiles);
            this.groupBoxNetworkFolder.Controls.Add(this.btnCheckNetworkFolders);
            this.groupBoxNetworkFolder.Location = new System.Drawing.Point(7, 339);
            this.groupBoxNetworkFolder.Name = "groupBoxNetworkFolder";
            this.groupBoxNetworkFolder.Size = new System.Drawing.Size(469, 106);
            this.groupBoxNetworkFolder.TabIndex = 2;
            this.groupBoxNetworkFolder.TabStop = false;
            this.groupBoxNetworkFolder.Text = "Архитектура сетевой папки";
            // 
            // btnCheckNetworkFiles
            // 
            this.btnCheckNetworkFiles.Location = new System.Drawing.Point(252, 43);
            this.btnCheckNetworkFiles.Name = "btnCheckNetworkFiles";
            this.btnCheckNetworkFiles.Size = new System.Drawing.Size(164, 23);
            this.btnCheckNetworkFiles.TabIndex = 1;
            this.btnCheckNetworkFiles.Text = "Сравнить файлы";
            this.btnCheckNetworkFiles.UseVisualStyleBackColor = true;
            this.btnCheckNetworkFiles.Click += new System.EventHandler(this.btnCheckNetworkFiles_Click);
            // 
            // btnCheckNetworkFolders
            // 
            this.btnCheckNetworkFolders.Location = new System.Drawing.Point(57, 43);
            this.btnCheckNetworkFolders.Name = "btnCheckNetworkFolders";
            this.btnCheckNetworkFolders.Size = new System.Drawing.Size(164, 23);
            this.btnCheckNetworkFolders.TabIndex = 0;
            this.btnCheckNetworkFolders.Text = "Сравнить директории";
            this.btnCheckNetworkFolders.UseVisualStyleBackColor = true;
            this.btnCheckNetworkFolders.Click += new System.EventHandler(this.btnCheckNetworkFolders_Click);
            // 
            // groupBoxSuperUser
            // 
            this.groupBoxSuperUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSuperUser.Controls.Add(this.tbSuperUserDomain);
            this.groupBoxSuperUser.Controls.Add(this.tbSuperUserPassword);
            this.groupBoxSuperUser.Controls.Add(this.tbSuperUserLogin);
            this.groupBoxSuperUser.Controls.Add(this.tbSuperUserPath);
            this.groupBoxSuperUser.Controls.Add(this.labelSuperUserDomain);
            this.groupBoxSuperUser.Controls.Add(this.labelSuperUserPassword);
            this.groupBoxSuperUser.Controls.Add(this.labelSuperUserLogin);
            this.groupBoxSuperUser.Controls.Add(this.labelSuperUserPath);
            this.groupBoxSuperUser.Controls.Add(this.btnSuperUserConnect);
            this.groupBoxSuperUser.Location = new System.Drawing.Point(7, 176);
            this.groupBoxSuperUser.Name = "groupBoxSuperUser";
            this.groupBoxSuperUser.Size = new System.Drawing.Size(469, 151);
            this.groupBoxSuperUser.TabIndex = 1;
            this.groupBoxSuperUser.TabStop = false;
            this.groupBoxSuperUser.Text = "Подключение для записи";
            // 
            // tbSuperUserDomain
            // 
            this.tbSuperUserDomain.Location = new System.Drawing.Point(67, 97);
            this.tbSuperUserDomain.Name = "tbSuperUserDomain";
            this.tbSuperUserDomain.Size = new System.Drawing.Size(207, 20);
            this.tbSuperUserDomain.TabIndex = 8;
            this.tbSuperUserDomain.TextChanged += new System.EventHandler(this.tbSuperUserDomain_TextChanged);
            // 
            // tbSuperUserPassword
            // 
            this.tbSuperUserPassword.Location = new System.Drawing.Point(67, 71);
            this.tbSuperUserPassword.Name = "tbSuperUserPassword";
            this.tbSuperUserPassword.Size = new System.Drawing.Size(114, 20);
            this.tbSuperUserPassword.TabIndex = 7;
            this.tbSuperUserPassword.TextChanged += new System.EventHandler(this.tbSuperUserPassword_TextChanged);
            // 
            // tbSuperUserLogin
            // 
            this.tbSuperUserLogin.Location = new System.Drawing.Point(67, 45);
            this.tbSuperUserLogin.Name = "tbSuperUserLogin";
            this.tbSuperUserLogin.Size = new System.Drawing.Size(114, 20);
            this.tbSuperUserLogin.TabIndex = 6;
            this.tbSuperUserLogin.TextChanged += new System.EventHandler(this.tbSuperUserLogin_TextChanged);
            // 
            // tbSuperUserPath
            // 
            this.tbSuperUserPath.Location = new System.Drawing.Point(67, 19);
            this.tbSuperUserPath.Name = "tbSuperUserPath";
            this.tbSuperUserPath.Size = new System.Drawing.Size(390, 20);
            this.tbSuperUserPath.TabIndex = 5;
            this.tbSuperUserPath.TextChanged += new System.EventHandler(this.tbSuperUserPath_TextChanged);
            // 
            // labelSuperUserDomain
            // 
            this.labelSuperUserDomain.AutoSize = true;
            this.labelSuperUserDomain.Location = new System.Drawing.Point(14, 100);
            this.labelSuperUserDomain.Name = "labelSuperUserDomain";
            this.labelSuperUserDomain.Size = new System.Drawing.Size(42, 13);
            this.labelSuperUserDomain.TabIndex = 4;
            this.labelSuperUserDomain.Text = "Домен";
            // 
            // labelSuperUserPassword
            // 
            this.labelSuperUserPassword.AutoSize = true;
            this.labelSuperUserPassword.Location = new System.Drawing.Point(14, 74);
            this.labelSuperUserPassword.Name = "labelSuperUserPassword";
            this.labelSuperUserPassword.Size = new System.Drawing.Size(45, 13);
            this.labelSuperUserPassword.TabIndex = 3;
            this.labelSuperUserPassword.Text = "Пароль";
            // 
            // labelSuperUserLogin
            // 
            this.labelSuperUserLogin.AutoSize = true;
            this.labelSuperUserLogin.Location = new System.Drawing.Point(14, 48);
            this.labelSuperUserLogin.Name = "labelSuperUserLogin";
            this.labelSuperUserLogin.Size = new System.Drawing.Size(38, 13);
            this.labelSuperUserLogin.TabIndex = 2;
            this.labelSuperUserLogin.Text = "Логин";
            // 
            // labelSuperUserPath
            // 
            this.labelSuperUserPath.AutoSize = true;
            this.labelSuperUserPath.Location = new System.Drawing.Point(14, 22);
            this.labelSuperUserPath.Name = "labelSuperUserPath";
            this.labelSuperUserPath.Size = new System.Drawing.Size(31, 13);
            this.labelSuperUserPath.TabIndex = 1;
            this.labelSuperUserPath.Text = "Путь";
            // 
            // btnSuperUserConnect
            // 
            this.btnSuperUserConnect.Location = new System.Drawing.Point(353, 117);
            this.btnSuperUserConnect.Name = "btnSuperUserConnect";
            this.btnSuperUserConnect.Size = new System.Drawing.Size(103, 23);
            this.btnSuperUserConnect.TabIndex = 0;
            this.btnSuperUserConnect.Text = "Подключить";
            this.btnSuperUserConnect.UseVisualStyleBackColor = true;
            this.btnSuperUserConnect.Click += new System.EventHandler(this.btnSuperUserConnect_Click);
            // 
            // groupBoxReadOnly
            // 
            this.groupBoxReadOnly.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxReadOnly.Controls.Add(this.btnReadOnlyConnect);
            this.groupBoxReadOnly.Controls.Add(this.tbReadOnlyDomain);
            this.groupBoxReadOnly.Controls.Add(this.tbReadOnlyPassword);
            this.groupBoxReadOnly.Controls.Add(this.tbReadOnlyLogin);
            this.groupBoxReadOnly.Controls.Add(this.tbReadOnlyPath);
            this.groupBoxReadOnly.Controls.Add(this.labelReadOnlyDomain);
            this.groupBoxReadOnly.Controls.Add(this.labelReadOnlyPassword);
            this.groupBoxReadOnly.Controls.Add(this.labelReadOnlyLogin);
            this.groupBoxReadOnly.Controls.Add(this.labelReadOnlyPath);
            this.groupBoxReadOnly.Location = new System.Drawing.Point(7, 13);
            this.groupBoxReadOnly.Name = "groupBoxReadOnly";
            this.groupBoxReadOnly.Size = new System.Drawing.Size(469, 151);
            this.groupBoxReadOnly.TabIndex = 0;
            this.groupBoxReadOnly.TabStop = false;
            this.groupBoxReadOnly.Text = "Покдлючение для чтения";
            // 
            // btnReadOnlyConnect
            // 
            this.btnReadOnlyConnect.Location = new System.Drawing.Point(353, 117);
            this.btnReadOnlyConnect.Name = "btnReadOnlyConnect";
            this.btnReadOnlyConnect.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnReadOnlyConnect.Size = new System.Drawing.Size(103, 23);
            this.btnReadOnlyConnect.TabIndex = 8;
            this.btnReadOnlyConnect.Text = "Подключить";
            this.btnReadOnlyConnect.UseVisualStyleBackColor = true;
            this.btnReadOnlyConnect.Click += new System.EventHandler(this.btnReadOnlyConnect_Click);
            // 
            // tbReadOnlyDomain
            // 
            this.tbReadOnlyDomain.Location = new System.Drawing.Point(69, 97);
            this.tbReadOnlyDomain.Name = "tbReadOnlyDomain";
            this.tbReadOnlyDomain.Size = new System.Drawing.Size(207, 20);
            this.tbReadOnlyDomain.TabIndex = 7;
            this.tbReadOnlyDomain.TextChanged += new System.EventHandler(this.tbReadOnlyDomain_TextChanged);
            // 
            // tbReadOnlyPassword
            // 
            this.tbReadOnlyPassword.Location = new System.Drawing.Point(69, 71);
            this.tbReadOnlyPassword.Name = "tbReadOnlyPassword";
            this.tbReadOnlyPassword.Size = new System.Drawing.Size(114, 20);
            this.tbReadOnlyPassword.TabIndex = 6;
            this.tbReadOnlyPassword.TextChanged += new System.EventHandler(this.tbReadOnlyPassword_TextChanged);
            // 
            // tbReadOnlyLogin
            // 
            this.tbReadOnlyLogin.Location = new System.Drawing.Point(69, 45);
            this.tbReadOnlyLogin.Name = "tbReadOnlyLogin";
            this.tbReadOnlyLogin.Size = new System.Drawing.Size(114, 20);
            this.tbReadOnlyLogin.TabIndex = 5;
            this.tbReadOnlyLogin.TextChanged += new System.EventHandler(this.tbReadOnlyLogin_TextChanged);
            // 
            // tbReadOnlyPath
            // 
            this.tbReadOnlyPath.Location = new System.Drawing.Point(69, 19);
            this.tbReadOnlyPath.Name = "tbReadOnlyPath";
            this.tbReadOnlyPath.Size = new System.Drawing.Size(390, 20);
            this.tbReadOnlyPath.TabIndex = 4;
            this.tbReadOnlyPath.TextChanged += new System.EventHandler(this.tbReadOnlyPath_TextChanged);
            // 
            // labelReadOnlyDomain
            // 
            this.labelReadOnlyDomain.AutoSize = true;
            this.labelReadOnlyDomain.Location = new System.Drawing.Point(16, 100);
            this.labelReadOnlyDomain.Name = "labelReadOnlyDomain";
            this.labelReadOnlyDomain.Size = new System.Drawing.Size(42, 13);
            this.labelReadOnlyDomain.TabIndex = 3;
            this.labelReadOnlyDomain.Text = "Домен";
            // 
            // labelReadOnlyPassword
            // 
            this.labelReadOnlyPassword.AutoSize = true;
            this.labelReadOnlyPassword.Location = new System.Drawing.Point(16, 74);
            this.labelReadOnlyPassword.Name = "labelReadOnlyPassword";
            this.labelReadOnlyPassword.Size = new System.Drawing.Size(45, 13);
            this.labelReadOnlyPassword.TabIndex = 2;
            this.labelReadOnlyPassword.Text = "Пароль";
            // 
            // labelReadOnlyLogin
            // 
            this.labelReadOnlyLogin.AutoSize = true;
            this.labelReadOnlyLogin.Location = new System.Drawing.Point(16, 48);
            this.labelReadOnlyLogin.Name = "labelReadOnlyLogin";
            this.labelReadOnlyLogin.Size = new System.Drawing.Size(38, 13);
            this.labelReadOnlyLogin.TabIndex = 1;
            this.labelReadOnlyLogin.Text = "Логин";
            // 
            // labelReadOnlyPath
            // 
            this.labelReadOnlyPath.AutoSize = true;
            this.labelReadOnlyPath.Location = new System.Drawing.Point(16, 22);
            this.labelReadOnlyPath.Name = "labelReadOnlyPath";
            this.labelReadOnlyPath.Size = new System.Drawing.Size(31, 13);
            this.labelReadOnlyPath.TabIndex = 0;
            this.labelReadOnlyPath.Text = "Путь";
            // 
            // fontDialog
            // 
            this.fontDialog.MaxSize = 12;
            // 
            // FormDmsSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 482);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(506, 515);
            this.Name = "FormDmsSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            this.tabControl1.ResumeLayout(false);
            this.tabPageForm.ResumeLayout(false);
            this.groupBoxRevision.ResumeLayout(false);
            this.groupBoxRevision.PerformLayout();
            this.groupBoxFont.ResumeLayout(false);
            this.groupBoxIconSize.ResumeLayout(false);
            this.groupBoxIconSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHeaderIcon)).EndInit();
            this.tabPageNetwork.ResumeLayout(false);
            this.groupBoxNetworkFolder.ResumeLayout(false);
            this.groupBoxSuperUser.ResumeLayout(false);
            this.groupBoxSuperUser.PerformLayout();
            this.groupBoxReadOnly.ResumeLayout(false);
            this.groupBoxReadOnly.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageForm;
        private System.Windows.Forms.TabPage tabPageNetwork;
        private System.Windows.Forms.GroupBox groupBoxSuperUser;
        private System.Windows.Forms.GroupBox groupBoxReadOnly;
        private System.Windows.Forms.Label labelReadOnlyDomain;
        private System.Windows.Forms.Label labelReadOnlyPassword;
        private System.Windows.Forms.Label labelReadOnlyLogin;
        private System.Windows.Forms.Label labelReadOnlyPath;
        private System.Windows.Forms.TextBox tbReadOnlyDomain;
        private System.Windows.Forms.TextBox tbReadOnlyPassword;
        private System.Windows.Forms.TextBox tbReadOnlyLogin;
        private System.Windows.Forms.TextBox tbReadOnlyPath;
        private System.Windows.Forms.Button btnReadOnlyConnect;
        private System.Windows.Forms.Button btnSuperUserConnect;
        private System.Windows.Forms.TextBox tbSuperUserDomain;
        private System.Windows.Forms.TextBox tbSuperUserPassword;
        private System.Windows.Forms.TextBox tbSuperUserLogin;
        private System.Windows.Forms.TextBox tbSuperUserPath;
        private System.Windows.Forms.Label labelSuperUserDomain;
        private System.Windows.Forms.Label labelSuperUserPassword;
        private System.Windows.Forms.Label labelSuperUserLogin;
        private System.Windows.Forms.Label labelSuperUserPath;
        private System.Windows.Forms.GroupBox groupBoxNetworkFolder;
        private System.Windows.Forms.Button btnCheckNetworkFiles;
        private System.Windows.Forms.Button btnCheckNetworkFolders;
        private System.Windows.Forms.GroupBox groupBoxFont;
        private System.Windows.Forms.GroupBox groupBoxIconSize;
        private System.Windows.Forms.Button btnSelectFont;
        private System.Windows.Forms.FontDialog fontDialog;
        private System.Windows.Forms.TrackBar trackBarHeaderIcon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxRevision;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelRevisionDay;
        private System.Windows.Forms.TextBox tbRevisionDay;
        private System.Windows.Forms.CheckBox cbRevisionShowAlways;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}