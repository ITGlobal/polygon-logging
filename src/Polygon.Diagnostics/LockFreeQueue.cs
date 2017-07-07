using System.Threading;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Потокобезопасная lock-free очередь
    /// </summary>
    internal sealed class LockFreeQueue<T>
    {
        private sealed class Node
        {
            public readonly T Item;
            public Node Next;
            public Node(T item)
            {
                Item = item;
            }
        }

        private volatile Node head;
        private volatile Node tail;

        /// <summary>
        ///     Конструктор
        /// </summary>
        public LockFreeQueue()
        {
            head = tail = new Node(default(T));
        }

#pragma warning disable 420 // volatile semantics not lost as only by-ref calls are interlocked

        /// <summary>
        ///     Добавить объект в очередь
        /// </summary>
        public void Enqueue(T item)
        {
            var newNode = new Node(item);
            for (;;)
            {
                var currentTail = tail;
                if (Interlocked.CompareExchange(ref currentTail.Next, newNode, null) == null)   //append to the tail if it is indeed the tail.
                {
                    Interlocked.CompareExchange(ref tail, newNode, currentTail);   //CAS in case we were assisted by an obstructed thread.
                    return;
                }

                Interlocked.CompareExchange(ref tail, currentTail.Next, currentTail);  //assist obstructing thread.
            }
        }

        /// <summary>
        ///     Попытаться удалить объект из очереди
        /// </summary>
        public bool TryDequeue(out T item)
        {
            for (;;)
            {
                var currentHead = head;
                var currentTail = tail;
                var currentHeadNext = currentHead.Next;

                if (currentHead == currentTail)
                {
                    if (currentHeadNext == null)
                    {
                        item = default(T);
                        return false;
                    }

                    Interlocked.CompareExchange(ref tail, currentHeadNext, currentTail);   // assist obstructing thread
                }
                else
                {
                    item = currentHeadNext.Item;
                    if (Interlocked.CompareExchange(ref head, currentHeadNext, currentHead) == currentHead)
                    {
                        return true;
                    }
                }
            }
        }
    }
}
