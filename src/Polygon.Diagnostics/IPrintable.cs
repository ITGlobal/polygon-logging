using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Объект, реализующий этот интерфейс, умеет адекватно выводить себя в лог
    /// </summary>
    [PublicAPI]
    public interface IPrintable
    {
        /// <summary>
        ///     Вывести объект в лог
        /// </summary>
        [NotNull]
        string Print(PrintOption option);
    }
}

