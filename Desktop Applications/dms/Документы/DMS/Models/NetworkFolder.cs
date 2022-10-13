using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;
using PGS.DMS.Controllers;

namespace PGS.DMS.Models
{
    /// <summary>
    /// Класс представляет объект сетевой директории. 
    /// </summary>
    /// <remarks>
    /// <para>Содержит методы валидации/актуальности (Сопоставления структуры директории с базой данных).</para>
    /// Методы удобной работы с путями.
    /// </remarks>
    public class NetworkFolder
    {
        private const string TRASH_BUCKET_NAME = "$trashbucket$";

        private string _networkFolderPath;

        /// <summary>
        /// Путь к сетевой директории.
        /// </summary>
        public string NetworkFolderPath
        {
            get { return _networkFolderPath; }
            private set
            {
                //if (!Unc.IsUncPath(value))
                //    throw new ArgumentException($"{value}\nНе является сетевым путём!");
                _networkFolderPath = value;
            }
        }

        /// <summary>
        /// Путь к корзине в сетевой директории.
        /// </summary>
        private string TrashBucketPath => CombinePath(TRASH_BUCKET_NAME);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkFolderPath"></param>
        /// <param name="isSuperUser">Если true - то будет автоматическая проверка соответствия структуры БД и сетевой папки.</param>
        public NetworkFolder(string networkFolderPath, bool isSuperUser = false)
        {
// todo: Добаботать класс на права редактирования
            NetworkFolderPath = networkFolderPath;
            if (!isSuperUser) return;
        }

        /// <summary>
        /// Проверка соединения с сетевой папкой.
        /// </summary>
        /// <returns></returns>
        public bool CheckConnection()
        {
            var di = new DirectoryInfo(NetworkFolderPath);
            return di.Exists;
        }

        /// <summary>
        /// Возвращает путь к файлу/директории в сетевой папке.
        /// </summary>
        /// <param name="listNames">Имя директорий/файлов.</param>
        /// <returns></returns>
        public string CombinePath(params string[] listNames)
        {
            var tmp = NetworkFolderPath;
            foreach (var name in listNames)
                tmp = Path.Combine(tmp, name);
            return tmp;
        }

        /// <summary>
        /// Возвращает относительный путь (Сравнение с сетевым путём).
        /// </summary>
        public string GetRelativePath(string path)
        {
            return path.Replace(NetworkFolderPath + "\\", "");
        }

        #region Работа с корзиной

        /// <summary>
        /// Проверка/создание корзины.
        /// </summary>
        private void CheckTrashBucket()
        {
            var pathToTrashBucket = CombinePath(TRASH_BUCKET_NAME);
            if (!Directory.Exists(pathToTrashBucket))
                Directory.CreateDirectory(pathToTrashBucket);
        }

        /// <summary>
        /// Переносит директорию с файлами в корзину.
        /// </summary>
        /// <param name="path">Путь вида "\documentGroup\subgroups\..."</param>
        private void MoveToTrashBucket(string path)
        {
            var pathInNetwork = CombinePath(path);
            var pathInTrashBucket = CombinePath(TrashBucketPath, path);
            if (!Directory.Exists(pathInTrashBucket))
                Directory.CreateDirectory(pathInTrashBucket);
            Directory.Move(pathInNetwork, pathInTrashBucket);
        }

        #endregion

        #region Работа с каталогами

        //todo: можно объединить
        /// <summary>
        /// Удаление директории по относительному пути.
        /// </summary>
        public void DeleteDirectoryByRelative(string relativePath)
        {
            new DirectoryInfo(CombinePath(relativePath)).Delete(true);
        }

        /// <summary>
        /// Удаление директории по абсолютному пути.
        /// </summary>
        public void DeleteDirectoryByAbsolute(string absolutePath)
        {
            new DirectoryInfo(absolutePath).Delete(true);
        }

        #endregion

        #region Работа с файлами

        #endregion

        #region Воссоздание каталогов из структуры групп документов БД

        public enum ChangeType
        {
            // Если есть запись в бд, но нет на сетевом диске.
            Add = 0,
            // Если есть на сетевом диске, но нет записи в бд.
            Remove = 1
        }

        /// <summary>
        /// Сравнивает директории на сетевом диске со структорой бд и возвращает сводку.
        /// </summary>
        public Dictionary<string, ChangeType> CompareNetworkFolders(Dictionary<string, ChangeType> equalList, DocumentGroup documentGroup)
        {
            var di = new DirectoryInfo(CombinePath(documentGroup.FullPath));

            // Если есть запись в бд, но нет директории на сетевом диске.
            if (!di.Exists)
            {
                equalList.Add($"{documentGroup.FullPath}", ChangeType.Add);
            }
            else
            {
                var directories = di.GetDirectories();

                // Если есть директория на сетевом диске, но нет записи в бд.
                foreach (var dir in directories)
                {
                    if (!(documentGroup.DocumentSubgroups.Exists(x => x.Name == dir.Name)))
                        equalList.Add($"{Path.Combine(documentGroup.FullPath, dir.Name)}", ChangeType.Remove);
                }
            }
            
            // Если есть подгруппы.
            if (documentGroup.DocumentSubgroups.Count > 0)
                foreach (var subgroup in documentGroup.DocumentSubgroups)
                {
                    CompareNetworkFolders(equalList, subgroup);
                }

            return equalList;
        }

        /// <summary>
        /// Сравнивает и применяет изменения директории на сетевом диске со структорой бд.
        /// </summary>
        /// <param name="equalDict">Словарь с изменениями, полученный функцией CompareNetworkFolders</param>
        public void RecreateNetworkFolders(Dictionary<string, ChangeType> equalDict)
        {
            foreach (var keyValue in equalDict)
            {
                switch (keyValue.Value)
                {
                    case ChangeType.Add:
                        Directory.CreateDirectory(CombinePath(keyValue.Key));
                        break;
                    case ChangeType.Remove:
                        Directory.Delete(CombinePath(keyValue.Key), true);
                        break;
                    default:
                        return;
                }
            }
        }

        /// <summary>
        /// Сравнивает директории на сетевом диске со структорой бд и возвращает сводку.
        /// </summary>
        public Dictionary<string, ChangeType> CompareNetworkFiles(Dictionary<string, ChangeType> equalList, DocumentGroup documentGroup)
        {
            var di = new DirectoryInfo(CombinePath(documentGroup.FullPath));
            if (!di.Exists)
                return equalList;
            var files = di.GetFiles();

            // Лист со всеми файлами документов в группе
            var filesInDocumentGroup = 
                (from document in documentGroup.Documents from file in document.Files select file.NetworkName).ToList();

            
            // Если есть файл на сетевом диске, но нет записи в бд.
            foreach (var file in files)
            {
                if (!filesInDocumentGroup.Contains(file.Name))
                    equalList.Add(Path.Combine(documentGroup.FullPath, file.Name), ChangeType.Add);
            }

            // Если есть запись в бд, но нет файла на сетевом диске.
            foreach (var file in filesInDocumentGroup)
            {
                if (!files.Any(x => x.Name == file))
                {
                    equalList.Add(Path.Combine(documentGroup.FullPath, file), ChangeType.Remove);
                } 
            }

            // Если есть подгруппы.
            if (documentGroup.DocumentSubgroups.Count > 0)
                foreach (var subgroup in documentGroup.DocumentSubgroups)
                {
                    CompareNetworkFiles(equalList, subgroup);
                }

            return equalList;
        }

        /// <summary>
        /// Сравнивает и удаляет "лишние" файлы.
        /// </summary>
        /// <param name="equalDict">Словарь с изменениями, полученный функцией CompareNetworkFolders</param>
        /// <remarks>Не создаёт недостающие файлы</remarks>
        public void RecreateNetworkFiles(Dictionary<string, ChangeType> equalDict)
        {
            foreach (var keyValue in equalDict)
            {
                switch (keyValue.Value)
                {
                    case ChangeType.Add:
                        try
                        {
                            var z = CombinePath(keyValue.Key);
                            FileManager.DeleteFile(CombinePath(keyValue.Key));
                        }
                        catch
                        {
                            // Игнорируем
                        }

                        break;
                    case ChangeType.Remove:
                    default:
                        continue;
                }
            }
        }

        #endregion
    }
}