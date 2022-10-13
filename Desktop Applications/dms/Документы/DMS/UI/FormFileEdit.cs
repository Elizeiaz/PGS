using System;
using System.Linq;
using System.Windows.Forms;
using PGS.DMS.Controllers;
using PGS.DMS.Models;

namespace PGS.DMS.UI
{
    /// <summary>
    /// Форма добавления/изменения документа.
    /// </summary>
    public partial class FormFileEdit : Form
    {
        #region EditMode

        private bool _editMode = false;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                BtnRemoveLink.Visible = linkfile.Links[0].LinkData != null && value;
                BtnOk.Visible = value;
                BtnCancel.Text = value ? "Отмена" : "Закрыть";
            }
        }

        #endregion

        private readonly bool _isNewFile;

        private readonly NetworkFolder _networkFolder;

        public File File { get; private set; }

        /// <summary>
        /// Абсолюный путь к файлу на локальном устройстве (Компьютере пользователя).
        /// </summary>
        public string AbsolutePath { get; private set; }

        /// <summary>
        /// Режим создание документа.
        /// </summary>
        public FormFileEdit(NetworkFolder networkFolder, Document document)
        {
            _networkFolder = networkFolder;
            InitializeComponent();

            _isNewFile = true;
            File = new File
            {
                Document = document,
                Date = DateTime.Now,
                Added = GetUserByUsername(DB.DBUser)
            };
            labelCreatorValue.Text = GetUserById((int)File.Added);
        }

        /// <summary>
        /// Режим редактирование документа.
        /// </summary>
        /// <param name="absolutePath">Абсолютный путь для нового файла (Который ещё не был перенесён на сетевой диск)</param>
        public FormFileEdit(NetworkFolder networkFolder, File file, string absolutePath = "")
        {
            InitializeComponent();

            _isNewFile = false;
            File = file;
            _networkFolder = networkFolder;
            Text = File.FullName;

            labelCreatorValue.Text = GetUserById((int)File.Added);

            dateTimePicker.Text = File.Date.ToShortDateString();
            tbSource.Text = File.Source;

            if (file.Id != null)
            {
                var path = _networkFolder.CombinePath(File.Document.Group.FullPath,
                    File.Document.Files.Last().NetworkName);
                linkfile.Links[0].LinkData = path;
                //toolTip1.SetToolTip(linkfile, path);
            }
            else
            {
                linkfile.Links[0].LinkData = absolutePath;
                //toolTip1.SetToolTip(linkfile, absolutePath);
            }

            linkfile.Text = System.IO.Path.GetFileName(File.FullName);

            BtnRemoveLink.Visible = true;
        }

        private void FormFileEdit_Load(object sender, EventArgs e)
        {
            if (!SelectFile())
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        #region Получения данных о создателе из бд

        private string GetUserById(int id)
        {
            var con = DB.Connect();
            var com = con.CreateCommand();
            com.CommandText = $"SELECT username FROM users.users WHERE id = {id}";
            try
            {
                var username = (string)com.ExecuteScalar();
                con.Disconnect();
                return username;
            }
            catch
            {
                con.Disconnect();
                throw;
            }
        }

        private int GetUserByUsername(string username)
        {
            var con = DB.Connect();
            var com = con.CreateCommand();
            com.CommandText = $"SELECT id FROM users.users WHERE username = {username.ToDBString()}";
            try
            {
                var id = (int)com.ExecuteScalar();
                con.Disconnect();
                return id;
            }
            catch
            {
                con.Disconnect();
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Валидация перед сохранением.
        /// </summary>
        private bool ValidateFile()
        {
            if (linkfile.Links[0].LinkData == null)
            {
                MessageBox.Show("Выберите файл", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        #region Выбор файлов

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var target = e.Link.LinkData as string;

            if (target == null)
                SelectFile();
            else
                System.Diagnostics.Process.Start(target);
        }

        /// <summary>
        /// Выбор файла из проводника.
        /// </summary>
        private bool SelectFile()
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var f = ofd.FileName;
                f = Unc.ResolveToUNC(f);
                AbsolutePath = f;
                linkfile.Links[0].LinkData = f;
                toolTip1.SetToolTip(linkfile, f);

                var fileName = System.IO.Path.GetFileName(f);
                linkfile.Text = fileName;
                File.FullName = fileName;

                BtnRemoveLink.Visible = true;
                return true;
            }

            return false;
        }

        private void BtnRemoveLink_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Удалить ссылку на файл?", "Удалить ссылку", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                linkfile.Links[0].LinkData = null;
                linkfile.Text = "Выбрать файл";
                BtnRemoveLink.Visible = false;
            }
        }

        #endregion

        #region Работа с элементами формы

        private void tbSource_TextChanged(object sender, EventArgs e)
        {
            tbSource.Text = tbSource.Text.TrimStart();
            File.Source = tbSource.Text;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!_isNewFile)
            {
                dateTimePicker.Value = File.Date;
                return;
            }

            File.Date = dateTimePicker.Value;
        }

        // btnCancel
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        // btnOK
        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateFile()) return;
            DialogResult = DialogResult.OK;
        }

        #endregion

    }
}