using System;
using System.Text;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Статический класс для создания форматированных сообщений лога
    /// </summary>
    [PublicAPI]
    public static partial class LogMessage
    {
        /// <summary>
        ///     Отключить форматирование для строки при ее записи в лог
        /// </summary> 
        public static IPrintable Preformatted(this string str) => PreformattedStringImpl.Create(str);

        /// <summary>
        ///     Отформатировать сообщение (из $-строки)
        /// </summary>
        public static ILogMessage Format(FormattableString str)
        {
            var formattedString = FormattingHelper.FormatMessage(str);
            return LogMessageImpl.Create(formattedString);
        }

        /// <summary>
        ///     Отформатировать сообщение
        /// </summary>
        [StringFormatMethod("format")]
        public static ILogMessage FormatString(string format, params object[] args)
        {
            var formattedString = FormattingHelper.FormatMessage(format, args);
            return LogMessageImpl.Create(formattedString);
        }

        /// <summary>
        ///     Собрать сообщение лога из $-строки и списка именованных полей
        /// </summary>
        public static ILogMessage Make(FormattableString text, params ILogField[] fields)
        {
            var builder = LocalStringBuilderCache.New();
            FormattingHelper.FormatMessageInPlace(builder, text);

            return MakeCore(builder, fields);
        }

        /// <summary>
        ///     Собрать сообщение лога из строки и списка именованных полей
        /// </summary>
        public static ILogMessage MakeString(string text, params ILogField[] fields)
        {
            var builder = LocalStringBuilderCache.New();
            builder.Append(text);
            return MakeCore(builder, fields);
        }

        private static ILogMessage MakeCore(StringBuilder builder, ILogField[] fields)
        {
            if (fields != null && fields.Length > 0)
            {
                // Автоматическое добавление завершающей точки
                if (builder.Length > 0 && builder[builder.Length - 1] != '.')
                {
                    builder.Append('.');
                }

                // Автоматическое добавление завершающего пробела
                if (builder.Length > 0 && !char.IsWhiteSpace(builder[builder.Length - 1]))
                {
                    builder.Append(' ');
                }

                for (var i = 0; i < fields.Length; i++)
                {
                    builder.Append(fields[i].Print(PrintOption.Default));
                    if (i != fields.Length - 1)
                    {
                        builder.Append(", ");
                    }

                    (fields[i] as IReusable)?.Release();
                }
            }

            return LogMessageImpl.Create(LocalStringBuilderCache.GetStringAndRelease(builder));
        }

        /// <summary>
        ///     Создать объект для записи в лог поля с указанными названием и значением, с автоматическим форматированием значения
        /// </summary>
        [NotNull]
        public static ILogField Field(
            [NotNull] string name, 
            [CanBeNull] object value,
            [CanBeNull] string format = null,
            [CanBeNull] string suffix = null)
            => LogFieldImpl.Create(name, value, format, suffix);

        /// <summary>
        ///     Создать объект для записи в лог поля с указанными названием и значением (значение заменяется звездочками)
        /// </summary>
        [NotNull]
        public static ILogField SecureField([NotNull] string name, [CanBeNull] object value) => SecureLogFieldImpl.Create(name, value);
    }
}

