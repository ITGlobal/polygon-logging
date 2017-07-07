namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Отформатированное сообщение для записи в лог
    /// </summary>
    internal sealed class LogMessageImpl : ILogMessage, IReusable, IObjectPoolItem
    {
        private static readonly ObjectPool<LogMessageImpl> _Pool = new ObjectPool<LogMessageImpl>(() => new LogMessageImpl());
        private LogMessageImpl() { }

        private string message;

        public static LogMessageImpl Create(string message)
        {
            var msg = _Pool.Acquire();
            msg.message = message;
            return msg;
        }
        
        public string Print(PrintOption option) => message;
        public override string ToString() => Print(PrintOption.Default);
        public void Release() => _Pool.Release(this);
        public void Recycle() => message = null;
    }
}

