using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using PGS.DMS.Models;

namespace PGS.DMS.Controllers
{
    /// <summary>
    /// Статичный класс предоставляет функции для работы с файлами.
    /// </summary>
    /// <remarks>
    /// Копирование, удаление, открытие и т.д.
    /// </remarks>
    internal static class FileManager
    {
        private const string TRASH_FOLDER = "$TRASH_FOLDER$";
        private static bool _isTrashFolderExist = false;

        /// <summary>
        /// Хранит все процессы, запущенные функцией Open
        /// </summary>
        private static readonly List<Process> _processPool = new List<Process>();

        /// <summary>
        /// Просматривает все процессы на статус завершённости => удаляет из _processPool.
        /// </summary>
        private static void CheckProcessesOnClose()
        {
            foreach (var process in _processPool.Where(p => p.HasExited)) 
                _processPool.Remove(process);
        }

        private static void CreateTrashFolder(NetworkFolder networkFolder)
        {
            var path = networkFolder.CombinePath(TRASH_FOLDER);
            if (Directory.Exists(path))
            {
                _isTrashFolderExist = true;
                return;
            }

            // Если не удастся создать - произойдёт ошибка и всё отменится.
            Directory.CreateDirectory(path);
            _isTrashFolderExist = true;
        }

        /// <summary>
        /// Функция копирования файла (С заменой!).
        /// </summary>
        /// <param name="from">Абсолютный путь к файлу.</param>
        /// <param name="to">Абсолютный путь, куда надо поместить файл (С именем файла).</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void CopyFile(string from, string to)
        {
            var fi = new FileInfo(from);
            if (!fi.Exists)
                throw new FileNotFoundException();
            fi.CopyTo(to, true);
        }

        /// <summary>
        /// Функция перемещения файла.
        /// </summary>
        /// <param name="from">Абсолютный путь к файлу.</param>
        /// <param name="to">Абсолютный путь, куда надо поместить файл (С именем файла).</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void MoveFile(string from, string to)
        {
            var fi = new FileInfo(from);
            if (!fi.Exists)
                throw new FileNotFoundException();
            fi.MoveTo(to);
        }

        /// <summary>
        /// Удаляет файл.
        /// </summary>
        /// <param name="pathToFile">Абсолютный путь к файлу.</param>
        public static void DeleteFile(string pathToFile)
        {
            var fi = new FileInfo(pathToFile);
            if (!fi.Exists)
                throw new FileNotFoundException();
            fi.Delete();
        }

        /// <summary>
        /// Безопасное удаление файла (Перемещает в корзину).
        /// </summary>
        /// <param name="networkFolder"></param>
        /// <param name="pathToFile">Абсолютный путь к файлу.</param>
        public static void SaveDeleteFile(NetworkFolder networkFolder, string pathToFile)
        {
            var fi = new FileInfo(pathToFile);
            if (!fi.Exists)
                throw new FileNotFoundException();
            
            if (!_isTrashFolderExist)
                CreateTrashFolder(networkFolder);

            MoveFile(pathToFile, networkFolder.CombinePath(TRASH_FOLDER, Path.GetFileName(pathToFile)));
        }

        /// <summary>
        /// Удаляет файл.
        /// </summary>
        /// <param name="networkFolder"></param>
        /// <param name="file"></param>
        /// <param name="ignoreUnexist">Игнорировать несуществующие файлы</param>
        public static void DeleteFile(NetworkFolder networkFolder, Models.File file, bool ignoreUnexist = false)
        {
            try
            {
                if (file.Document == null)
                    throw new NullReferenceException("Отсутствует ссылка на документ");

                if (file.Document.Group == null)
                    throw new NullReferenceException("Отсутствует ссылка на группу документов");

                var path = networkFolder.CombinePath(file.Document.Group.FullPath, file.NetworkName);
                SaveDeleteFile(networkFolder, path);
            }
            catch (FileNotFoundException)
            {
                if (!ignoreUnexist)
                    throw;
            }
        }

        /// <summary>
        /// Удаляет файлы, относящиеся к документу.
        /// </summary>
        /// <remarks>Игнорирует несуществующие файлы.</remarks>
        public static void DeleteFiles(NetworkFolder networkFolder, Document document)
        {
            if (document == null) 
                throw new NullReferenceException("Отсутствует ссылка на документ");

            foreach(var file in document.Files)
                DeleteFile(networkFolder, file, false);
        }

        /// <summary>
        /// Удаляет файлы, относящиеся к группе документов.
        /// </summary>
        /// <remarks>Игнорирует несуществующие файлы.</remarks>
        public static void DeleteFiles(NetworkFolder networkFolder, DocumentGroup documentGroup)
        {
            // Удаление файлов в документе
            if (documentGroup.Documents != null && documentGroup.Documents.Count > 0)
            {
                foreach (var document in documentGroup.Documents)
                {
                    DeleteFiles(networkFolder, document);
                }
            }

            // Если есть подгруппы
            if (documentGroup.DocumentSubgroups != null && documentGroup.DocumentSubgroups.Count > 0)
            {
                foreach (var subgroup in documentGroup.DocumentSubgroups)
                {
                    DeleteFiles(networkFolder, subgroup);
                }
            }
        }

        /// <summary>
        /// Удаление группы документов (директории на сетевом диске) вместе с файлами
        /// </summary>
        public static bool DeleteDirectory(NetworkFolder networkFolder, DocumentGroup documentGroup)
        {
            var path = networkFolder.CombinePath(documentGroup.FullPath);
            var di = new DirectoryInfo(path);
            if (!di.Exists) 
                return false;

            DeleteFiles(networkFolder, documentGroup);
            di.Delete(true);

            return true;
        }

        /// <summary>
        /// Запускает файл или веб-ссылку.
        /// </summary>
        /// <param name="filePath">Абсолютный путь к файлу.</param>
        /// <returns>Процесс открытого файла.</returns>
        public static Process Open(string filePath)
        {
            try
            {
                var tmp = Process.Start(filePath);
                _processPool.Add(tmp);
                return tmp;
            }
            catch (Win32Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Завершает процесс открытого файла.
        /// </summary>
        /// <param name="filePath">Абсолютный путь к файлу.</param>
        public static void Close(string filePath)
        {
            foreach (var process in _processPool.Where(p => p.MainModule != null && p.MainModule.FileName == filePath))
            {
                process.Close();
                _processPool.Remove(process);
            }
        }
    }
}