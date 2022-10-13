using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using PGS.DMS.Controllers;
using PGS.DMS.Models;
using File = PGS.DMS.Models.File;
using PGS.UI.Groups;

namespace PGS.DMS.UI
{
    public partial class FormDocumentEdit : Form
    {
        #region EditMode

        private bool _editMode = false;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                if (_editMode != value)
                {
                    if (value && !DB.doCheckPermissionLevel("app_dms_rw"))
                        return;

                    _editMode = value;
                    ConfigureEditMode(value);
                }
            }
        }

        private void ConfigureEditMode(bool value)
        {
            btnOk.Visible = value;
            toolStrip1.Enabled = value;
            tbDocName.ReadOnly = !value;
            tbDocName.BackColor = SystemColors.Window;
            tbDocNumber.ReadOnly = !value;
            tbDocNumber.BackColor = SystemColors.Window;
        }

        private void cbDocResponsible_Enter(object sender, EventArgs e)
        {
            if (!EditMode)
            {
                cbDocResponsible.Enabled = false;
                cbDocResponsible.Enabled = true;
                labelGroupName.Focus();
            }
        }

        private void cbDocResponsible_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        #endregion
        //TODO: Зачем оно тут? Надо помнить про объектный подход.
        private readonly DocumentGroup _initialDocumentGroup;

        //TODO: Использется один раз в одной процедуре.
        /// <summary>
        /// Группа к которой относится документ.
        /// </summary>
        public DocumentGroup DocumentGroup;

        public Document Document;

        private readonly NetworkFolder _networkFolder;

        /// <summary>
        /// Добавленные файлы. | key - File, value - абсолютный путь из папки пользователя.
        /// </summary>
        private Dictionary<File, string> _addedFiles = new Dictionary<File, string>();

        /// <summary>
        /// Удалённые файлы.
        /// </summary>
        private List<File> _removedFiles = new List<File>();

        // Создание нового.
        public FormDocumentEdit(NetworkFolder networkFolder, DocumentGroup documentGroup = null)
        {
            InitializeComponent();

            Document = new Document
            {
                DateIn = DateTime.Now,
                Creator = GetUserByUsername(DB.DBUser),
                Responsible = GetUserByUsername(DB.DBUser)
            };
            if (documentGroup != null)
            {
                DocumentGroup = documentGroup;
                Document.Group = DocumentGroup;
                DocumentGroup.Documents.Add(Document);
            }

            _networkFolder = networkFolder;
        }

        // Просмотр существующего.
        public FormDocumentEdit(NetworkFolder networkFolder, Document document)
        {
            InitializeComponent();

            Document = document;
            _networkFolder = networkFolder;
            _initialDocumentGroup = document.Group;
            Text = document.Name;
        }

        private void FormDocumentEdit_Load(object sender, EventArgs e)
        {
            LoadProperties();
            UploadDocumentPanel(Document);
            ConfigureEditMode(_editMode);
            OptimizeForm();
            UpdateDgv();
        }

        private void FormDocumentEdit_Shown(object sender, EventArgs e)
        {
            // Если создаём новый документ - начинаем с добавления файла.
            if (Document.Id == null)
                btnFileAdd_Click(null, null);
        }

        private void FormDocumentEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            SavePropertirs();
        }

        /// <summary>
        /// Обновить/создать UserControlDocumentEdit в panelDocument.
        /// </summary>
        private void UploadDocumentPanel(Document document)
        {
            FillComboBoxResponsible();
            UploadInfo(Document);
        }

        #region Оптимизация элементов формы

        //TODO: Название ни очем.
        private void OptimizeForm()
        {
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                null, dgv, new object[] { true });
        }

        #endregion

        #region Работа с реестром

        /// <summary>
        /// Сохранение состояния полей формы в регистр.
        /// </summary>
        private void SavePropertirs()
        {
            // Состояние формы
            var rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\PGS\\DMS");

            if (rk != null)
            {
                var rkDe = rk.CreateSubKey("FormDocumentEdit");

                rkDe.SetValue("WindowState", WindowState.ToString());
                if (WindowState != FormWindowState.Maximized)
                {
                    rkDe.SetValue("WindowLeft", Left);
                    rkDe.SetValue("WindowTop", Top);
                    rkDe.SetValue("WindowWidth", Width);
                    rkDe.SetValue("WindowHeight", Height);
                }
            }
        }

        /// <summary>
        /// Загрузка состояния полей формы из регистра.
        /// </summary>
        private void LoadProperties()
        {
            try
            {
                var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PGS\\DMS");
                if (rk == null) return;

                var rkDe = rk.OpenSubKey("FormDocumentEdit");
                if (rkDe != null)
                {
                    // Загрузка состояния формы
                    WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState),
                        (string)rkDe.GetValue("WindowState", WindowState.ToString()));

                    Left = (int)rkDe.GetValue("WindowLeft", Left);
                    Top = (int)rkDe.GetValue("WindowTop", Top);
                    Width = (int)rkDe.GetValue("WindowWidth", Width);
                    Height = (int)rkDe.GetValue("WindowHeight", Height);
                }
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить конфигурацию\nдля некоторых элементов.",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Dgv

        /// <summary>
        /// Заполнение DataGridView файлами.
        /// </summary>
        private void UpdateDgv()
        {
            dgv.Rows.Clear();
            if (Document.Files.Count == 0)
                return;

            var startFile = Document.Files.First(x => x.ReplacedFrom == null);

            for (var i = 0; i < Document.Files.Count; i++)
            {
                var tempId = dgv.Rows.Add(
                    startFile.Id, startFile.FullName, startFile.Date.ToShortDateString(),
                    GetUserById((int)startFile.Added), startFile.Source
                );
                dgv.Rows[tempId].Tag = startFile;

                startFile = startFile.ReplacedTo;
            }

            // Раскрашиваю зелёным последний файл
            if (dgv.Rows.Count > 0) dgv.Rows[dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.LightGreen;

            dgv.ClearSelection();
        }

        // Открытие файла на двойной клик.
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.IsCurrentCellInEditMode)
                return;

            var file = dgv.Rows[e.RowIndex].Tag as File;
            if (file == null)
            {
                MessageBox.Show("Не удалось открыть файл.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var path = file.Id == null
                ? _addedFiles[file]
                : _networkFolder.CombinePath(file.Document.Group.FullPath, file.NetworkName);

            try
            {
                System.Diagnostics.Process.Start(path);
            }
            catch (Exception exception)
            {
                MessageBox.Show($"{exception.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Копирование файла в буфер обмена.
        /// </summary>
        private void CopyFileToClipboard()
        {
            System.Collections.Specialized.StringCollection replacementList = new System.Collections.Specialized.StringCollection();

            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите файл", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedFile = dgv.SelectedRows[0].Tag as File;

            var path = _addedFiles.ContainsKey(selectedFile) 
                ? _addedFiles[selectedFile] 
                : _networkFolder.CombinePath(selectedFile.Document.Group.FullPath, selectedFile.NetworkName);
            replacementList.Add(path);
            
            Clipboard.SetFileDropList(replacementList);
        }

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                btnFileRemove_Click(null, null);
            }
            if (e.KeyCode == Keys.C && e.Control)
                CopyFileToClipboard();
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var file = dgv.Rows[e.RowIndex].Tag as File;
            if (file != null)
            {
                if (e.ColumnIndex == 4)
                    file.Source = (string)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "";
                if (e.ColumnIndex == 2)
                {
                    var s_date = (string)dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value ?? "";
                    DateTime date;
                    if (DateTime.TryParse(s_date, out date))
                    {
                        file.Date = date;
                    }
                }
            }
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DateTime date;
                e.Cancel = !DateTime.TryParse((string)e.FormattedValue, out date);
            }
        }

        #endregion

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

        #region Сохранение/изменение документа

        private bool SaveDocument()
        {
            var con = DB.Connect();
            OdbcTransaction tr = null;
            try
            {
                // Создание записи в бд.
                tr = con.BeginTransaction();
                Document.Save(tr);

                foreach (var file in _removedFiles) File.DeleteFile((int)file.Id, tr);

                // Работа с файломи в сетевой папке.
                HandleRemovedFiles();
                HandleAddedFiles();
                if (_initialDocumentGroup != null &&
                    _initialDocumentGroup.FullPath != Document.Group.FullPath && Document.Files.Count > 0)
                    MoveFiles();

                tr.Commit();
            }
            catch
            {
                tr?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }

            return true;
        }

        #region Обработка изменений/добавлений "физических" файлов (На сетевом диске).

        /// <summary>
        /// Обработка удаления файлов.
        /// </summary>
        private void HandleRemovedFiles()
        {
            foreach (var file in _removedFiles) FileManager.DeleteFile(_networkFolder, file, true);

            _removedFiles.Clear();
        }

        /// <summary>
        /// Добавление новых файлов.
        /// </summary>
        private void HandleAddedFiles()
        {
            foreach (var keyValue in _addedFiles)
            {
                var pathToFileInNetworkDisk =
                    _networkFolder.CombinePath(keyValue.Key.Document.Group.FullPath,
                        keyValue.Key.NetworkName);
                FileManager.CopyFile(keyValue.Value, pathToFileInNetworkDisk);
            }

            _addedFiles.Clear();
        }

        /// <summary>
        /// Переносит файлы, если сменили группу.
        /// </summary>
        private void MoveFiles()
        {
            try
            {
                foreach (var file in Document.Files)
                    FileManager.MoveFile(
                        _networkFolder.CombinePath(_initialDocumentGroup.FullPath, file.NetworkName),
                        _networkFolder.CombinePath(Document.Group.FullPath, file.NetworkName)
                    );
            }
            catch
            {
                // Игнорирую
            }
        }

        #endregion

        #endregion

        #region Валидация

        private bool ValidateDocument()
        {
            if (Document.Group == null)
            {
                MessageBox.Show("Выберите группу!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Document.Name))
            {
                MessageBox.Show("Укажите название документа!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            // Проверяем уникальность названия данными из бд. 
            if (!CheckUniqueDocumentName())
            {
                MessageBox.Show("В группе уже присутствует документ с подобным названием!",
                    "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (Document.DateIn == null)
            {
                MessageBox.Show("Укажите дату начала действия документа!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (Document.Responsible == null)
            {
                MessageBox.Show("Выберите ответственного!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (Document.Files.Count == 0)
                return MessageBox.Show("С документов не связано ни одного файла.\n Сохранить?",
                    "", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK;

            return true;
        }
        //TODO: ООП это метод документа!
        private bool CheckUniqueDocumentName()
        {
            var query = Document.Id != null
                ? $@"
SELECT Count(*) = 0 FROM dms.docs
WHERE groupid = {Document.Group.Id.ToDBString()} AND name = {Document.Name.ToDBString()} AND id != {Document.Id.ToDBString()}
"
                : $@"
SELECT Count(*) = 0 FROM dms.docs
WHERE groupid = {Document.Group.Id.ToDBString()} AND name = {Document.Name.ToDBString()}
";

            var con = DB.Connect();

            try
            {
                var com = con.CreateCommand();
                com.CommandText = query;
                return (bool)com.ExecuteScalar();
            }
            catch
            {
                con.Disconnect();
                throw;
            }
        }

        #endregion



        #region Работа с элементами формы

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!ValidateDocument())
                return;

            try
            {
                SaveDocument();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Не удалось создать группу\n{exception}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnFileAdd_Click(object sender, EventArgs e)
        {
            var fe = new FormFileEdit(_networkFolder, Document);
            fe.EditMode = EditMode;

            if (fe.ShowDialog() != DialogResult.OK)
                return;

            // replaced
            if (Document.Files.Count != 0)
            {
                var lastFile = fe.File.ReplacedFrom = Document.Files.Last(x => x.ReplacedTo == null);
                lastFile.ReplacedTo = fe.File;
                fe.File.ReplacedFrom = lastFile;
            }

            _addedFiles.Add(fe.File, fe.AbsolutePath);
            Document.Files.Add(fe.File);

            UpdateDgv();

            if (string.IsNullOrWhiteSpace(Document.Name))
            {
                Document.Name = Path.GetFileNameWithoutExtension(fe.File.FullName);
                UploadDocumentPanel(Document);
            }
        }

        private void btnFileRemove_Click(object sender, EventArgs e)
        {
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите файл", "",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedFile = dgv.SelectedRows[0].Tag as File;

            if (MessageBox.Show($"Удалить файл: {selectedFile.FullName}",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2)
                != DialogResult.Yes)
                return;

            if (_addedFiles.ContainsKey(selectedFile))
                _addedFiles.Remove(selectedFile);
            else
                _removedFiles.Add(selectedFile);

            if (selectedFile.ReplacedFrom != null)
                selectedFile.ReplacedFrom.ReplacedTo = selectedFile.ReplacedTo;
            if (selectedFile.ReplacedTo != null)
                selectedFile.ReplacedTo.ReplacedFrom = selectedFile.ReplacedFrom;

            selectedFile.ReplacedFrom = null;
            selectedFile.ReplacedTo = null;

            Document.Files.Remove(selectedFile);

            UpdateDgv();
        }

        private void FormDocumentEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) Close();
        }

        private void dgv_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var hitTestInfo = dgv.HitTest(e.X, e.Y);
                if (hitTestInfo.ColumnIndex == -1 && hitTestInfo.RowIndex == -1) dgv.ClearSelection();
            }
        }

       

        #endregion

        // todo: Сделать поверку на актуальность (Кол-во) записей бд Users, место хранения
        /// <summary>
        /// Словарь с пользователями. | key = id, value = name
        /// </summary>
        /// <remarks>Включает в себя "пустого" пользователя с id = -1.</remarks>
        private static Dictionary<int, string> _users = GetUsers();

        /// <summary>
        /// Лист с местами хранения документа.
        /// </summary>
        private static List<Group> _places = Group.LoadGroup("common.storageplaces");

        private int? _tempPlace;

        /// <summary>
        /// Заполнение элементов формы информацией класса Document.
        /// </summary>
        private void UploadInfo(Document document)
        {
            _tempPlace = document.Place;

            tbGroupSelect.Text = document.Group == null ? "" : document.Group.Name;
            tbDocName.Text = document.Name ?? "";
            tbDocNumber.Text = document.Number;
            tbDocComment.Text = document.Comment;

            labelDocCreatorValue.Text = document.Creator != null
                ? _users[(int)document.Creator]
                : DB.DBUser;

            //Действует с
            if (document.DateIn != null)
            {
                tbDocDateIn.Value = (DateTime)document.DateIn;
                tbDocDateIn.Format = DateTimePickerFormat.Short;
            }
            else
            {
                tbDocDateIn.Format = DateTimePickerFormat.Custom;
            }

            //Действует по
            if (document.DateOut != null)
            {
                tbDocDateOut.Value = (DateTime)document.DateOut;
                tbDocDateOut.Format = DateTimePickerFormat.Short;
            }
            else
            {
                tbDocDateOut.CustomFormat = " ";
                tbDocDateOut.Format = DateTimePickerFormat.Custom;
            }

            //Дата ревизии
            if (document.DateRevision != null)
            {
                tbDocRevisionDate.Value = (DateTime)document.DateRevision;
                tbDocRevisionDate.Format = DateTimePickerFormat.Short;
            }
            else
            {
                tbDocRevisionDate.Format = DateTimePickerFormat.Custom;
            }

            //Следующая ревизия
            if (document.DateNextRevision != null)
            {
                tbDocNextRevisionDate.Value = (DateTime)document.DateNextRevision;
                tbDocNextRevisionDate.Format = DateTimePickerFormat.Short;
            }
            else
            {
                tbDocNextRevisionDate.Format = DateTimePickerFormat.Custom;
            }


            cbDocResponsible.SelectedValue = document.Responsible ?? -1;

            checkBoxIsPhysicalMedium.Checked = document.IsPhysicalMedium;
            if (document.Place != null)
            {
                cbDocPlace.Text = _places.First(x => x.ID == document.Place).Name;
                _tempPlace = document.Place;
            }
            else
            {
                cbDocPlace.Text = "";
                _tempPlace = null;
            }
        }

        private static Dictionary<int, string> GetUsers()
        {
            var users = new Dictionary<int, string>() { { -1, "" } };

            var con = DB.Connect();

            try
            {
                var com = con.CreateCommand();
                com.CommandText = "SELECT id, username FROM users.users";
                var reader = com.ExecuteReader();
                while (reader.Read()) users.Add((int)reader["id"], (string)reader["username"]);
            }
            catch
            {
                con.Disconnect();
                throw;
            }

            return users;
        }

        private void FillComboBoxResponsible()
        {
            cbDocResponsible.SelectedIndexChanged -= cbDocResponsible_SelectedIndexChanged;
            cbDocResponsible.DataSource = new BindingSource(_users, null);
            cbDocResponsible.DisplayMember = "Value";
            cbDocResponsible.ValueMember = "key";
            cbDocResponsible.SelectedIndexChanged += cbDocResponsible_SelectedIndexChanged;


        }


        #region Изменение полей документа

        private void tbGroupSelect_Click(object sender, EventArgs e)
        {
            if (!EditMode) return;

            var formSelectGroup = new FormDmsGroup();
            formSelectGroup.ShowDialog();
            if (formSelectGroup.SelectedGroupId == null)
                return;
            tbGroupSelect.Text = formSelectGroup.SelectedGroupName;

            Document.Group = DocumentGroup.LoadDocumentGroup((int)formSelectGroup.SelectedGroupId);
        }

        private void tbDocName_TextChanged(object sender, EventArgs e)
        {
            tbDocName.Text = tbDocName.Text.TrimStart();
            Document.Name = tbDocName.Text;
        }

        private void tbDocNumber_TextChanged(object sender, EventArgs e)
        {
            tbDocNumber.Text = tbDocNumber.Text.TrimEnd();
            Document.Number = tbDocNumber.Text;
        }

        private void tbDocComment_TextChanged(object sender, EventArgs e)
        {
            Document.Comment = tbDocComment.Text;
        }

        private void tbDocDateIn_ValueChanged(object sender, EventArgs e)
        {
            if (EditMode)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Short;
                Document.DateIn = ((DateTimePicker)sender).Value;
            }
            else
            {
                if (Document.DateIn != null)
                    ((DateTimePicker)sender).Value = (DateTime)Document.DateIn;
            }
        }

        private void tbDocDateIn_KeyDown(object sender, KeyEventArgs e)
        {
            if (!EditMode) return;

            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                Document.DateIn = null;
            }
        }

        private void timePickerDocDateOut_ValueChanged(object sender, EventArgs e)
        {
            if (EditMode)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Short;
                Document.DateOut = ((DateTimePicker)sender).Value;
            }
            else
            {
                if (Document.DateOut != null)
                    ((DateTimePicker)sender).Text = ((DateTime)Document.DateOut).ToShortDateString();
            }
        }

        private void timePickerDocDateOut_KeyDown(object sender, KeyEventArgs e)
        {
            if (!EditMode) return;

            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                Document.DateOut = null;
            }
        }

        private void tbDocRevisionDate_ValueChanged(object sender, EventArgs e)
        {
            if (EditMode)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Short;
                Document.DateRevision = ((DateTimePicker)sender).Value;
            }
            else
            {
                if (Document.DateRevision != null)
                    ((DateTimePicker)sender).Value = (DateTime)Document.DateRevision;
            }
        }
        private void tbDocRevisionDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (!EditMode) return;

            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                Document.DateRevision = null;
            }
        }

        private void timePickerDocDateRevision_ValueChanged(object sender, EventArgs e)
        {
            if (EditMode)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Short;
                Document.DateNextRevision = ((DateTimePicker)sender).Value;
            }
            else
            {
                if (Document.DateNextRevision != null)
                    ((DateTimePicker)sender).Text = ((DateTime)Document.DateNextRevision).ToShortDateString();
            }
        }

        private void timePickerDocDateRevision_KeyDown(object sender, KeyEventArgs e)
        {
            if (!EditMode) return;

            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                ((DateTimePicker)sender).Format = DateTimePickerFormat.Custom;
                Document.DateNextRevision = null;
            }
        }

        private void cbDocResponsible_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((int)cbDocResponsible.SelectedValue == -1)
                Document.Responsible = null;
            else
                Document.Responsible = (int)cbDocResponsible.SelectedValue;
        }

        private void checkBoxIsPhysicalMedium_CheckedChanged(object sender, EventArgs e)
        {
            if (!EditMode)
            {
                checkBoxIsPhysicalMedium.Checked = Document.IsPhysicalMedium;
                return;
            }

            Document.IsPhysicalMedium = checkBoxIsPhysicalMedium.Checked;
            cbDocPlace.Enabled = checkBoxIsPhysicalMedium.Checked;

            if (checkBoxIsPhysicalMedium.Checked)
            {
                labelDocPlace.ForeColor = SystemColors.ControlText;
                Document.Place = _tempPlace;
                cbDocPlace.Text = _tempPlace != null
                    ? _places.First(x => x.ID == _tempPlace).Name
                    : "";
            }
            else
            {
                labelDocPlace.ForeColor = SystemColors.ControlDarkDark;
                Document.Place = null;
                cbDocPlace.Text = "";
            }
        }

        private void cbDocPlace_Click(object sender, EventArgs e)
        {
            if (!EditMode) return;

            var placeSelect = new FormGroupSelect();
            placeSelect.GroupTable = "common.storageplaces";
            placeSelect.SelectedGroupID = Document.Place;
            placeSelect.ShowDialog();
            Document.Place = placeSelect.SelectedGroupID;
            _tempPlace = placeSelect.SelectedGroupID;
            cbDocPlace.Text = Document.Place != null
                ? _places.First(x => x.ID == Document.Place).Name
                : "";
            labelGroupName.Focus();
        }












        #endregion

     
    }

}