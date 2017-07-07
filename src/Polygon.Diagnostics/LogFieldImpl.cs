namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Именованное поле для записи в лог
    /// </summary>
    internal sealed class LogFieldImpl : ILogField, IReusable, IObjectPoolItem
    {
        private static readonly ObjectPool<LogFieldImpl> _Pool =
            new ObjectPool<LogFieldImpl>(() => new LogFieldImpl());
        private LogFieldImpl() { }

        private string name;
        private object value;
        private string format;
        private string suffix;

        public static LogFieldImpl Create(string name, object value, string format, string suffix)
        {
            var field = _Pool.Acquire();
            field.name = name;
            field.value = value;
            field.format = format;
            field.suffix = suffix;
            return field;
        }

        public string Name => name;
        public string Print(PrintOption option)
        {
            if (suffix != null)
            {
                return $"{name}={FormattingHelper.FormatValue(value, format)}{suffix}";
            }

            return $"{name}={FormattingHelper.FormatValue(value, format)}";
        }

        public override string ToString() => Print(PrintOption.Default);

        public void Release() => _Pool.Release(this);

        public void Recycle()
        {
            name = null;
            value = null;
            format = null;
            suffix = null;
        }
    }

    /// <summary>
    ///     Именованное поле для записи в лог
    /// </summary>
    internal sealed class LogFieldImpl<T> : ILogField, IReusable, IObjectPoolItem
    {
        private static readonly ObjectPool<LogFieldImpl<T>> _Pool =
            new ObjectPool<LogFieldImpl<T>>(() => new LogFieldImpl<T>());
        private LogFieldImpl() { }

        private string name;
        private T value;
        private string format;
        private string suffix;

        public static LogFieldImpl<T> Create(string name, T value, string format, string suffix)
        {
            var field = _Pool.Acquire();
            field.name = name;
            field.value = value;
            field.format = format;
            field.suffix = suffix;
            return field;
        }

        public string Name => name;
        public string Print(PrintOption option)
        {
            if (suffix != null)
            {
                return $"{name}={FormattingHelper.FormatValue(value, format)}{suffix}";
            }

            return $"{name}={FormattingHelper.FormatValue(value, format)}";
        }

        public override string ToString() => Print(PrintOption.Default);

        public void Release() => _Pool.Release(this);

        public void Recycle()
        {
            name = null;
            value = default(T);
            format = null;
            suffix = null;
        }
    }
}

