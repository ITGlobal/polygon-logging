using System;
using System.Threading;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Пул переиспользуемых объектов
    /// </summary>
    internal sealed class ObjectPool<T>
        where T : class, IObjectPoolItem
    {
        private readonly LockFreeQueue<T> objectQueue = new LockFreeQueue<T>();
        private readonly Func<T> factory;

        private int count;
        private int acquiredCount;

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="factory">
        ///     Фабрика для новых объектов
        /// </param>
        /// <param name="capacity">
        ///     Изначальный размер пула
        /// </param>
        public ObjectPool(Func<T> factory, int capacity = 0)
        {
            this.factory = factory;
            EnsureCapacity(capacity);
        }

        /// <summary>
        ///     Общее число объектов в пуле
        /// </summary>
        public int Count => Interlocked.CompareExchange(ref count, 0, 0);

        /// <summary>
        ///     Число объектов в пуле, которые в данный момент используются
        /// </summary>
        public int AcquiredCount => Interlocked.CompareExchange(ref acquiredCount, 0, 0);

        /// <summary>
        ///     Число свободных объектов в пуле
        /// </summary>
        public int AvailableCount => Count - AcquiredCount;

        /// <summary>
        ///     Расширить пул до указанного размера
        /// </summary>
        public void EnsureCapacity(int capacity)
        {
            while (count < capacity)
            {
                objectQueue.Enqueue(factory());
                Interlocked.Increment(ref count);
            }
        }

        /// <summary>
        ///     Получить объект из пула. При необходимости размер пула будет увеличен.
        /// </summary>
        public T Acquire()
        {
            T obj;
            if (!objectQueue.TryDequeue(out obj))
            {
                obj = factory();
                Interlocked.Increment(ref count);
            }

            Interlocked.Increment(ref acquiredCount);

            return obj;
        }

        /// <summary>
        ///     Вернуть объект в пул
        /// </summary>
        public void Release(T obj)
        {
            obj.Recycle();

            objectQueue.Enqueue(obj);
            Interlocked.Decrement(ref acquiredCount);
        }
    }
}
