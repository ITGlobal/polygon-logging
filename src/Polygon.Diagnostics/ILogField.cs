using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Именованное поле для записи в лог
    /// </summary>
    [PublicAPI]
    public interface ILogField : IPrintable
    {
        /// <summary>
        ///     Название поля
        /// </summary>
        [NotNull]
        string Name { get; }
    }
}

