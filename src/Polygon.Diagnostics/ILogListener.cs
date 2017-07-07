using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Приемник событий лога
    /// </summary>
    [PublicAPI]
    public interface ILogListener
    {
        /// <summary>
        ///     Записать событие в лог
        /// </summary>
        void Write(ref LogEvent e);
    }
}

