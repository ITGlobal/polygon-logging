namespace Polygon.Diagnostics
{
    internal sealed class PreformattedStringImpl : ILogMessage, IReusable, IObjectPoolItem
    {
        private static readonly ObjectPool<PreformattedStringImpl> _Pool = new ObjectPool<PreformattedStringImpl>(() => new PreformattedStringImpl());
        private PreformattedStringImpl() { }

        private string message;

        public static PreformattedStringImpl Create(string message)
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

