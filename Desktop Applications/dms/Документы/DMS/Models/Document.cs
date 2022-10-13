using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;

namespace PGS.DMS.Models
{
    /// <summary>
    /// Модель Документа. Обладает методами взаимодействия с бд.
    /// </summary>
    public class Document
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
        /// Id документа в бд.
        /// </summary>
        public int? Id
        {
            get { return _id; }
            set
            {
                _id = value;
                Changed?.Invoke();
            }
        }

        private string _name;

        /// <summary>
        /// Название документа.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Changed?.Invoke();
            }
        }

        private DocumentGroup _group;

        /// <summary>
        /// Группа документов, к которой относится файл
        /// </summary>
        public DocumentGroup Group
        {
            get { return _group; }
            set
            {
                _group = value;
                Changed?.Invoke();
            }
        }

        private List<File> _files;

        /// <summary>
        /// Файлы относящиеся к документу.
        /// </summary>
        public List<File> Files
        {
            get { return _files; }
            set
            {
                _files = value;
                Changed?.Invoke();
            }
        }

        private string _number = "";

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                Changed?.Invoke();
            }
        }


        private string _Comment = "";
        /// <summary>
        /// Коментарий
        /// </summary>
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                if (_Comment != value)
                {
                    _Comment = value;
                    Changed?.Invoke();
                }
            }
        }

        private int? _creator;

        /// <summary>
        /// Создатель.
        /// </summary>
        public int? Creator
        {
            get { return _creator; }
            set
            {
                _creator = value;
                Changed?.Invoke();
            }
        }

        private DateTime? _dateIn = DateTime.Now;

        /// <summary>
        /// Дата ввода.
        /// </summary>
        public DateTime? DateIn
        {
            get { return _dateIn; }
            set
            {
                _dateIn = value;
                Changed?.Invoke();
            }
        }

        private DateTime? _dateRevision;

        /// <summary>
        /// Дата ревизии.
        /// </summary>
        public DateTime? DateRevision
        {
            get { return _dateRevision; }
            set
            {
                _dateRevision = value;
                Changed?.Invoke();
            }
        }

        private DateTime? _dateNextRevision;

        /// <summary>
        /// Дата следующей ревизии.
        /// </summary>
        public DateTime? DateNextRevision
        {
            get { return _dateNextRevision; }
            set
            {
                _dateNextRevision = value;
                Changed?.Invoke();
            }
        }

        private DateTime? _dateOut;

        /// <summary>
        /// Дата вывода из оборота.
        /// </summary>
        public DateTime? DateOut
        {
            get { return _dateOut; }
            set
            {
                _dateOut = value;
                Changed?.Invoke();
            }
        }

        private int? _responsible;

        /// <summary>
        /// Ответственный.
        /// </summary>
        public int? Responsible
        {
            get { return _responsible; }
            set
            {
                _responsible = value;
                Changed?.Invoke();
            }
        }

        private bool _isPhysicalMedium;

        /// <summary>
        /// Есть бумажная копия.
        /// </summary>
        public bool IsPhysicalMedium
        {
            get { return _isPhysicalMedium; }
            set
            {
                _isPhysicalMedium = value;
                Changed?.Invoke();
            }
        }

        private int? _place;

        /// <summary>
        /// Место хранения.
        /// </summary>
        public int? Place
        {
            get { return _place; }
            set
            {
                _place = value;
                Changed?.Invoke();
            }
        }

        public List<Tuple<string, int>> _links;

        /// <summary>
        /// Связи документов. | string=ref_table (Ссылка на таблицу), int=ref_id (Id записи в таблице ref_table)
        /// </summary>
        public List<Tuple<string, int>> Links
        {
            get { return _links; }
            set
            {
                _links = value;
                Changed?.Invoke();
            }
        }

        /// <summary>
        /// Создания нового документа.
        /// </summary>
        public Document()
        {
            Changed += () => IsSaved = false;

            Files = new List<File>();
            Links = new List<Tuple<string, int>>();
        }

        /// <summary>
        /// Существующий документ.
        /// </summary>
        /// <remarks>Автоматически подгрузит группу документов и файлы</remarks>
        public Document(int id, List<File> files = null)
        {
            Changed += () => IsSaved = false;

            Id = id;
            Files = files != null ? files : File.LoadFiles(this);
        }

        /// <summary>
        /// Созданий новой записи/сохранение изменений.
        /// </summary>
        public void Save()
        {
            if (Id == null)
                Id = InsertDocument(this);
            else
                UpdateDocument(this);

            IsSaved = true;
        }

        /// <summary>
        /// Созданий новой записи/сохранение изменений.
        /// </summary>
        public void Save(OdbcTransaction transaction)
        {
            if (Id == null)
                Id = InsertDocument(this, transaction);
            else
                UpdateDocument(this, transaction);

            IsSaved = true;
        }

        #region Загрузка/изменение/удаление информации о документе данных из бд

        #region LoadDocument

        /// <summary>
        /// Загружает документ по id.
        /// </summary>
        /// /// <param name="loadOtherDocuments">Загружать остальные документы в родительских группах</param>
        public static Document LoadDocument(int documentId, bool loadOtherDocuments = true)
        {
            var con = DB.Connect();

            try
            {
                return LoadDocument(documentId, con, loadOtherDocuments);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Загружает документ по id.
        /// </summary>
        /// <param name="loadOtherDocuments">Загружать остальные документы в родительских группах</param>
        public static Document LoadDocument(int documentId, OdbcConnection con, bool loadOtherDocuments = true)
        {
            var query = $@"
SELECT 
id, name, groupid, number, creator, date_in, responsible, physical_medium, date_revision, date_revision_next, date_out, place, comment
FROM dms.docs
where id = {documentId}
ORDER BY name
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var reader = com.ExecuteReader();

            if (!reader.Read()) return null;

            DateTime tempDate;

            var id = (int)reader["id"];
            var document = new Document(id)
            {
                Name = (string)reader["name"],
                Group = DocumentGroup.LoadDocumentGroup((int)reader["groupId"], loadOtherDocuments),
                Number = reader["number"] != DBNull.Value ? (string)reader["number"] : "",
                Creator = (int)reader["creator"],
                DateIn = DateTime.Parse(reader["date_in"].ToString()),
                Responsible = (int)reader["responsible"],
                IsPhysicalMedium = (bool)reader["physical_medium"],
                Comment = reader["comment"] != DBNull.Value ? (string)reader["comment"] : "",
                Links = LoadLinks(id, con)
            };

            if (!loadOtherDocuments)
                document.Group?.Documents.Add(document);

            if (DateTime.TryParse(reader["date_revision"].ToString(), out tempDate))
                document.DateRevision = tempDate;
            if (DateTime.TryParse(reader["date_revision_next"].ToString(), out tempDate))
                document.DateNextRevision = tempDate;
            if (DateTime.TryParse(reader["date_out"].ToString(), out tempDate))
                document.DateOut = tempDate;
            if (reader["place"] != DBNull.Value)
                document.Place = (int)reader["place"];


            return document;
        }

        /// <summary>
        /// Подгружает все документы, относящиеся к группе документов.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        public static List<Document> LoadDocuments(int documentGroupId)
        {
            var con = DB.Connect();

            try
            {
                return LoadDocuments(documentGroupId, con);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Подгружает все документы, относящиеся к группе документов.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        /// <param name="con"></param>
        public static List<Document> LoadDocuments(int documentGroupId, OdbcConnection con)
        {
            var documents = new List<Document>();

            var query = $@"
SELECT
id, name, groupid, number, creator, date_in, responsible, physical_medium, date_revision, date_revision_next, date_out, place, comment
FROM dms.docs
where groupid = {documentGroupId}
ORDER BY name
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                DateTime tempDate;

                var id = (int)reader["id"];
                var document = new Document(id)
                {
                    Name = (string)reader["name"],
                    Group = DocumentGroup.LoadDocumentGroup((int)reader["groupId"]),
                    Number = reader["number"] != DBNull.Value ? (string)reader["number"] : "",
                    Creator = (int)reader["creator"],
                    DateIn = DateTime.Parse(reader["date_in"].ToString()),
                    Responsible = (int)reader["responsible"],
                    IsPhysicalMedium = (bool)reader["physical_medium"],
                    Comment = reader["comment"] != DBNull.Value ? (string)reader["comment"] : "",
                    Links = LoadLinks(id, con)
                };

                if (DateTime.TryParse(reader["date_revision"].ToString(), out tempDate))
                    document.DateRevision = tempDate;
                if (DateTime.TryParse(reader["date_revision_next"].ToString(), out tempDate))
                    document.DateNextRevision = tempDate;
                if (DateTime.TryParse(reader["date_out"].ToString(), out tempDate))
                    document.DateOut = tempDate;
                if (reader["place"] != DBNull.Value)
                    document.Place = (int)reader["place"];

                documents.Add(document);
            }

            return documents;
        }

        /// <summary>
        /// Подгружает все документы, относящиеся к группе документов
        /// </summary>
        /// <param name="documentGroup"></param>
        public static List<Document> LoadDocuments(DocumentGroup documentGroup)
        {
            var con = DB.Connect();

            try
            {
                return LoadDocuments(documentGroup, con);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Подгружает все документы, относящиеся к группе документов.
        /// </summary>
        public static List<Document> LoadDocuments(DocumentGroup documentGroup, OdbcConnection con)
        {
            var documents = new List<Document>();

            var query = $@"
SELECT
id, name, groupid, number, creator, date_in, responsible, physical_medium, date_revision, date_revision_next, date_out, place, comment
FROM dms.docs
where groupid = {documentGroup.Id}
ORDER BY name
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                DateTime tempDate;

                var id = (int)reader["id"];
                var document = new Document(id)
                {
                    Name = (string)reader["name"],
                    Group = documentGroup,
                    Number = reader["number"] != DBNull.Value ? (string)reader["number"] : "",
                    Creator = (int)reader["creator"],
                    DateIn = DateTime.Parse(reader["date_in"].ToString()),
                    Responsible = (int)reader["responsible"],
                    IsPhysicalMedium = (bool)reader["physical_medium"],
                    Comment = reader["comment"] != DBNull.Value ?(string)reader["comment"] : "",
                    Links = LoadLinks(id, con)
                };

                if (DateTime.TryParse(reader["date_revision"].ToString(), out tempDate))
                    document.DateRevision = tempDate;
                if (DateTime.TryParse(reader["date_revision_next"].ToString(), out tempDate))
                    document.DateNextRevision = tempDate;
                if (DateTime.TryParse(reader["date_out"].ToString(), out tempDate))
                    document.DateOut = tempDate;
                if (reader["place"] != DBNull.Value)
                    document.Place = (int)reader["place"];

                documents.Add(document);
            }

            return documents;
        }

        /// <summary>
        /// Подгружает все связи, принадлежащие документы.
        /// </summary>
        private static List<Tuple<string, int>> LoadLinks(int documentId)
        {
            var con = DB.Connect();

            try
            {
                return LoadLinks(documentId, con);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Подгружает все связи, принадлежащие документы.
        /// </summary>
        private static List<Tuple<string, int>> LoadLinks(int documentId, OdbcConnection con)
        {
            var links = new List<Tuple<string, int>>();

            var com = con.CreateCommand();
            com.CommandText = $"SELECT ref_table, ref_id FROM dms.doc_links WHERE doc = {DB.ToDBString(documentId)}";

            var reader = com.ExecuteReader();

            while (reader.Read()) links.Add(new Tuple<string, int>((string)reader["ref_table"], (int)reader["ref_id"]));

            return links;
        }

        #endregion

        #region Insert

        /// <summary>
        /// Добавление новой записи в таблицу dms.docs.
        /// </summary>
        private int? InsertDocument(Document document)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                var id = InsertDocument(document, transaction);
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
        /// Добавление новой записи в таблицу dms.docs.
        /// </summary>
        private int InsertDocument(Document document, OdbcTransaction transaction)
        {
            var query = $@"
INSERT INTO dms.docs 
(groupid, name, number, creator, date_in, date_revision, date_revision_next, date_out, responsible, physical_medium, place, comment)
VALUES
({document.Group.Id.ToDBString()}, {document.Name.ToDBString()}, {document.Number.ToDBString()},
{document.Creator.ToDBString()}, {document.DateIn.ToDBString()}, {document.DateRevision.ToDBString()}, {document.DateNextRevision.ToDBString()},
{document.DateOut.ToDBString()}, {document.Responsible.ToDBString()}, {DB.ToDBString(document.IsPhysicalMedium)},
{document.Place.ToDBString()}, {document.Comment.ToDBString()})
RETURNING id
";

            var com = new OdbcCommand(query, transaction.Connection, transaction);
            var id = (int)com.ExecuteScalar();
            document.Id = id;

            // Сохранение файлов
            if (document.Files.Count > 0)
            {
                var savingFile = document.Files.First(x => x.ReplacedTo == null);
                for (var i = 0; i < document.Files.Count; i++)
                {
                    savingFile.Save(transaction);
                    savingFile = savingFile.ReplacedFrom;
                }
            }

            // Сохранение связей
            InsertDocumentLinks(document, transaction);

            document.IsSaved = true;

            return id;
        }

        /// <summary>
        /// Добавление связей документа. 
        /// </summary>
        /// <remarks>Если запись уже есть - ошибка игнорируется.</remarks>
        private void InsertDocumentLinks(Document document)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                InsertDocumentLinks(document, transaction);
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
        /// Добавление связей документа. 
        /// </summary>
        /// <remarks>Если запись уже есть - ошибка игнорируется.</remarks>
        private void InsertDocumentLinks(Document document, OdbcTransaction transaction)
        {
            var query = "";
            var com = new OdbcCommand(query, transaction.Connection, transaction);

            foreach (var link in document.Links)
            {
                query = $@"
INSERT INTO dms.doc_links 
VALUES 
({document.Id.ToDBString()}, {link.Item1.ToDBString()}, {DB.ToDBString(link.Item2)})
ON CONFLICT DO NOTHING
";
                com.CommandText = query;
                com.ExecuteNonQuery();
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Изменение записи в таблице dms.docs.
        /// </summary>
        private void UpdateDocument(Document document)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;
            try
            {
                transaction = con.BeginTransaction();
                UpdateDocument(document, transaction);
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
        /// Изменение записи в таблице dms.docs.
        /// </summary>
        private void UpdateDocument(Document document, OdbcTransaction transaction)
        {
            var query = $@"
UPDATE dms.docs 
SET 
groupid = {document.Group.Id.ToDBString()}, name = {document.Name.ToDBString()},
number = {document.Number.ToDBString()}, creator = {document.Creator.ToDBString()},
date_in = {document.DateIn.ToDBString()}, date_revision = {document.DateRevision.ToDBString()},
date_revision_next = {document.DateNextRevision.ToDBString()}, date_out = {document.DateOut.ToDBString()},
responsible = {document.Responsible.ToDBString()}, physical_medium = {DB.ToDBString(document.IsPhysicalMedium)},
place = {document.Place.ToDBString()}, comment = {document.Comment.ToDBString()}
WHERE id = {document.Id}
";
            var com = new OdbcCommand(query, transaction.Connection, transaction);
            com.ExecuteNonQuery();

            // Сохранение файлов
            if (document.Files.Count > 0)
            {
                var savingFile = document.Files.First(x => x.ReplacedTo == null);
                for (var i = 0; i < document.Files.Count; i++)
                {
                    savingFile.Save(transaction);
                    savingFile = savingFile.ReplacedFrom;
                }
            }

            // Сохранение связей
            InsertDocumentLinks(document, transaction);

            document.IsSaved = true;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Удаление записи из таблицы dms.docs.
        /// </summary>
        /// <param name="documentId">id документа</param>
        /// <remarks> + удаление всех связей из таблицы dms.doc_links</remarks>
        public static void DeleteDocument(int documentId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;
            try
            {
                transaction = con.BeginTransaction();
                DeleteDocument(documentId, transaction);
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
        /// Удаление записи из таблицы dms.docs.
        /// </summary>
        /// <param name="documentId">id документа</param>
        /// <param name="transaction"></param>
        /// <remarks> + удаление всех связей из таблицы dms.doc_links</remarks>
        public static void DeleteDocument(int documentId, OdbcTransaction transaction)
        {
            var query = $@"
DELETE FROM dms.docs
WHERE id = {documentId}
";

            File.DeleteFiles(documentId, transaction);
            DeleteDocumentLinks(documentId, transaction);

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Рекрсивное удаление ВСЕХ документов и файлов принадлежащей группе и её подгруппам.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        public static void RecursiveDeleteDocuments(int documentGroupId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                RecursiveDeleteDocuments(documentGroupId, transaction);
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
        /// Рекрсивное удаление ВСЕХ документов принадлежащей группе и её подгруппам.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        /// <param name="transaction"></param>
        public static void RecursiveDeleteDocuments(int documentGroupId, OdbcTransaction transaction)
        {
            // Удаление файлов
            File.RecursiveDeleteFiles(documentGroupId, transaction);
            // Удаление связей
            RecursiveDeleteDocumentsLinks(documentGroupId, transaction);

            var query = $@"
        WITH RECURSIVE tmp AS (
          SELECT id, parentid FROM dms.doc_groups WHERE id = {documentGroupId}
            UNION ALL
          SELECT e.id, e.parentid FROM tmp t
          INNER JOIN dms.doc_groups e ON e.parentid = t.id
        ) DELETE FROM dms.docs WHERE groupid IN (SELECT id FROM tmp)
        ";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаляет одну запись по уникальной паре значений.
        /// </summary>
        public static void DeleteDocumentLink(int documentId, string refTable, int refId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                DeleteDocumentLink(documentId, refTable, refId, transaction);
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
        /// Удаляет одну связь по уникальной паре значений.
        /// </summary>
        public static void DeleteDocumentLink(int documentId, string refTable, int refId, OdbcTransaction transaction)
        {
            var query = $@"
DELETE FROM dms.doc_links 
WHERE doc = {DB.ToDBString(documentId)} AND ref_table = {refTable.ToDBString()} AND ref_id = {DB.ToDBString(refId)}
";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Удаляет все связи по id документа.
        /// </summary>
        /// <param name="documentId"></param>
        public static void DeleteDocumentLinks(int documentId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                DeleteDocumentLinks(documentId, transaction);
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
        /// Удаляет все связи по id документа.
        /// </summary>
        /// <param name="documentId"></param>
        public static void DeleteDocumentLinks(int documentId, OdbcTransaction transaction)
        {
            var query = $@"
DELETE FROM dms.doc_links 
WHERE doc = {DB.ToDBString(documentId)}
";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Рекрсивное удаление ВСЕХ связей документов принадлежащей группе и её подгруппам.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        public static void RecursiveDeleteDocumentsLinks(int documentGroupId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                RecursiveDeleteDocumentsLinks(documentGroupId, transaction);
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
        /// Рекрсивное удаление ВСЕХ связей документов принадлежащей группе и её подгруппам.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        /// <param name="transaction"></param>
        public static void RecursiveDeleteDocumentsLinks(int documentGroupId, OdbcTransaction transaction)
        {
            var query = $@"
        WITH RECURSIVE tmp AS 
        (
            SELECT id, parentid FROM dms.doc_groups WHERE id = {DB.ToDBString(documentGroupId)}
                UNION ALL
            SELECT e.id, e.parentid FROM tmp t
            INNER JOIN dms.doc_groups e ON e.parentid = t.id
        ) 
        DELETE FROM dms.doc_links WHERE doc_links.doc IN 
        (
            SELECT id FROM dms.docs WHERE groupid IN (SELECT id FROM tmp)
        )
        ";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        #endregion

        #endregion
    }
}