namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Переиспользуемый объект для пула объектов <see cref="ObjectPool{T}"/>
    /// </summary>
    internal interface IObjectPoolItem
    {
        /// <summary>
        ///     Очистить состояния объекта после его использования
        /// </summary>
        void Recycle();
    }
}
