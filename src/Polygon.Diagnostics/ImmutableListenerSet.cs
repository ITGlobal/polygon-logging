using System;
using System.Diagnostics;
using System.Linq;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Иммутабельная коллекция приемников сообщений лога
    /// </summary>
    internal sealed class ImmutableListenerSet : ILogListener
    {
        /// <summary>
        ///     Пустая коллекция
        /// </summary>
        public static readonly ImmutableListenerSet Empty = new ImmutableListenerSet(new ILogListener[0]);

        private readonly ILogListener[] listeners;
        
        private ImmutableListenerSet(ILogListener[] listeners)
        {
            this.listeners = listeners;
        }

        /// <summary>
        ///     Добавить приемник к коллекции
        /// </summary>
        /// <param name="listener">
        ///     Приемник сообщений лога
        /// </param>
        /// <returns>
        ///     Новая коллекция
        /// </returns>
        public ImmutableListenerSet Add(ILogListener listener) => new ImmutableListenerSet(listeners.Concat(new[] { listener }).ToArray());

        /// <summary>
        ///     Записать событие в лог
        /// </summary>
        public void Write(ref LogEvent e)
        {
            foreach (var listener in listeners)
            {
                // Передаем событие приемникам
                try
                {
                    listener.Write(ref e);
                }
                catch (Exception ex) 
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
        }
    }
}

