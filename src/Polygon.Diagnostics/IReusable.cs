namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Временный объект для записи сообщения в лог. После записи должен быть явно высвобожден путем вызова метода Release().
    /// </summary>
    internal interface IReusable
    {
        void Release();
    }
}

