using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PGS.Comps
{
    /// <summary>
    /// Интерфейс картинка компонента
    /// </summary>
    public interface ICompImage
    {
        /// <summary>
        /// ID компонента в базе
        /// </summary>
        int CompID { get; }
        /// <summary>
        /// Изображение
        /// </summary>
        Image Image { get; }
    }

    /// <summary>
    /// Свойства компонента
    /// </summary>
    public partial class CompInfo : ICompInfo
    {
        /// <summary>
        /// Изображение структурной формулы
        /// </summary>
        public class CompImage : ICompImage
        {
            /// <summary>
            /// Событие объект изменент
            /// </summary>
            public event Action Changed;
            /// <summary>
            /// Поле сохранено в базу
            /// </summary>
            private bool _Saved = true;
            /// <summary>
            /// Сохранено в базу
            /// </summary>
            public bool Saved
            {
                get
                {
                    return _Saved;
                }
            }
            /// <summary>
            /// Есть в базе
            /// </summary>
            private bool InBase = false;

            /// <summary>
            /// Поле ID компонента в базе
            /// </summary>
            private int _CompID;
            /// <summary>
            /// Поле информация о компоненте
            /// </summary>
            private CompInfo _CompInfo = null;
            /// <summary>
            /// ID компонента в базе
            /// </summary>
            public int CompID
            {
                get
                {
                    if (_CompInfo != null)
                    {
                        return _CompInfo.ID;
                    }
                    else return _CompID;
                }
            }

            /// <summary>
            /// Поле изображение
            /// </summary>
            private Image _CompImg = null;
            /// <summary>
            /// Изображение
            /// </summary>
            public Image Image
            {
                get
                {
                    return _CompImg;
                }
                set
                {
                    if (_CompImg != value)
                    {
                        _CompImg = value;
                        _Saved = false;
                        Changed?.Invoke();
                    }
                }
            }

            /// <summary>
            /// Внутренняя для загрузки из базы
            /// </summary>
            /// <param name="compid"></param>
            private CompImage(int compid)
            {
                _CompID = compid;
            }
            /// <summary>
            /// Конструктор связаный с CompInfo
            /// </summary>
            /// <param name="compinfo"></param>
            public CompImage(CompInfo compinfo)
            {
                _CompInfo = compinfo;
            }

            /// <summary>
            /// Кэш загрузки из базы
            /// </summary>
            static private Dictionary<int, CompImage> Cache = new Dictionary<int, CompImage>();
            /// <summary>
            /// Загрзить данные из базы
            /// </summary>
            /// <param name="id">ID компонента в базе</param>
            /// <param name="Forced">Не использовать кэш</param>
            /// <returns></returns>
            public static CompImage Load(int id, bool Forced = false)
            {
                if (Cache.ContainsKey(id) && !Forced)
                {
                    return Cache[id];
                }
                else
                {
                    var con = DB.Connect();
                    try
                    {
                        return Load(id, con);
                    }
                    finally
                    {
                        con.Disconnect();
                    }
                }
            }
            /// <summary>
            /// Загрузить данные из базы
            /// </summary>
            /// <param name="id"></param>
            /// <param name="con"></param>
            /// <returns></returns>
            public static CompImage Load(int id, OdbcConnection con)
            {
                var com = con.CreateCommand();
                com.CommandText = "SELECT image FROM Comps.formulas WHERE compid = " + id;

                var strImg = (string)com.ExecuteScalar();

                CompImage r;
                if (Cache.ContainsKey(id))
                {
                    r = Cache[id];
                }
                else
                {
                    r = new CompImage(id);
                }

                if (strImg != null)
                {
                    r.Image = DB.Base64ToImageFromString(strImg);
                    r.InBase = true;
                }
                r._Saved = true;

                Cache[id] = r;
                return r;
            }
            /// <summary>
            /// Сохранить в базу
            /// </summary>
            /// <param name="con"></param>
            /// <param name="tr"></param>
            public void Save(OdbcConnection con, OdbcTransaction tr)
            {
                if (Saved)
                    return;

                string Q;
                if (Image == null)
                {
                    Q = "DELETE FROM comps.formulas WHERE compid = {0}";
                }
                else
                {
                    if (!InBase)
                    {
                        Q = @"INSERT INTO comps.formulas (compid, image) VALUES({0}, {1})";
                    }
                    else
                    {
                        Q = @"UPDATE comps.formulas SET image = {1} WHERE compid = {0}";
                    }
                }
                OdbcCommand cm = new OdbcCommand("", con, tr);

                cm.CommandText = string.Format(DB.CultureSQL, Q, CompID, DB.ImageToBase64(Image));

                cm.ExecuteNonQuery();
                InBase = (Image != null);
                _Saved = true;
            }
            /// <summary>
            /// Удалить из базы
            /// </summary>
            /// <param name="id"></param>
            /// <param name="con"></param>
            /// <param name="tr"></param>
            public static void Delete(int id, OdbcConnection con, OdbcTransaction tr)
            {
                OdbcCommand cm = new OdbcCommand("", con, tr);
                cm.CommandText = "DELETE FROM comps.formulas WHERE compid = " + id;
                cm.ExecuteNonQuery();
            }



            internal static object FileToImage(string fileName)
            {
                throw new NotImplementedException();
            }
        }
    }
}