using System;
using System.IO;
using System.Management;

namespace PGS.DMS.Controllers
{
    internal static class Unc
    {
        /// <summary>
        ///     Преобразует указанный путь в полный путь UNC, если этот путь является подключенным диском.
        ///     Если это не сетевой путь, возвращает без изменений
        /// </summary>
        public static string ResolveToUNC(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("The path argument was null or whitespace.");

            if (!Path.IsPathRooted(path))
                throw new ArgumentException(
                    $"The path '{path}' was not a rooted path and ResolveToUNC does not support relative paths."
                );

            // Если путь уже в UNC формате
            if (path.StartsWith(@"\\")) return path;

            var rootPath = ResolveToRootUNC(path);

            return path.StartsWith(rootPath) ? path : path.Replace(GetDriveLetter(path), rootPath);
        }

        /// <summary>
        ///     Преобразует путь в UNC, если этот путь является подключенным сетевым диском.
        ///     Если это не сетевой путь, возвращает без изменений
        /// </summary>
        /// <param name="path">The path to resolve.</param>
        public static string ResolveToRootUNC(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("The path argument was null or whitespace.");

            if (!Path.IsPathRooted(path))
                throw new ArgumentException(
                    $"The path '{path}' was not a rooted path and ResolveToRootUNC does not support relative paths."
                );

            if (path.StartsWith(@"\\")) return Directory.GetDirectoryRoot(path);

            // Находим букву тома
            var driveletter = GetDriveLetter(path);

            // Проверяем, является ли буква тома сетевым диском через WMI
            using (var mo = new ManagementObject())
            {
                mo.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", driveletter));

                var driveType = (DriveType)(uint)mo["DriveType"];
                var networkRoot = Convert.ToString(mo["ProviderName"]);

                if (driveType == DriveType.Network)
                    return networkRoot;
                else
                    return driveletter + Path.DirectorySeparatorChar;
            }
        }

        /// <summary>
        ///     Проверяет, является ли указанный путь сетевым диском
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns></returns>
        public static bool IsNetworkDrive(string path)
        {
            if (IsUncPath(path)) return true;

            // Находим букву тома
            var driveletter = GetDriveLetter(path);

            // Проверяем, является ли буква тома сетевым диском через WMI
            using (var mo = new ManagementObject())
            {
                mo.Path = new ManagementPath(string.Format("Win32_LogicalDisk='{0}'", driveletter));
                var driveType = (DriveType)(uint)mo["DriveType"];
                return driveType == DriveType.Network;
            }
        }

        /// <summary>
        /// Удовлетворяет ли переденнвй путь UNC стилю
        /// </summary>
        public static bool IsUncPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Path.IsPathRooted(path)) return false;

            return path.StartsWith(@"\\");
        }

        /// <summary>
        ///     Получает букву тома
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns>C:</returns>
        public static string GetDriveLetter(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentNullException("The path argument was null or whitespace.");

            if (!Path.IsPathRooted(path))
                throw new ArgumentException(
                    $"The path '{path}' was not a rooted path and GetDriveLetter does not support relative paths."
                );

            if (path.StartsWith(@"\\")) throw new ArgumentException("A UNC path was passed to GetDriveLetter");

            return Directory.GetDirectoryRoot(path).Replace(Path.DirectorySeparatorChar.ToString(), "");
        }

        /// <summary>
        /// Конкатенация путей с учётом специфики ОС
        /// </summary>
        public static string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        /// <summary>
        /// Возвращает доменное имя или ip адрес хоста (П: @"\\192.168.0.39\#pgs" => 192.168.0.39)
        /// </summary>
        public static string GetHostFromUNC(string unc)
        {
            if (!IsUncPath(unc)) throw new ArgumentException("Переданный путь не является UNC");
            var host = unc.Remove(0, 2);
            host = host.Split('\\')[0];
            return host;
        }
    }
}