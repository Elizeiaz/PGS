using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using BOOL = System.Boolean;
using DWORD = System.UInt32;
using LPWSTR = System.String;
using NET_API_STATUS = System.UInt32;

namespace PGS.DMS.Controllers
{
    /// <summary>
    /// Класс предоставляет функции подключения/отключения сетевого диска.
    /// </summary>
    public class NetworkDriver : IDisposable
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct USE_INFO_2
        {
            internal string ui2_local;
            internal string ui2_remote;
            internal string ui2_password;
            internal uint ui2_status;
            internal uint ui2_asg_type;
            internal uint ui2_refcount;
            internal uint ui2_usecount;
            internal string ui2_username;
            internal string ui2_domainname;
        }

        internal enum ParmError
        {
            TROUBLE_IN_LOCAL = 1,
            TROUBLE_IN_REMOTE = 2,
            TROUBLE_IN_PASSWORD = 3,
            TROUBLE_IN_ASGTYPE = 4,
            TROUBLE_IN_USERNAME = 5,
            TROUBLE_IN_DOMAINNAME = 6,
            TROUBLE_IN_UNC = 7
        }

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern uint NetUseAdd(
            string UncServerName,
            uint Level,
            ref USE_INFO_2 Buf,
            out uint ParmError);

        [DllImport("NetApi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern uint NetUseDel(
            string UncServerName,
            string UseName,
            uint ForceCond);

        private bool disposed = false;
        private bool _withDisconnect;

        private string _sUncPath;
        private string _sUser;
        private string _sPassword;
        private string _sDomain;
        private int _iLastError;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="withDisconnect">С разрывом соединения.</param>
        public NetworkDriver(bool withDisconnect = false)
        {
            _withDisconnect = withDisconnect;
        }

        /// <summary>
        /// Код последней ошибки Win32Exception.  Успешно = 0
        /// </summary>
        public int LastError => _iLastError;

        public void Dispose()
        {
            if (!disposed && _withDisconnect) NetUseDelete();
            disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Подключить сетевой путь.
        /// </summary>
        /// <param name="uncPath"></param>
        /// <param name="user"></param>
        /// <param name="domain"></param>
        /// <param name="password"></param>
        /// <returns>Код ошибки Win32Exception.</returns>
        public int NetUseWithCredentials(string uncPath, string user, string domain, string password)
        {
            if (!Unc.IsUncPath(uncPath))
                throw new Win32Exception($"Не верно указан параметр:\n{(ParmError)7}");

            _sUncPath = uncPath;
            _sUser = user;
            _sPassword = password;
            _sDomain = domain;
            return NetUseWithCredentials();
        }

        private int NetUseWithCredentials()
        {
            try
            {
                var useinfo = new USE_INFO_2
                {
                    ui2_remote = _sUncPath,
                    ui2_username = _sUser,
                    ui2_domainname = _sDomain,
                    ui2_password = _sPassword,
                    ui2_asg_type = 0,
                    ui2_usecount = 1
                };

                uint paramErrorIndex;
                var returncode = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
                if (paramErrorIndex != 0)
                    throw new Win32Exception($"Не верно указан параметр:\n{(ParmError)paramErrorIndex}");
                return (int)returncode;
            }
            catch (Win32Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Создать соединение с сетевым диском.
        /// </summary>
        public static int Connect(string unc, NetworkCredential credential)
        {
            if (!Unc.IsUncPath(unc))
                throw new Win32Exception($"Не верно указан параметр:\n{(ParmError)7}");

            try
            {
                var useinfo = new USE_INFO_2
                {
                    ui2_remote = unc,
                    ui2_username = credential.UserName,
                    ui2_domainname = credential.Domain,
                    ui2_password = credential.Password,
                    ui2_asg_type = 0,
                    ui2_usecount = 1
                };

                uint paramErrorIndex;
                var returncode = NetUseAdd(null, 2, ref useinfo, out paramErrorIndex);
                if (paramErrorIndex != 0)
                    throw new Win32Exception($"Не верно указан параметр:\n{(ParmError)paramErrorIndex}");
                return (int)returncode;
            }
            catch (Win32Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Разорвать соединение с сетевым диском.
        /// </summary>
        public static int Disconnect(string uncPath)
        {
            var returncode = NetUseDel(null, uncPath, 2);
            return (int)returncode;
        }

        /// <summary>
        /// Разорвать соединение с сетевым диском.
        /// </summary>
        public int NetUseDelete()
        {
            try
            {
                var returncode = NetUseDel(null, _sUncPath, 2);
                _iLastError = (int)returncode;
                return (int)returncode;
            }
            catch
            {
                _iLastError = Marshal.GetLastWin32Error();
                throw new Win32Exception(_iLastError);
            }
        }

        ~NetworkDriver()
        {
            Dispose();
        }
    }
}