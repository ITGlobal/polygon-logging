using System;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Токен цепочки асинхронных событий
    /// </summary>
    [PublicAPI]
    public struct AsyncMethodToken : IDisposable
    {
        public void Dispose() => AsyncScope.Pop();
    }
}

