

using System;

namespace PGS.Types
{
    /// <summary>
    /// Обобщенный класс для реализации индексированных свойств
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ArrayEx<T>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="GetValue">Метод get</param>
        /// <param name="SetValue">Метод set</param>
        /// <param name="GetCount">Метод count</param>
        public ArrayEx(Func<int, T> GetValue, Action<int, T> SetValue, Func<int> GetCount)
        {
            if ((GetValue == null) || (SetValue == null))
            {
                throw new ArgumentNullException("GetValue or SetValue can't by null");
            }
            this.GetValue += GetValue;
            this.SetValue += SetValue;
            this.GetCount += GetCount;
        }
        /// <summary>
        /// Событие set
        /// </summary>
        private event Action<int, T> SetValue;
        /// <summary>
        /// Событие get 
        /// </summary>
        private event Func<int, T> GetValue;
        /// <summary>
        /// Событие count
        /// </summary>
        private event Func<int> GetCount;
        /// <summary>
        /// Элемент в заданной позиции
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public T this[int i]
        {
            get
            {
                return GetValue.Invoke(i);
            }
            set
            {
                SetValue?.Invoke(i, value);
            }
        }
        /// <summary>
        /// Количество элементов
        /// </summary>
        public int Count
        {
            get
            {
                return GetCount.Invoke();
            }
        }
    }
}