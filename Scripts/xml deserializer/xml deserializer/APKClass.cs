using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace xml_deserializer
{
    /// <summary>
    /// Интерфейс взаимодействия с АПК ПГС
    /// </summary>
    [Guid("26718F52-B4FD-40F5-99EC-2BD6CBEB694E")]
    [ComVisible(true)]
    public interface IAPK
    {
        /// <summary>
        /// Добавит заказ в производство
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool AddToProduction(string order);
        /// <summary>
        /// Удалиь заказ из производства
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool RemoveFromProduction(string order);

        /// <summary>
        /// Проверка заказа
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        bool Check(string order);
        /// <summary>
        /// Показать список смесей с проблемами
        /// </summary>
        /// <param name="order"></param>
        void ShowProblemList(string order);
    }

    /// <summary>
    /// Класс АПК
    /// </summary>
    [Guid("A0240C8E-2A81-41D5-BE63-CCB9D49B1543")]
    [ClassInterface(ClassInterfaceType.None)]
    public class APK : IAPK
    {
        /// <summary>
        /// Добавит заказ в производство
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool AddToProduction(string order)
        {
            var orders = Order.ParseXML(order);
            
            throw new NotImplementedException();
        }
        /// <summary>
        /// Удалиь заказ из производства
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool RemoveFromProduction(string order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверка заказа
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public bool Check(string order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Показать список смесей с проблемами
        /// </summary>
        /// <param name="order"></param>
        public void ShowProblemList(string order)
        {
            throw new NotImplementedException();
        }


        private bool AddInProduction(List<Order> orders)
        {
            throw new NotImplementedException();
        }
    }
}
