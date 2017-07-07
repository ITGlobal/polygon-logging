namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Именованное поле для записи пароля в лог (заменяется звездочками)
    /// </summary>
    internal sealed class SecureLogFieldImpl : ILogField, IReusable, IObjectPoolItem
    {
        private static readonly ObjectPool<SecureLogFieldImpl> _Pool =
            new ObjectPool<SecureLogFieldImpl>(() => new SecureLogFieldImpl());
        private SecureLogFieldImpl() { }

        private string name;
        private object value;

        public static SecureLogFieldImpl Create(string name, object value)
        {
            var field = _Pool.Acquire();
            field.name = name;
            field.value = value;
            return field;
        }

        public string Name => name;
        public string Print(PrintOption option)
        {
            var str = FormattingHelper.FormatValue(value) ?? string.Empty;
            var stars = new string('*', str.Length);
            return $"{name}={stars}";
        }

        public override string ToString() => Print(PrintOption.Default);

        public void Release() => _Pool.Release(this);

        public void Recycle()
        {
            name = null;
            value = null;
        }
    }
}

