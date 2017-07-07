using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Режим вывода объекта типа <see cref="IPrintable"/>
    /// </summary>
    [PublicAPI]
    public enum PrintOption
    {
        /// <summary>
        ///     Обычный режим
        /// </summary>
        Default,

        /// <summary>
        ///     Вывод вложенного объекта
        /// </summary>
        Nested
    }
}

