using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using PGS.DMS.Controllers;
using PGS.DMS.Models;
using PGS.DMS.Properties;
using PGS.UI.Groups;

namespace PGS.DMS.UI
{
    public partial class UserControlDocumentEdit : UserControl
    {
        /// <summary>
        /// Словарь с пользователями. | key = id, value = name
        /// </summary>
        /// <remarks>Включает в себя "пустого" пользователя с id = -1.</remarks>
        private static Dictionary<int, string> _users = GetUsers();

        /// <summary>
        /// Лист с местами хранения документа.
        /// </summary>
        private static List<Group> _places = Group.LoadGroup("common.storageplaces");

        private readonly Document _document;

        public UserControlDocumentEdit(Document document = null)
        {
            InitializeComponent();
            _document = document ?? new Document();
            UploadInfo(_document);
        }

        #region Заполнение элементов данными документа

        /// <summary>
        /// Заполнение элементов формы информацией класса Document.
        /// </summary>
        private void UploadInfo(Document document)
        {
            tbGroupSelect.Text = document.Group == null ? "" : document.Group.Name;
            tbDocName.Text = document.Name ?? "";
            tbDocNumber.Text = document.Number;

            labelDocCreatorValue.Text = document.Creator != null
                ? _users[(int)document.Creator]
                : DB.DBUser;

            if (document.DateIn != null)
            {
                timePickerDateIn.Value = ((DateTime)document.DateIn);
            }
            else
            {
                timePickerDateIn.CustomFormat = " ";
                timePickerDateIn.Format = DateTimePickerFormat.Custom;
            }

            if (document.DateOut != null)
            {
                timePickerDocDateOut.Text = ((DateTime)document.DateOut).ToShortDateString();
            }
            else
            {
                timePickerDocDateOut.CustomFormat = " ";
                timePickerDocDateOut.Format = DateTimePickerFormat.Custom;
            }

            if (document.DateRevision != null)
            {
                timePickerDocDateRevision.Value = (DateTime)document.DateRevision;
            }
            else
            {
                timePickerDocDateRevision.CustomFormat = " ";
                timePickerDocDateRevision.Format = DateTimePickerFormat.Custom;
            }

            if (document.DateNextRevision != null)
            {
                timePickerDocDateRevisionNext.Value = (DateTime)document.DateNextRevision;
            }
            else
            {
                timePickerDocDateRevisionNext.CustomFormat = " ";
                timePickerDocDateRevisionNext.Format = DateTimePickerFormat.Custom;
            }

           

            if (document.Responsible != null)
                tbDocResponsible.Text = _users[(int) document.Responsible];

            checkBoxIsPhysicalMedium.Checked = document.IsPhysicalMedium;

            if (document.Place != null)
            {
                tbDocPlace.Text = _places.First(x => x.ID == document.Place).Name;
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

        #endregion

        private void checkBoxIsPhysicalMedium_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxIsPhysicalMedium.Checked = !checkBoxIsPhysicalMedium.Checked;
        }
    }
}