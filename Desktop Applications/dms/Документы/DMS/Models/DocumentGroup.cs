using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Odbc;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace PGS.DMS.Models
{
    /// <summary>
    /// Модель группы документов. Обладает методами взаимодействия с бд.
    /// </summary>
    public class DocumentGroup
    {
        private bool _isSaved = true;

        public bool IsSaved
        {
            get { return _isSaved; }
            private set { _isSaved = value; }
        }

        public event Action Changed;

        private List<DocumentGroup> _documentGroups;

        /// <summary>
        /// Подгруппы принадлежащие данный группе документов.
        /// </summary>
        public List<DocumentGroup> DocumentSubgroups
        {
            get { return _documentGroups; }
            set
            {
                _documentGroups = value;
                Changed?.Invoke();
            }
        }

        private List<Document> _documents;

        /// <summary>
        /// Документы принадлежащие данный группе документов.
        /// </summary>
        public List<Document> Documents
        {
            get { return _documents; }
            set
            {
                _documents = value;
                Changed?.Invoke();
            }
        }

        private int? _id;

        /// <summary>
        /// Id группы в бд.
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

        private DocumentGroup _parent;

        /// <summary>
        /// Группа документов, к которой пренадлежит данная группа (Если есть).
        /// </summary>
        public DocumentGroup Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                Changed?.Invoke();
            }
        }

        private string _name;

        /// <summary>
        /// Название группы документов.
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

        /// <summary>
        /// Полный путь к группе (На основе ее предков) | ".../parentGroup/currentGroup/childGroup/..."
        /// </summary>
        public string FullPath
        {
            get
            {
                var list = new List<DocumentGroup>();
                CreateChain(list, this);

                return list.Aggregate("", (current, dg) => Path.Combine(current, dg.Name));
            }
        }

        /// <summary>
        /// Новая группа документов.
        /// </summary>
        public DocumentGroup()
        {
            Changed += () => IsSaved = false;

            Id = null;
            Parent = null;
            Documents = new List<Document>();
            DocumentSubgroups = new List<DocumentGroup>();
        }

        /// <summary>
        /// Существующая группа документов.
        /// </summary>
        /// <remarks>
        /// Значения null у documents и documentSubgroup начинают автоматическую подгрузку из бд.
        /// </remarks>
        public DocumentGroup(int id, DocumentGroup parent, string name, List<Document> documents = null,
            List<DocumentGroup> documentSubgroups = null)
        {
            Changed += () => IsSaved = false;

            Id = id;
            Parent = parent;
            Name = name;

            Documents = documents != null
                ? documents
                : Document.LoadDocuments(this);

            DocumentSubgroups = documentSubgroups != null
                ? documentSubgroups
                : LoadDocumentSubgroups(this);
        }

        /// <summary>
        /// Созданий новой записи/сохранение изменений.
        /// </summary>
        public void Save()
        {
            if (Id == null)
            {
                Id = InsertDocumentGroup(this);
                foreach (var subgroup in DocumentSubgroups) subgroup.Save();
            }
            else
            {
                UpdateDocumentGroup(this);
                foreach (var subgroup in DocumentSubgroups) subgroup.Save();
            }

            IsSaved = true;
        }

        /// <summary>
        /// Созданий новой записи/сохранение изменений.
        /// </summary>
        public void Save(OdbcTransaction transaction)
        {
            if (Id == null)
            {
                Id = InsertDocumentGroup(this, transaction);
                foreach (var subgroup in DocumentSubgroups) subgroup.Save(transaction);
            }
            else
            {
                UpdateDocumentGroup(this, transaction);
                foreach (var subgroup in DocumentSubgroups) subgroup.Save(transaction);
            }

            IsSaved = true;
        }

        #region MyRegion

        /// <summary>
        /// Воссоздаёт цепочку групп, где первый элемент - корень.
        /// </summary>
        /// <param name="listDg">В этот лист идёт рекурсивное добавление</param>
        /// <param name="documentGroup"></param>
        private static void CreateChain(List<DocumentGroup> listDg, DocumentGroup documentGroup)
        {
            listDg.Add(documentGroup);
            if (documentGroup.Parent == null)
                listDg.Reverse();
            else
                CreateChain(listDg, documentGroup.Parent);
        }

        #endregion

        #region Загрузка/удаление информации о группе документов данных из бд

        #region LoadDocument

        /// <summary>
        /// Загружает DocumentGroup по id, а также её документы.
        /// </summary>
        /// <param name="id">id группы документов.</param>
        /// /// <param name="loadDocuments">Будут ли подгружены документы.</param>
        /// <returns>Возвращает Null если по указанному id нет записи.</returns>
        public static DocumentGroup LoadDocumentGroup(int id, bool loadDocuments = true)
        {
            DocumentGroup dg = null;

            var con = DB.Connect();
            try
            {
                dg = LoadDocumentGroup(id, con, loadDocuments);
            }
            finally
            {
                con.Disconnect();
            }

            return dg;
        }

        /// <summary>
        /// Загружает DocumentGroup по id, а также её документы.
        /// </summary>
        /// <param name="id">id группы документов.</param>
        /// <param name="con"></param>
        /// <param name="loadDocuments">Будут ли подгружены документы.</param>
        /// <returns>Возвращает Null если по указанному id нет записи.</returns>
        public static DocumentGroup LoadDocumentGroup(int id, OdbcConnection con, bool loadDocuments = true)
        {
            var query = $@"
SELECT parentid, name
FROM dms.doc_groups
WHERE id = {id}
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var reader = com.ExecuteReader();
            if (!reader.Read()) return null;

            var name = (string)reader["name"];
            int? parentId = null;
            if (!(reader["parentid"] is DBNull))
                parentId = (int)reader["parentid"];

            DocumentGroup parent = null;
            if (parentId != null)
                parent = LoadDocumentGroup((int)parentId, con, loadDocuments);

            // Если подгружать документы не надо - отправляем пустой лист
            var documents = loadDocuments ? null : new List<Document>();

            return new DocumentGroup(id, parent, name, documents);
        }

        /// <summary>
        /// Загружает все группы.
        /// </summary>
        /// <param name="loadDocuments">Будут ли подгружены документы.</param>
        public static List<DocumentGroup> LoadDocumentGroups(bool loadDocuments = true)
        {
            var con = DB.Connect();
            try
            {
                return LoadDocumentGroups(con, loadDocuments);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Загружает все группы.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="loadDocuments">Подгружать документы?</param>
        public static List<DocumentGroup> LoadDocumentGroups(OdbcConnection con, bool loadDocuments = true)
        {
            var documentGroups = new List<DocumentGroup>();

            var query = $@"
SELECT id, name
FROM dms.doc_groups
WHERE parentid is null
ORDER BY name
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var id = (int)reader["id"];
                var name = (string)reader["name"];

                // Если подгружать документы не надо - отправляем пустой лист
                var documents = loadDocuments ? null : new List<Document>();

                documentGroups.Add(new DocumentGroup(id, null, name, documents));
            }

            return documentGroups;
        }


        /// <summary>
        /// Подгружает все подгруппы, относящиеся к группе.
        /// </summary>
        private static List<DocumentGroup> LoadDocumentSubgroups(DocumentGroup documentGroup)
        {
            var con = DB.Connect();

            try
            {
                return LoadDocumentSubgroups(documentGroup, con);
            }
            finally
            {
                con.Disconnect();
            }
        }

        /// <summary>
        /// Подгружает все подгруппы, относящиеся к группе.
        /// </summary>
        private static List<DocumentGroup> LoadDocumentSubgroups(DocumentGroup documentGroup, OdbcConnection con)
        {
            var documentGroups = new List<DocumentGroup>();

            var query = $@"
SELECT id, name
FROM dms.doc_groups
WHERE parentid = {documentGroup.Id}
ORDER BY name
";
            var com = con.CreateCommand();
            com.CommandText = query;

            var reader = com.ExecuteReader();
            while (reader.Read())
            {
                var id = (int)reader["id"];
                var name = (string)reader["name"];
                documentGroups.Add(new DocumentGroup(id, documentGroup, name));
            }

            return documentGroups;
        }

        #endregion

        #region Insert

        /// <summary>
        /// Добавление новой записи в таблицу dms.docs. 
        /// </summary>
        /// <remarks>(Не добавляет подгруппы)</remarks>
        private int? InsertDocumentGroup(DocumentGroup documentGroup)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                var id = InsertDocumentGroup(documentGroup, transaction);
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
        /// <remarks>(Не добавляет подгруппы)</remarks>
        private int InsertDocumentGroup(DocumentGroup documentGroup, OdbcTransaction transaction)
        {
            var query = $@"
INSERT INTO dms.doc_groups
(parentid, name)
VALUES
({(documentGroup.Parent == null ? "null" : documentGroup.Parent.Id.ToDBString())}, {documentGroup.Name.ToDBString()})
RETURNING id
";

            var com = new OdbcCommand(query, transaction.Connection, transaction);
            var id = (int)com.ExecuteScalar();
            documentGroup.Id = id;

            // Сохранение документов (Сохранение файлов происходит при сохранении документов)
            foreach (var document in documentGroup.Documents) document.Save(transaction);

            documentGroup.IsSaved = true;

            return id;
        }

        #endregion

        #region Update

        /// <summary>
        /// Обновление записи в таблице dms.docs. 
        /// </summary>
        /// <remarks>(Не обновляет подгруппы)</remarks>
        private int? UpdateDocumentGroup(DocumentGroup documentGroup)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                var id = UpdateDocumentGroup(documentGroup, transaction);
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
        /// Обновление записи в таблице dms.docs. 
        /// </summary>
        /// <remarks>(Не обновляет подгруппы)</remarks>
        private int UpdateDocumentGroup(DocumentGroup documentGroup, OdbcTransaction transaction)
        {
            var query = $@"
UPDATE dms.doc_groups SET
parentid = {(documentGroup.Parent == null ? "null" : documentGroup.Parent.Id.ToDBString())},
name = {documentGroup.Name.ToDBString()}
WHERE id = {documentGroup.Id.ToDBString()}
";

            var com = new OdbcCommand(query, transaction.Connection, transaction);
            com.ExecuteScalar();

            // Обновление документов (Обновление файлов происходит при обновлении документов)
            foreach (var document in documentGroup.Documents) document.Save(transaction);

            documentGroup.IsSaved = true;

            return (int)documentGroup.Id;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Удаление группы со всеми подгруппами, а также их документы и файлы.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        public static void DeleteDocumentGroups(int documentGroupId)
        {
            var con = DB.Connect();
            OdbcTransaction transaction = null;

            try
            {
                transaction = con.BeginTransaction();
                DeleteDocumentGroups(documentGroupId, transaction);
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
        /// Удаление группы со всеми подгруппами, а также их документы и файлы.
        /// </summary>
        /// <param name="documentGroupId">id группы документов.</param>
        /// <param name="transaction"></param>
        public static void DeleteDocumentGroups(int documentGroupId, OdbcTransaction transaction)
        {
            // Удаление документов (Удаляя документ, удаляем и файлы)
            Document.RecursiveDeleteDocuments(documentGroupId, transaction);

            var query = $@"
WITH RECURSIVE tmp AS (
  SELECT id, parentid FROM dms.doc_groups WHERE id = {documentGroupId}
    UNION ALL
  SELECT e.id, e.parentid FROM tmp t
  INNER JOIN dms.doc_groups e ON e.parentid = t.id
) DELETE FROM dms.doc_groups WHERE id IN (SELECT id FROM tmp)
";

            var command = new OdbcCommand(query, transaction.Connection, transaction);
            command.ExecuteNonQuery();
        }

        #endregion

        #endregion
    }
}