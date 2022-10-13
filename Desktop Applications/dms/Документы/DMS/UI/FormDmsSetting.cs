using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using PGS.DMS.Controllers;
using PGS.DMS.Models;

namespace PGS.DMS.UI
{
    /// <summary>
    /// Форма настроек параметров основной формы DMS и параметров подключения к сетевому диску.
    /// </summary>
    public partial class FormDmsSetting : Form
    {
        private bool _editMode = false;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                tabPageNetwork.Enabled = value;
                if (value)
                {
                    tbReadOnlyPassword.UseSystemPasswordChar = false;
                    tbSuperUserPassword.UseSystemPasswordChar = false;
                }
                else
                {
                    tbReadOnlyPassword.UseSystemPasswordChar = true;
                    tbSuperUserPassword.UseSystemPasswordChar = true;
                }
            }
        }

        private FormDmsBrowser _formDmsBrowser;

        public FormDmsSetting(FormDmsBrowser formDmsBrowser)
        {
            InitializeComponent();
            _formDmsBrowser = formDmsBrowser;
            UploadNetworkFields();

            trackBarHeaderIcon.Value = _formDmsBrowser.toolStrip1.ImageScalingSize.Height;
            cbRevisionShowAlways.Checked = _formDmsBrowser.IsRevisionHighlight;
            tbRevisionDay.Text = _formDmsBrowser.RevisionModeDay.ToString();
        }

        /// <summary>
        /// Заполнение полей данных подключения к сетевому диску.
        /// </summary>
        private void UploadNetworkFields()
        {
            tbReadOnlyPath.Text = FormDmsBrowser.UncReadOnlyPath;
            tbReadOnlyLogin.Text = FormDmsBrowser.ReadOnlyCredential.UserName;
            tbReadOnlyPassword.Text = FormDmsBrowser.ReadOnlyCredential.Password;
            tbReadOnlyDomain.Text = FormDmsBrowser.ReadOnlyCredential.Domain;

            tbSuperUserPath.Text = FormDmsBrowser.UncSuperUserPath;
            tbSuperUserLogin.Text = FormDmsBrowser.SuperUserCredential.UserName;
            tbSuperUserPassword.Text = FormDmsBrowser.SuperUserCredential.Password;
            tbSuperUserDomain.Text = FormDmsBrowser.SuperUserCredential.Domain;
        }

        #region Кнопки "Подключить"

        // Кнопка подключить для ReadOnly (Сохраняет в бд).
        private void btnReadOnlyConnect_Click(object sender, EventArgs e)
        {
            var credentional =
                new NetworkCredential(
                    FormDmsBrowser.ReadOnlyCredential.UserName,
                    FormDmsBrowser.ReadOnlyCredential.Password,
                    FormDmsBrowser.ReadOnlyCredential.Domain
                );
            try
            {
                InsertUpdate(GetInsertUpdateReadOnlyDictionary());
                var code = NetworkDriver.Connect(FormDmsBrowser.UncReadOnlyPath, credentional);
                switch (code)
                {
                    case 0:
                        MessageBox.Show($"Успешное подключение!\n{FormDmsBrowser.UncReadOnlyPath}",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _formDmsBrowser.ReadOnlyFolder = new NetworkFolder(FormDmsBrowser.UncReadOnlyPath);
                        break;
                    case 1219:
                        if (MessageBox.Show("Имеется подключение под другим пользователем. Переподключить?",
                                "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                            return;
                        NetworkDriver.Disconnect(FormDmsBrowser.UncReadOnlyPath);
                        if (NetworkDriver.Connect(FormDmsBrowser.UncReadOnlyPath, credentional) != 0)
                        {
                            MessageBox.Show("Переподключение не удалось",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        _formDmsBrowser.ReadOnlyFolder = new NetworkFolder(FormDmsBrowser.UncReadOnlyPath);
                        break;
                    default:
                        throw new Win32Exception(code);
                }
            }
            catch (Win32Exception exception)
            {
                MessageBox.Show($"Подключение не удалось!\n{exception.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Кнопка подключить для SuperUser (Сохраняет в бд).
        private void btnSuperUserConnect_Click(object sender, EventArgs e)
        {
            var credentional =
                new NetworkCredential(
                    FormDmsBrowser.SuperUserCredential.UserName,
                    FormDmsBrowser.SuperUserCredential.Password,
                    FormDmsBrowser.SuperUserCredential.Domain
                );

            try
            {
                InsertUpdate(GetInsertUpdateSuperUserDictionary());
                var code = NetworkDriver.Connect(FormDmsBrowser.UncSuperUserPath, credentional);
                switch (code)
                {
                    case 0:
                        MessageBox.Show($"Успешное подключение!\n{FormDmsBrowser.UncSuperUserPath}",
                            "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _formDmsBrowser.SuperUserFolder = new NetworkFolder(FormDmsBrowser.UncSuperUserPath);
                        break;
                    case 1219:
                        if (MessageBox.Show("Имеется подключение под другим пользователем. Переподключить?",
                                "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) != DialogResult.Yes)
                            return;
                        NetworkDriver.Disconnect(FormDmsBrowser.UncSuperUserPath);
                        if (NetworkDriver.Connect(FormDmsBrowser.UncSuperUserPath, credentional) != 0)
                        {
                            MessageBox.Show("Переподключение не удалось",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        _formDmsBrowser.ReadOnlyFolder = new NetworkFolder(FormDmsBrowser.UncSuperUserPath);
                        break;
                    default:
                        throw new Win32Exception(code);
                }
            }
            catch (Win32Exception exception)
            {
                MessageBox.Show($"Подключение не удалось!\n{exception.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Заносим в БД

        private Dictionary<string, string> GetInsertUpdateReadOnlyDictionary()
        {
            return new Dictionary<string, string>()
            {
                // ReadOnly config
                { "Server_Fetch", FormDmsBrowser.UncReadOnlyPath },
                { "User", FormDmsBrowser.ReadOnlyCredential.UserName },
                { "User_Password", FormDmsBrowser.ReadOnlyCredential.Password },
                { "User_Domain", FormDmsBrowser.ReadOnlyCredential.Domain }
            };
        }

        private Dictionary<string, string> GetInsertUpdateSuperUserDictionary()
        {
            return new Dictionary<string, string>()
            {
                // SuperUser config
                { "Server_Push", FormDmsBrowser.UncSuperUserPath },
                { "Admin", FormDmsBrowser.SuperUserCredential.UserName },
                { "Password", FormDmsBrowser.SuperUserCredential.Password },
                { "Domain", FormDmsBrowser.SuperUserCredential.Domain }
            };
        }

        private void InsertUpdate(Dictionary<string, string> dictionary)
        {
            var con = DB.Connect();

            try
            {
                var com = con.CreateCommand();

                foreach (var keyValue in dictionary)
                {
                    com.CommandText = $@"
INSERT INTO dms.dms_config (name, value)
VALUES ({keyValue.Key.ToDBString()}, {keyValue.Value.ToDBString()})
ON CONFLICT (name) DO UPDATE
SET value = {keyValue.Value.ToDBString()}
";

                    com.ExecuteNonQuery();
                }
            }
            finally
            {
                con.Disconnect();
            }
        }

        #endregion

        #endregion

        #region Текст боксы

        private void tbReadOnlyPath_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.UncReadOnlyPath = (sender as TextBox).Text;
        }

        private void tbReadOnlyLogin_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.ReadOnlyCredential.UserName = (sender as TextBox).Text;
        }

        private void tbReadOnlyPassword_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.ReadOnlyCredential.Password = (sender as TextBox).Text;
        }

        private void tbReadOnlyDomain_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.ReadOnlyCredential.Domain = (sender as TextBox).Text;
        }

        private void tbSuperUserPath_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.UncSuperUserPath = (sender as TextBox).Text;
        }

        private void tbSuperUserLogin_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.SuperUserCredential.UserName = (sender as TextBox).Text;
        }

        private void tbSuperUserPassword_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.SuperUserCredential.Password = (sender as TextBox).Text;
        }

        private void tbSuperUserDomain_TextChanged(object sender, EventArgs e)
        {
            FormDmsBrowser.SuperUserCredential.Domain = (sender as TextBox).Text;
        }

        #endregion

        #region Кнопки проверки сетевой директории

        private void btnCheckNetworkFolders_Click(object sender, EventArgs e)
        {
            if (_formDmsBrowser.SuperUserFolder == null)
            {
                MessageBox.Show("Подключите сетевой диск с возможностью записи.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var equalDict = new Dictionary<string, NetworkFolder.ChangeType>();

            var documentGroup = new DocumentGroup(0, null, "",
                new List<Document>(), DocumentGroup.LoadDocumentGroups());

            _formDmsBrowser.SuperUserFolder.CompareNetworkFolders(equalDict, documentGroup);

            // Формируем сводку
            var message = new StringBuilder();
            if (equalDict.Count == 0)
                message.Append("Проверка пройдена успешно!");

            if (equalDict.ContainsValue(NetworkFolder.ChangeType.Remove))
            {
                message.AppendLine("Директория есть на сетевом диске, но нет в бд:");
                foreach (var keyValue in equalDict.Where(x => x.Value == NetworkFolder.ChangeType.Remove))
                    message.AppendLine($"  -{keyValue.Key}");

                message.Append("\n");
            }

            if (equalDict.ContainsValue(NetworkFolder.ChangeType.Add))
            {
                message.AppendLine("Есть запись в бд, но нет на диске:");
                foreach (var keyValue in equalDict.Where(x => x.Value == NetworkFolder.ChangeType.Add))
                    message.AppendLine($"  +{keyValue.Key}");
            }

            MessageBox.Show(message.ToString(), "Сводка", MessageBoxButtons.OK);

            if (equalDict.Count != 0 &&
                MessageBox.Show("Применить изменения?", "", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                DialogResult.Yes)
                try
                {
                    MessageBox.Show("Функционал заблокирован на уровне кода!\n" +
                                    "Создайте резервную копию и раскомментируйте функцию.");
                    //_formDmsBrowser.SuperUserFolder.RecreateNetworkFolders(equalDict);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Во время изменения произошла ошибка\n{exception}", "", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
        }

        private void btnCheckNetworkFiles_Click(object sender, EventArgs e)
        {
            if (_formDmsBrowser.SuperUserFolder == null)
            {
                MessageBox.Show("Подключите сетевой диск с возможностью записи.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var equalDict = new Dictionary<string, NetworkFolder.ChangeType>();

            var documentGroup = new DocumentGroup(0, null, "",
                new List<Document>(), DocumentGroup.LoadDocumentGroups());

            _formDmsBrowser.SuperUserFolder.CompareNetworkFiles(equalDict, documentGroup);

            // Формируем сводку
            var message = new StringBuilder();
            if (equalDict.Count == 0)
                message.Append("Проверка пройдена успешно!");

            if (equalDict.ContainsValue(NetworkFolder.ChangeType.Add))
            {
                message.AppendLine("Файл есть на сетевом диске, но нет в бд:");
                foreach (var keyValue in equalDict.Where(x => x.Value == NetworkFolder.ChangeType.Add))
                    message.AppendLine($"  +{keyValue.Key}");
                message.Append("\n");
            }

            if (equalDict.ContainsValue(NetworkFolder.ChangeType.Remove))
            {
                message.AppendLine("Есть запись в бд, но нет на диске:");
                foreach (var keyValue in equalDict.Where(x => x.Value == NetworkFolder.ChangeType.Remove))
                    message.AppendLine($" -{keyValue.Key}");

                message.AppendLine("(Ошибки такого рода обрабатываются вручную)");
            }

            MessageBox.Show(message.ToString(), "Сводка", MessageBoxButtons.OK);

            if (equalDict.Count(x => x.Value == NetworkFolder.ChangeType.Add) != 0 &&
                MessageBox.Show("Применить изменения?", "", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) ==
                DialogResult.Yes)
                try
                {
                    MessageBox.Show("Функционал заблокирован на уровне кода!\n" +
                                    "Создайте резервную копию и раскомментируйте функцию.");
                    //_formDmsBrowser.SuperUserFolder.RecreateNetworkFiles(equalDict);
                }
                catch (Exception exception)
                {
                    MessageBox.Show($"Во время изменения произошла ошибка\n{exception}", "", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
        }

        #endregion

        #region Настройка основной формы

        #region Иконки

        private void trackBarHeaderIcon_Scroll(object sender, EventArgs e)
        {
            _formDmsBrowser.SetToolStripImageSize(trackBarHeaderIcon.Value);
        }


        #endregion

        #region Шрифты

        private void btnSelectFont_Click(object sender, EventArgs e)
        {
            fontDialog.Font = _formDmsBrowser.Font;
            fontDialog.ShowDialog();
            Font = fontDialog.Font;
            _formDmsBrowser.SetFont(fontDialog.Font);
        }

        /// <summary>
        /// Изменение шрифта в форме.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="font"></param>
        /// <param name="recursive">Изменит шрифт во всех Control, принадлежащих форме.</param>
        private static void ChangeFont(Form form, Font font, bool recursive = false)
        {
            form.Font = font;
            
            if (!recursive) return;

            foreach (Control control in form.Controls)
            {
                RecursiveChangeFont(control, font);
            }

            (form as FormDmsBrowser)?.RefreshForm(false);
        }

        private static void RecursiveChangeFont(Control control, Font font)
        {
            control.Font = font;
            if (control.Controls.Count > 0)
            {
                foreach (Control subcontrol in control.Controls)
                {
                    RecursiveChangeFont(subcontrol, font);
                }
            }
        }

        #endregion

        #endregion

        #region Настройка функции "Ревизия"

        private void chRevisionShowAlways_CheckedChanged(object sender, EventArgs e)
        {
            _formDmsBrowser.IsRevisionHighlight = cbRevisionShowAlways.Checked;
            _formDmsBrowser.RefreshForm(false);
        }

        private void tbRevisionDay_TextChanged(object sender, EventArgs e)
        {
            int dayCount;

            if (string.IsNullOrWhiteSpace(tbRevisionDay.Text))
            {
                _formDmsBrowser.RevisionModeDay = 30;
                return;
            }

            if (!int.TryParse(tbRevisionDay.Text, out dayCount))
            {
                MessageBox.Show("Не удалось распознать число");
                return;
            }

            _formDmsBrowser.RevisionModeDay = dayCount;
        }

        private void tbRevisionDay_Leave(object sender, EventArgs e)
        {
            int dayCount;

            if (!int.TryParse(tbRevisionDay.Text, out dayCount))
            {
                MessageBox.Show("Не удалось распознать число");
                return;
            }
        }

        #endregion

    }
}