using System;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Событие лога
    /// </summary>
    [PublicAPI]
    public struct LogEvent
    {
        /// <summary>
        ///     Время
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        ///     Номер потока
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        ///     Название потока
        /// </summary>
        [CanBeNull]
        public string ThreadName { get; set; }

        /// <summary>
        ///     Полное название логгера
        /// </summary>
        [NotNull]
        public string LoggerName { get; set; }

        /// <summary>
        ///     Название логгера
        /// </summary>
        [NotNull]
        public string MethodName { get; set; }

        /// <summary>
        ///     Номер строки
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        ///     Уровень логирования
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        ///     Идентификатор цепочки операций
        /// </summary>
        public ushort ScopeId { get; set; }

        /// <summary>
        ///     Глубина вложенности операций
        /// </summary>
        public int Depth { get; set; }

        /// <summary>
        ///     Сообщение
        /// </summary>
        [NotNull]
        public string Message { get; set; }

        /// <summary>
        ///     Исключение
        /// </summary>
        [CanBeNull]
        public Exception Exception { get; set; }
    }
}

