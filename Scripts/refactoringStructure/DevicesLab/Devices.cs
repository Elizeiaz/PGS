using PGS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Windows.Forms;

namespace DevicesLab
{
    public class Device
    {
        /// <summary>
        /// Коллекция всех приборов
        /// </summary>
        private static List<Device> _DevicesCollection { get; } = new List<Device>();

        public static ReadOnlyCollection<Device> DevicesCollection => _DevicesCollection.AsReadOnly();

        #region Методы DevicesCollection

        public static string GetDeviceName(int ID)
        {
            var dev = _DevicesCollection.Find(x => x.ID == ID);
            return dev != null ? dev.Name : "?";
        }

        #endregion

        public int ID { get; }
        public string Name { get; }

        public List<ChannelDetector> ChannelDetectors { get; set; } = new List<ChannelDetector>();

        public Device(int id, string name, List<ChannelDetector> channelDetectors)
        {
            ID = id;
            Name = name;
            ChannelDetectors = channelDetectors;
        }

        /// <summary>
        /// Возвращает все детекторы на переданном канале
        /// </summary>
        /// <param name="channelId">id канала</param>
        public ChannelDetector[] GetChannelDetectors(int channelId)
        {
            return ChannelDetectors.Where(x => x.ID == channelId).ToArray();
        }

        /// <summary>
        /// Возвраващет все детекторы на переданном канале
        /// </summary>
        ChannelDetector[] this [int? channelId]
        {
            get => ChannelDetectors.Where(det => det.Channel == channelId).ToArray();
        }

        static Device()
        {
            LoadDevices();
            ChannelDetector.LoadDetectors();
        }

        public class ChannelDetector
        {
            /// <summary>
            /// Коллекция всех детекторов
            /// </summary>
            private static readonly List<ChannelDetector> _DetectorsCollection = new List<ChannelDetector>();
            public static ReadOnlyCollection<ChannelDetector> DetectorsCollection => _DetectorsCollection.AsReadOnly();

            #region Методы DetectorCollection

            /// <summary>
            /// Находит детектор по названию
            /// </summary>
            [Obsolete("ПРЕДУПРЕЖДЕНИЕ! Название детектора не уникально")]
            public static ChannelDetector[] SearchByName(string name)
            {
                return _DetectorsCollection.Where(det => det.Name == name).ToArray();
            }

            /// <summary>
            /// Находит детектор по id
            /// </summary>
            public static ChannelDetector SearchById(int id)
            {
                return _DetectorsCollection.FirstOrDefault(det => det.ID == id);
            }

            #endregion

            public int ID { get; }
            public string Name { get; }
            public int? Channel { get; }
            public string Description { get; }
            public int? CompId { get; }

            public ChannelDetector(int id, string name, int? channel, string description, int? compId)
            {
                ID = id;
                Name = name;
                Channel = channel;
                Description = description;
                CompId = compId;
            }

            #region Работа с БД

            /// <summary>
            /// Заполнение/обновление DetectorsCollection
            /// </summary>
            public static void LoadDetectors()
            {
                OdbcConnection con = con = new OdbcConnection();
                con.ConnectionString = DB.ConnectionString;

                try
                {
                    con.Open();
                    LoadDetectors(con);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }

            /// <summary>
            /// Заполнение/обновление DetectorsCollection
            /// </summary>
            public static void LoadDetectors(OdbcConnection con)
            {
                ChannelDetector._DetectorsCollection.Clear();

                //Загрузим все детекторы
                IDbCommand cm_det = con.CreateCommand();
                cm_det.CommandText = GetAllDetectors;

                var dr = cm_det.ExecuteReader();

                dr = cm_det.ExecuteReader();
                while (dr.Read())
                {
                    var id = (int)dr["ID"];
                    var name = (string)dr["Name"];
                    int? channel = null;
                    if (dr["channel"] != DBNull.Value)
                        channel = (int)dr["channel"];
                    var description = (string)dr["description"];
                    int? compId = null;
                    if (dr["compId"] != DBNull.Value)
                        channel = (int)dr["compId"];

                    ChannelDetector._DetectorsCollection.Add(new ChannelDetector(id, name, channel, description, compId));
                }
            }

            #endregion

        }

        #region Работа с БД

        #region SQL Querys

        private static readonly string GetAllDevices = @"
SELECT id, name
FROM lab.devices
";

        private static readonly string GetAllDetectors = @"
SELECT 
id,
device,
channel,
name,
description,
compid
FROM lab.channels_detectors
";

        private static readonly string GetDetectorsByDeviceId = @"
SELECT 
id,
device,
channel,
name,
description,
compid
FROM lab.channels_detectors
WHERE device = ?
";

        #endregion

        public static void LoadDevices()
        {
            OdbcConnection con = con = new OdbcConnection();
            con.ConnectionString = DB.ConnectionString;

            try
            {
                con.Open();
                LoadDevices(con);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        public static void LoadDevices(OdbcConnection con)
        {
            _DevicesCollection.Clear();

            IDbCommand cm_dev = con.CreateCommand();
            cm_dev.CommandText = GetAllDevices;
            IDbCommand cm_det = con.CreateCommand();
            cm_det.CommandText = GetDetectorsByDeviceId;

            var dr = cm_dev.ExecuteReader();
            while (dr.Read())
            {
                var id = (int)dr["id"];
                var name = (string)dr["name"];

                var dets = new List<ChannelDetector>();
                cm_det.Parameters.Clear();
                cm_det.Parameters.Add(new OdbcParameter("?", id));

                var drdet = cm_det.ExecuteReader();
                while (drdet.Read())
                {
                    var det_id = (int)dr["ID"];
                    var det_name = (string)dr["Name"];
                    int? channel = null;
                    if (dr["channel"] != DBNull.Value)
                        channel = (int)dr["channel"];
                    var description = (string)dr["description"];
                    int? compId = null;
                    if (dr["compId"] != DBNull.Value)
                        channel = (int)dr["compId"];
                    var det = new ChannelDetector(det_id, det_name, channel, description, compId);
                    dets.Add(det);
                }

                drdet.Close();

                var d = new Device(id, name, dets);
                _DevicesCollection.Add(d);
            }
        }



        #endregion
    }
}