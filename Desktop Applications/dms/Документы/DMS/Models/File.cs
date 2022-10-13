using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;

namespace PGS.DMS.Models
{
    /// <summary>
    /// Модель файла. Обладает методами взаимодействия с бд.
    /// </summary>
    public class File
    {
        private bool _isSaved = true;

        public bool IsSaved
        {
            get { return _isSaved; }
            private set { _isSaved = value; }
        }

        public event Action Changed;

        private int? _id;

        /// <summary>
        /// Id файла в бд.
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                _id = value;
                Changed?.Invoke();
            }
        }

        public Document _document;

        /// <summary>
        /// Id документа.
        /// </summary>
        public Document Document
        {
            get { return _document; }
            set
            {
                _document = value;
                Changed?.Invoke();
            }
        }

        private string _fullName;

        /// <summary>
        /// Полное имя (с расширением). | "filename.doc"
        /// </summary>
        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                Changed?.Invoke();
            }
        }

        private int? _replacedToId;

        /// <summary>
        /// Id новой версии файла (Если она есть, нет - null).
        /// </summary>
        /// <remarks>Если null => это самая свежая версия документа.</remarks>
        private int? ReplacedToId
        {
            get { return _replacedToId; }
            set
            {
                _replacedToId = value;
                Changed?.Invoke();
            }
        }

        private File _replacedTo;

        /// <summary>
        /// Новая версия файла (Если она есть, нет - null).
        /// </summary>
        /// <remarks>Если null => это самая свежая версия документа.</remarks>
        public File ReplacedTo
        {
            get { return _replacedTo; }
            set
            {
                _replacedTo = value;
                Changed?.Invoke();
            }
        }

        private File _replacedFrom;

        /// <summary>
        /// Предшествующая версия файла.
        /// </summary>
        public File ReplacedFrom
        {
            get { return _replacedFrom; }
            set
            {
                _replacedFrom = value;
                Changed?.Invoke();
            }
        }

        private DateTime _date;

        /// <summary>
        /// Дата добавления.
        /// </summary>
        public DateTime Date
        {
            get { return _date;}
            set
            {
                _date = value;
                Changed?.Invoke();
            }
        }

        private int? _added;

        /// <summary>
        /// Id пользователя, который добавил файл.
        /// </summary>
        public int? Added
        {
            get { return _added; }
            set
            {
                _added = value;
                Changed?.Invoke();
            }
        }

        private string _source;

        /// <summary>
        /// Источник (Откуда был взят/скачен) файл.
        /// </summary>
        public string Source
        {
            get { return _source; }
            set
            {
                _source = value;
                Changed?.Invoke();
            }
        }

        /// <summary>
        /// Формат файла. | "doc"
        /// </summary>
        public string Format
        {
            get
            {
                var splited = FullName.Split('.');
                return splited.Length == 1 ? "" : splited.Last(); 
            }
        }

        /// <summary>
        /// Название файла, под которым он хранится на сетевом диске. | "id__fileName.format"
        /// </summary>
        public string NetworkName
        {
            get
            {
                if (Id == null)
                    throw new NullReferenceException();
                return $"{Id}__{FullName}";
            }
        }

        /// <summary>
        /// Новый файл.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="fullName">Полное имя файла (С форматом). | "fileName.format"</param>
        public File()
        {
            Changed += () => IsSaved = false;
        }

        /// <summary>
        /// Существующий файл.
        /// </summary>
        public File(int id, Document document)
        {
            Changed += () => IsSaved = false;

            Id = id;
            Document = document;
        }

        ///// <summary>
        ///// Существующий файл.
        ///// </summary>
        //public File(int id, Document document, string name, int? replacedToId = null)
        //{
        //    Changed += () => IsSaved = false;

        //    Id = id;
        //    Document = document;
        //    FullName = name;
        //    Format = name;
        //    ReplacedToId = replacedToId;
        //}

        /// <summary>
        /// Созданий новой записи/сохранение изменений.
        /// </summary>
        public void Save()
        {
            if (Id == null)
                Id = InsertFile(this);
            else
                UpdateFile(this);

            IsSaved = true;
        }

        /// <summary>
        /// Созданий новой записи/сохранение изменений.
        /// </summary>
        public void Save(OdbcTransaction transaction)
        {
            if (Id == null)
                Id = InsertFile(this, transaction);
            else
                UpdateFile(this, transaction);

            IsSaved = true;
        }

        #region Загрузка/изменение/удаление информации о файле данных из бд

        #region LoadDocument

        /// <summary>
        /// Подгружает все файлы, отнощиеся к документу.
        /// </summary>
        /// <param name="documentId">id документа</param>
        public static List<File> LoadFiles(int documentId)
        {
            var con = DB.Connect();

            try
            {
                return LoadFiles(documentId, con);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Подгружает все файлы, отнощиеся к документу.
        /// </summary>
        /// <param name="documentId">id документа</param>
        /// <param name="con"></param>
        public static List<File> LoadFiles(int documentId, OdbcConnection con)
        {
            var files = new List<File>();

            var query = $@"
SELECT id, doc, original_name as name, replaced_to, date, added, source
FROM dms.files
where doc = {documentId}
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var document = Models.Document.LoadDocument(documentId);

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var id = (int)reader["id"];
                var file = new File(id, document)
                {
                    FullName = (string)reader["name"],
                    ReplacedToId = reader["replaced_to"] is DBNull ? null : (int?)reader["replaced_to"],
                    Date = DateTime.Parse(reader["date"].ToString()),
                    Added = (int)reader["added"],
                    Source = reader["source"] != DBNull.Value ? (string)reader["source"].ToString() : ""
                };

                files.Add(file);
            }

            BindReplaced(files);

            return files;
        }

        /// <summary>
        /// Подгружает все файлы, отнощиеся к документу.
        /// </summary>
        public static List<File> LoadFiles(Document document)
        {
            var con = DB.Connect();

            try
            {
                return LoadFiles(document, con);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Подгружает все файлы, отнощиеся к документу.
        /// </summary>
        public static List<File> LoadFiles(Document document, OdbcConnection con)
        {
            var files = new List<File>();

            var query = $@"
SELECT id, doc, original_name as name, replaced_to, date, added, source
FROM dms.files
where doc = {document.Id}
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var id = (int)reader["id"];
                var file = new File(id, document)
                {
                    FullName = (string)reader["name"],
                    ReplacedToId = reader["replaced_to"] is DBNull ? null : (int?)reader["replaced_to"],
                    Date = DateTime.Parse(reader["date"].ToString()),
                    Added = (int)reader["added"],
                    Source = reader["source"] != DBNull.Value ? (string)reader["source"].ToString() : ""
                };

                files.Add(file);
            }

            BindReplaced(files);

            return files;
        }

        /// <summary>
        /// Создание цепи новых-заменённых файлов.
        /// </summary>
        private static List<File> BindReplaced(List<File> files)
        {
            foreach (var file in files)
            {
                var enumerable = files.Where(x => x.Id == file.ReplacedToId);
                file.ReplacedTo = enumerable.Count() == 0 
                    ? null 
                    : enumerable.First();
            }

            foreach (var file in files)
            {
                var enumerable = files.Where(x => x.ReplacedToId == file.Id);
                file.ReplacedFrom = enumerable.Count() == 0
                    ? null
                    : enumerable.First();
            }

            return files;
        }

        #endregion

        #region Insert

        /// <summary>
        /// Создание новой записи в бд.
        /// </summary>
        private int? InsertFile(File file)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;
            try
            {
                transaction = con.BeginTransaction();
                var id = InsertFile(file, transaction);
                transaction.Commit();
                return id;
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Создание новой записи в бд.
        /// </summary>
        /// <returns>id созданной записи</returns>
        private int InsertFile(File file, OdbcTransaction transaction)
        {
            var query = $@"
INSERT INTO dms.files 
(doc, replaced_to, original_name, date, added, source)
VALUES
({file.Document.Id.ToDBString()}, {(file.ReplacedTo == null ? "".ToDBString() : file.ReplacedTo.Id.ToDBString())},
{file.FullName.ToDBString()}, {DB.ToDBString(file.Date)}, {file.Added.ToDBString()}, {file.Source.ToDBString()})
RETURNING id
";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            var id = (int)command.ExecuteScalar();
            file.IsSaved = true;

            file.Id = id;

            return id;
        }

        /// <summary>
        /// Создание новых записей в бд.
        /// </summary>
        private void InsertFiles(List<File> files)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                foreach (var file in files) InsertFile(file, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Создание новых записей в бд.
        /// </summary>
        private void InsertFiles(List<File> files, OdbcTransaction transaction)
        {
            foreach (var file in files) InsertFile(file, transaction);
        }

        #endregion

        #region Update

        /// <summary>
        /// Обновление записи в бд.
        /// </summary>
        private void UpdateFile(File file)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;
            try
            {
                transaction = con.BeginTransaction();
                UpdateFile(file, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Обновление записи в бд.
        /// </summary>
        private void UpdateFile(File file, OdbcTransaction transaction)
        {
            var query = $@"
UPDATE dms.files SET 
doc = {file.Document.Id.ToDBString()}, replaced_to = {(file.ReplacedTo == null ? "".ToDBString() : file.ReplacedTo.Id.ToDBString())},
original_name = {file.FullName.ToDBString()}, date = {DB.ToDBString(file.Date)}, added = {file.Added.ToDBString()}, source = {file.Source.ToDBString()}
WHERE id = {file.Id.ToDBString()}
";

            var commad = new OdbcCommand(query, transaction.Connection, transaction);
            commad.ExecuteNonQuery();
            file.IsSaved = true;
        }

        /// <summary>
        /// Обновление записей в бд.
        /// </summary>
        private void UpdateFiles(List<File> files)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;
            try
            {
                transaction = con.BeginTransaction();
                foreach (var file in files) UpdateFile(file, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Обновление записей в бд
        /// </summary>
        private void UpdateFiles(List<File> files, OdbcTransaction transaction)
        {
            foreach (var file in files) UpdateFile(file, transaction);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Удаление одной записи из таблицы dms.files по id.
        /// </summary>
        /// <param name="fileId">id файла</param>
        public static void DeleteFile(int fileId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                DeleteFile(fileId, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Удаление одной записи из таблицы dms.files по id.
        /// </summary>
        /// <param name="fileId">id файла</param>
        /// <param name="transaction"></param>
        public static void DeleteFile(int fileId, OdbcTransaction transaction)
        {
            var query = $@"
DELETE FROM dms.files
WHERE id = {fileId}
";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаление записей из таблицы dms.files, относящихся к документу.
        /// </summary>
        /// <param name="documentId">id документа (Поле doc)</param>
        public static void DeleteFiles(int documentId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                DeleteFiles(documentId, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Удаление записей из таблицы dms.files, относящихся к документу.
        /// </summary>
        /// <param name="documentId">id документа (Поле doc)</param>
        /// <param name="transaction"></param>
        public static void DeleteFiles(int documentId, OdbcTransaction transaction)
        {
            var query = $@"
DELETE FROM dms.files
WHERE doc = {documentId}
";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        //public static void DeleteFilesInDocumentGroup(int documentGroupId)
        //{

        //}

        //public static void DeleteFilesInDocumentGroup(int documentGroupId, OdbcTransaction transaction)
        //{

        //}

        /// <summary>
        /// Рекрсивное удаление ВСЕХ файлов принадлежащей группе и её подгруппам.
        /// </summary>
        /// <param name="documentGroupId">id группы документов</param>
        public static void RecursiveDeleteFiles(int documentGroupId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                RecursiveDeleteFiles(documentGroupId, transaction);
                transaction.Commit();
            }
            catch
            {
                transaction?.Rollback();
                throw;
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Рекрсивное удаление ВСЕХ файлов принадлежащей группе и её подгруппам.
        /// </summary>
        /// <param name="documentGroupId">id группы документов</param>
        /// <param name="transaction">Транзакция</param>
        public static void RecursiveDeleteFiles(int documentGroupId, OdbcTransaction transaction)
        {
            var query = $@"
        WITH RECURSIVE tmp AS 
        (
          SELECT id, parentid FROM dms.doc_groups WHERE id = {documentGroupId}
            UNION ALL
          SELECT e.id, e.parentid FROM tmp t
          INNER JOIN dms.doc_groups e ON e.parentid = t.id
        ) 
        DELETE FROM dms.files WHERE doc IN 
        (
        SELECT docs.id FROM tmp
        LEFT JOIN dms.docs ON tmp.id = docs.groupid
        )
        ";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        #endregion

        #endregion
    }
}