using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PGS.Types
{

    /// <summary>
    /// Интерфейс события Changed
    /// </summary>
    public interface IEventChanged
    {
        /// <summary>
        /// Событие объект изменен
        /// </summary>
        event Action Changed;
    }

    /// <summary>
    /// Список с событием Changed.
    /// Событие возникает при изменении списка.
    /// Если объекты реализуют интерфейс IEventChanged отслеживается событие Changed объектов.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListEx<T> : List<T>
    {


#warning Не все методы меняющие список перекрыты.

        /// <summary>
        /// Событие при изменении списка
        /// </summary>
        public event Action Changed;

        /// <summary>
        /// Вызвать событие изменения списка
        /// </summary>
        private void InvokeChanged()
        {
            Changed?.Invoke();
        }
        /// <summary>
        /// Событие при добавлени элемента списка
        /// </summary>
        public event Action<T> Added;

        /// <summary>
        /// Вызвать событие добавления в список
        /// </summary>
        /// <param name="item"></param>
        private void InvokeAdded(T item)
        {
            if (item is IEventChanged)
            {
                (item as IEventChanged).Changed += InvokeChanged;
            }
            Added?.Invoke(item);

        }
        /// <summary>
        /// Событие при удалении элемента списка
        /// </summary>
        public event Action<T> Removed;
        /// <summary>
        /// Вызвать событие удаления из списка
        /// </summary>
        /// <param name="item"></param>
        private void InvokeRemoved(T item)
        {
            Removed?.Invoke(item);
            if (item is IEventChanged)
            {
                (item as IEventChanged).Changed -= InvokeChanged;
            }
        }
        /// <summary>
        /// Добавляет объект в конец коллекции
        /// </summary>
        /// <param name="item"></param>
        new public void Add(T item)
        {
            base.Add(item);
            InvokeAdded(item);
            Changed?.Invoke();
        }
        /// <summary>
        /// Добавляет элементы указанной коллекции в конец списка System.Collections.Generic.List`1.
        /// </summary>
        /// <param name="collection"></param>
        new public void AddRange(System.Collections.Generic.IEnumerable<T> collection)
        {
            base.AddRange(collection);
            foreach (var item in collection)
            {
                Added?.Invoke(item);
                InvokeAdded(item);
            }
            Changed?.Invoke();
        }
        /// <summary>
        /// Добавляет элемент в список в позицию с указанным индексом
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        new public void Insert(int index, T item)
        {
            base.Insert(index, item);
            InvokeAdded(item);
            Changed?.Invoke();
        }
        /// <summary>
        /// Удалет первое входжение указанного объекта из коллекции
        /// </summary>
        /// <param name="item"></param>
        new public void Remove(T item)
        {
            InvokeRemoved(item);
            base.Remove(item);
            Changed?.Invoke();
        }
        /// <summary>
        /// Удаляет элемент списка с указанным индексом
        /// </summary>
        /// <param name="index"></param>
        new public void RemoveAt(int index)
        {
            InvokeRemoved(base[index]);
            base.RemoveAt(index);
            Changed?.Invoke();
        }
        /// <summary>
        /// Удаляет все элементы из коллекции
        /// </summary>
        new public void Clear()
        {
            foreach (var item in this)
            {
                InvokeRemoved(item);
            }
            base.Clear();
            Changed?.Invoke();
        }

        /// <summary>
        /// Индексатор возвращает элемент с заданным индексом
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        new public T this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                base[index] = value;
                Changed?.Invoke();
            }
        }
    }
}
