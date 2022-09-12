using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using PGS;

namespace xml_deserializer
{
    /// <summary>
    /// Заказ покупателя
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Номер заказа
        /// </summary>
        public string Number { get; set; } = "";
        
        /// <summary>
        /// Дата заказа
        /// </summary>
        public DateTime Date { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Дата начала производства
        /// </summary>
        public DateTime InProduce { get; set; } = DateTime.Now;
        
        /// <summary>
        /// Заказчик
        /// </summary>
        public string Customer { get; set; } = "";

        private bool _international = false;
        /// <summary>
        /// Международный заказ
        /// </summary>
        public bool International
        {
            get
            {
                return _international;
            }
            set
            {
                if (_international != value)
                {
                    _international = value;
                    foreach (Mix m in Mixes)
                    {
                        m.asMSO = value;
                    }
                }
            }
        }

        public Order(string number, DateTime date, DateTime inProduce, string customer, bool international = false)
        {
            Number = number;
            Date = date;
            InProduce = inProduce;
            Customer = customer;
            International = international;
        }

        /// <summary>
        /// Класс смесь заказа
        /// </summary>
        public class Mix
        {
            /// <summary>
            /// Количество заказанных смесей, шт.
            /// </summary>
            public int Quantity { get; set; } = 0;
            
            /// <summary>
            /// Единицы измерения концентрации заказа
            /// </summary>
            public PGS.Concentration.Units OrderUnits = PGS.Concentration.Units.Molar;
            
            /// <summary>
            /// Рег. номер стандатра смеси
            /// </summary>
            public string RegNumber { get; set; } = "";
            
            /// <summary>
            /// Рег.номер как МСО
            /// </summary>
            public bool asMSO { get; set; } = false;

            /// <summary>
            /// Объем баллона, л
            /// </summary>
            public double Volume { get; set; } = double.NaN;
          
            /// <summary>
            /// Тип баллона
            /// </summary>
            public int CylType { get; set; } = 1;
            
            /// <summary>
            /// Тип вентиля
            /// </summary>
            public int? ValveType { get; set; } = null;

            /// <summary>
            /// Класс компонента смеси
            /// </summary>
            public class Comp
            {
                /// <summary>
                /// ID компонента
                /// </summary>
                public int CompID { get; set; }
                
                /// <summary>
                /// Заказанная концентрация в единицах заказчика
                /// </summary>
                public double OrderConc { get; set; } = double.NaN;

                /// <summary>
                /// Разбавитель
                /// </summary>
                public bool Diluent { get; set; } = false;

                /// <summary>
                /// Концентрация в мол. %
                /// </summary>
                public double Conc { get; set; } = double.NaN;

                /// <summary>
                /// Конвертирует концентрацию в %
                /// </summary>
                private double ConcConverter(string unit, double conc)
                {
                    switch (unit)
                    {
                        case "%":
                            return conc;
                        case "ppm":
                            return conc / 10000;
                        default:
                            throw new Exception($"Отстутствует реализация для {unit}");
                    }
                }
                
#warning Нормально ли конвертация из Kod в CompID в констукторе?
                /// <param name="kod"></param>
                /// <param name="units">Вид представления концентрации (%, ppm)</param>
                /// <param name="conc"></param>
                public Comp(int kod, string units, double conc)
                {
                    CompID = _1C.GetID(kod);
                    OrderConc = conc;
                    Conc = ConcConverter(units, conc);
                }

                /// <summary>
                /// Компонент - разбавитель
                /// </summary>
                /// <param name="kod"></param>
                public Comp(int kod)
                {
                    CompID = _1C.GetID(kod);
                    Diluent = true;
                }
            }
            
            public Mix(int quantity, string regNumber, double volume, int complectationKod,
                PGS.Concentration.Units orderUnits = Concentration.Units.Molar)
            {
                Quantity = quantity;
                RegNumber = regNumber;
                Volume = volume;
                
                var complectation = Complectation.GetComplectation(complectationKod);
                CylType = complectation.CylType;
                ValveType = complectation.ValveType;
            }
            
            /// <summary>
            /// Список компонентов смеси
            /// </summary>
            public List<Comp> Comps = new List<Comp>();

            /// <summary>
            /// Загрузка смеси из XML
            /// </summary>
            /// <param name="xml"></param>
            /// <returns></returns>
            static public Mix ParseXML(string xml)
            {
                XElement x = XElement.Parse(xml);
                return ParseXML(x);
            }

            /// <summary>
            /// Загрузка смеси из XML
            /// </summary>
            /// <param name="xml"></param>
            /// <returns></returns>
            static public Mix ParseXML(XElement xml)
            {
                var cylXML = xml.Element("Cyl");
                
                var quantity = (int) xml.Attribute("Quantity");
                var units = (string) xml.Attribute("Units");
                var regNum = (string) xml.Attribute("RegNum");
                var volume = double.Parse(cylXML.Attribute("Volume").Value, CultureInfo.CurrentCulture);
                var compKod = (int) cylXML.Attribute("Complectation_Kod");
                
                var mix = new Mix(quantity, regNum, volume, compKod);

                //Заполнение листа Comp
                foreach (var comp in xml.Elements("Comp"))
                {
                    if (comp.Attribute("Conc") != null)
                    {
                        mix.Comps.Add(new Comp((int) comp.Attribute("Kod"), units, double.Parse(comp.Attribute("Conc").Value, CultureInfo.CurrentCulture)));
                    }
                    else
                    {
                        mix.Comps.Add(new Comp((int) comp.Attribute("Kod")));
                    }
                }

                return mix;
            }
        }
        /// <summary>
        /// Список смесей в заказе
        /// </summary>
        public List<Mix> Mixes = new List<Mix>();

        /// <summary>
        /// Создание списка заказов из xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        static public List<Order> ParseXML(string xml)
        {
            XElement x = XElement.Parse(xml);
            return ParseXML(x);
        }
        
        /// <summary>
        /// Создание списка заказов из xml
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        static public List<Order> ParseXML(XElement xml)
        {
            //Трансляция Код_1С в CompID 
            //ParseXML есть в Mix
            var orderList = new List<Order>();
            
            foreach (var order in xml.Elements("Order"))
            {
                Order currentOrder;
                
                var number = (string) order.Attribute("Number");
#warning В итоговой версии изменить Data на Date
                var date = DateTime.Parse(order.Attribute("Data").Value, CultureInfo.CurrentCulture);
                var inProduce = DateTime.Parse(order.Attribute("InProduce").Value, CultureInfo.CurrentCulture);
                var customer = (string) order.Attribute("Customer");
                if (order.Attribute("International") is null)
                {
                    currentOrder = new Order(number, date, inProduce, customer);
                }
                else
                {
                    currentOrder = new Order(
                        number, date, inProduce, customer,
                        (bool) order.Attribute("International"));
                }

                foreach (var mix in order.Elements("Mix"))
                {
                    currentOrder.Mixes.Add(Mix.ParseXML(mix));
                }
                
                orderList.Add(currentOrder);
            }

            return orderList;
        }
    }

#warning Похоже неправильно понял структуру класса, т.к. отсутствовали методы взаимодействия. Добавил GetComplectation
    /// <summary>
    /// Комплектации баллонов из 1С
    /// </summary>
    internal class Complectation
    {
        /// <summary>
        /// Кэш комплектаций
        /// </summary>
        private static Dictionary<int, Complectation> _Cache = new Dictionary<int, Complectation>();

        /// <summary>
        /// Загрузка кэша комплектаций
        /// </summary>
        static public void LoadCache()
        {
            _Cache.Clear();
            
            using (var con = PGS.DB.Connect())
            {
                
                var command = con.CreateCommand();
                command.CommandText = $"SELECT kod_1c, cyltype, valvetype FROM cyls.complectations_1c";

                try
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var complectation = LoadComplectation(reader);
                        _Cache.Add(complectation.Kod_1C, complectation);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

#warning Для чего нужен данный метод? Если нет в _Cache - значит нет и в базе => данный метод ничего не вернёт
        /// <summary>
        /// Загрузка комплектации по коду
        /// </summary>
        /// <param name="kod_1c"></param>
        /// <returns></returns>
        private static Complectation LoadComplectation(int kod_1c)
        {
            if (_Cache.ContainsKey(kod_1c))
            {
                return _Cache[kod_1c];
            }
    
            using (var con = PGS.DB.Connect())
            {
                var command = con.CreateCommand();
                command.CommandText = $"SELECT kod_1c, cyltype, valvetype FROM cyls.cyls";

                try
                {
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var kod = (int) reader["kod_1c"];
                        var cylType = (int) reader["cyltype"];
                        var valveType = reader["valvetype"] as int?;
                        
                        var complectation = new Complectation(kod, cylType, valveType);
                        _Cache.Add(kod_1c, complectation);
                        return complectation;
                    }
                    
                    throw new Exception($"Отсутствует комплектация с кодом: {kod_1c}");
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        /// <summary>
        /// Загрузка из базы
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static Complectation LoadComplectation(OdbcDataReader dr)
        {
            return new Complectation(
                (int) dr["kod_1c"],
                (int) dr["cyltype"], 
                dr["valvetype"] as int?);
        }

        public static Complectation GetComplectation(int kod)
        {
            try
            {
                return LoadComplectation(kod);
//                return _Cache[kod];
            }
            catch (Exception e)
            {
                throw new Exception("Отсутствует kod_1C");
            }
        }

        static Complectation()
        {
            LoadCache();
        }
        public Complectation(int Kod_1C, int CylType, int? ValveType)
        {
            this.Kod_1C = Kod_1C;
            this.CylType = CylType;
            this.ValveType = ValveType;
        }
        /// <summary>
        /// Код комплектации из 1С
        /// </summary>
        public int Kod_1C { get; }
        
        /// <summary>
        /// Тип баллона
        /// </summary>
        public int CylType { get; }
        
        /// <summary>
        /// Тип вентиля
        /// </summary>
        public int? ValveType { get; }
    }

    

    /// <summary>
    /// Класс для концертации кода компонента 1С в ID БД
    /// </summary>
    public static class _1C
    {//TODO: Потом класс будет перенесен в DBCommon в PGS.Comps

        // Ключ - id газа, значение - kod_1c
#warning Изменил int на int? т.к. не у каждого газа есть эквивалентный код 1C
        static private Dictionary<int, int?> _Cache_ID = new Dictionary<int, int?>();
        // Ключ - kod_1c, значение - id газа
        static private Dictionary<int, int> _Cache_Kod = new Dictionary<int, int>();

        static public void LoadCaches()
        {
            //TODO: Загрузка данных из базы
            using (var con = PGS.DB.Connect())
            {
                
                var command = con.CreateCommand();
                command.CommandText = $"SELECT id, kod_1c FROM comps.comps";

                try
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = (int) reader["id"];
                        var kod = reader["kod_1c"] as int?;
                        
                        _Cache_ID.Add(id, kod);
                        
                        //Не добавляю элементы, если значение у kod отсутствует
                        if (kod is null) continue;
                        
                        _Cache_Kod.Add((int) kod, id);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        static _1C()
        {
            LoadCaches();
        }

        /// <summary>
        /// Получить код компонента в 1С по ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public int GetKod(int id)
        {
#warning Изменить Exception на более корректное
            //Или резуьтат или исключение 
            if (_Cache_ID.ContainsKey(id))
            {
                var temp = _Cache_ID[id];
                if (temp != null)
                {
                    return (int) temp;
                }
                throw new Exception($"Отсутствие значения для id: {id}");
            }

            throw new Exception($"Отсутствие ключа для {id}");
        }

        /// <summary>
        /// Получить ID по коду компонента в 1С
        /// </summary>
        /// <param name="kod"></param>
        /// <returns></returns>
        static public int GetID(int kod)
        {
#warning Изменить Exception на более корректное
            //Или резуьтат или исключение 
            if (_Cache_Kod.ContainsKey(kod)) return _Cache_Kod[kod];

            throw new Exception($"Отсутствие ключа для kod: {kod}");
        }
    }
}
