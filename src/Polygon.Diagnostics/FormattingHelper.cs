#define USE_CUSTOM_STRING_FORMATTER
using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Вспомогательный класс для форматирования различных объектов при записи в лог
    /// </summary>
    [PublicAPI]
    public static partial class FormattingHelper
    {
        /// <summary>
        ///     Отформатировать сообщение
        /// </summary>
        internal static string FormatMessage(FormattableString str)
        {
#if USE_CUSTOM_STRING_FORMATTER
            return CustomStringFormat(str.Format, str.GetArguments());
#else
            return str.ToString(LogFormatProvider.Instance);
#endif
        }

        /// <summary>
        ///     Отформатировать сообщение
        /// </summary>
        [StringFormatMethod("format")]
        internal static string FormatMessage(string format, params object[] args)
        {
            if (args.Length == 0)
            {
                return format;
            }

#if USE_CUSTOM_STRING_FORMATTER
            return CustomStringFormat(format, args);
#else
            return string.Format(LogFormatProvider.Instance, format, args);
#endif
        }

        /// <summary>
        ///     Отформатировать сообщение
        /// </summary>
        internal static void FormatMessageInPlace(StringBuilder builder, FormattableString str)
        {
#if USE_CUSTOM_STRING_FORMATTER
            CustomStringFormat(builder, str.Format, str.GetArguments());
#else
            builder.AppendFormat(LogFormatProvider.Instance, str.Format, str.GetArguments());
#endif

        }

        /// <summary>
        ///     Отформатировать сообщение
        /// </summary>
        [StringFormatMethod("format")]
        internal static void FormatMessageInPlace(StringBuilder builder, string format, params object[] args)
        {
            if (args.Length == 0)
            {
                builder.Append(format);
                return;
            }

#if USE_CUSTOM_STRING_FORMATTER
            CustomStringFormat(builder, format, args);
#else
            builder.AppendFormat(LogFormatProvider.Instance, format, args);
#endif
        }

        internal static void ReleaseUnused(IPrintable printable)
        {
            var reusable = printable as IReusable;
            reusable?.Release();
        }

        internal static void ReleaseUnused(FormattableString str)
        {
            for (var i = 0; i < str.ArgumentCount; i++)
            {
                var reusable = str.GetArgument(i) as IReusable;
                reusable?.Release();
            }
        }

        internal static void ReleaseUnused(ILogField[] fields)
        {
            for (var i = 0; i < fields.Length; i++)
            {
                var reusable = fields[i] as IReusable;
                reusable?.Release();
            }
        }

        internal static void ReleaseUnused(object[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                var reusable = args[i] as IReusable;
                reusable?.Release();
            }
        }

        internal static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
        private const string NullStr = "null";
        private const string EmptyStr = "''";

        internal static string EscapeString(string str, bool quoteIfNecessary = true)
        {
            if (str == null)
            {
                return NullStr;
            }

            if (str.Length == 0)
            {
                return EmptyStr;
            }

            if (str == "null")
            {
                return "'null'";
            }

            if (str.Length > 1)
            {
                var first = str[0];
                var last = str[str.Length - 1];

                if (IsMatchingPairOfBrackets(first, last))
                {
                    return str;
                }
            }

            if (!quoteIfNecessary)
            {
                return str;
            }

            var shouldEscape = false;
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                if (!char.IsLetterOrDigit(c) && !char.IsPunctuation(c))
                {
                    shouldEscape = true;
                    break;
                }
            }

            if (!shouldEscape)
            {
                return str;
            }

            var sb = LocalStringBuilderCache.New(str.Length + 2);
            sb.Append("'");
            for (var i = 0; i < str.Length; i++)
            {
                var c = str[i];
                switch (c)
                {
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    case '\'':
                        sb.Append("\\'");
                        break;

                    default:
                        sb.Append(c);
                        break;
                }
            }
            sb.Append("'");

            return sb.ToString();
        }

        internal static string FormatValue(object arg) => FormatValue("", arg, FormatProvider);
        internal static string FormatValue(object arg, string format) => FormatValue(format, arg, FormatProvider);

        internal static string FormatValue(string format, object arg, IFormatProvider formatProvider)
        {
            // Значения null форматируются как "null"
            if (ReferenceEquals(arg, null))
            {
                return NullStr;
            }

            var printable = arg as IPrintable;
            if (!ReferenceEquals(printable, null))
            {
                var result = Format(printable);
                var reusable = printable as IReusable;
                reusable?.Release();
                return result;
            }

            var formattedValue = TryPrintKnownType(arg, format);
            if (formattedValue != null)
            {
                return formattedValue;
            }

            // enum-ы выводятся в лог единообразно
            if (arg is Enum)
            {
                return EnumPrintFormatter.Print(arg.GetType(), arg);
            }

            // IFormattable-объекты выводятся в лог как есть, с форматированием
            var formattable = arg as IFormattable;
            if (formattable != null)
            {
                return formattable.ToString(format, formatProvider);
            }

            // Все прочее выводится как есть
            return arg.ToString();
        }

#if USE_CUSTOM_STRING_FORMATTER

        // Кастомная реализация string.Format()
        // Отличия от системной:
        //  * значения полей форматируются через FormattingHelper
        //  * поля, вокруг который имеются кавычки, не оборачиваются в кавычки
        //  * символ ', если только он не находится рядом с полем форматирования, заменяется на `

        private static readonly Regex _FormatFieldRegex = new Regex("^([0-9+])(|\\:.*)$", RegexOptions.Compiled);

        private static string CustomStringFormat(string format, object[] args)
        {
            var builder = LocalStringBuilderCache.New(format.Length + (args?.Length ?? 0) * 5);
            CustomStringFormat(builder, format, args);
            return LocalStringBuilderCache.GetStringAndRelease(builder);
        }

        private static void CustomStringFormat(StringBuilder builder, string format, object[] args)
        {
            for (var i = 0; i < format.Length; i++)
            {
                if (format[i] == '{')
                {
                    if (i < format.Length - 1 && format[i + 1] == '{')
                    {
                        builder.Append('{');
                        i++;
                        continue;
                    }

                    FormatField(builder, format, ref i, args);
                    continue;
                }

                if (format[i] == '}')
                {
                    if (i < format.Length - 1 && format[i + 1] == '}')
                    {
                        builder.Append('}');
                        i++;
                        continue;
                    }
                }

                if (format[i] == '\'')
                {
                    var isNearFormatField =
                        i > 0 && format[i - 1] == '}' ||
                       i < format.Length - 1 && format[i + 1] == '{';
                    if (!isNearFormatField)
                    {
                        builder.Append('`');
                        continue;
                    }
                }

                builder.Append(format[i]);
            }
        }

        private static void FormatField(StringBuilder sb, string format, ref int i, object[] args)
        {
            // Read field
            i++;
            var indexStart = i;

            while (i < format.Length && format[i] != '}')
            {
                i++;
            }
            var indexEnd = i;

            var match = _FormatFieldRegex.Match(format, indexStart, indexEnd - indexStart);
            if (!match.Success)
            {
                PrintMalformedField(sb, format, indexStart, indexEnd);
                return;
            }

            var index = int.Parse(match.Groups[1].Value);
            if (index < 0 | index >= args.Length)
            {
                PrintMalformedField(sb, format, indexStart, indexEnd);
                return;
            }

            var argument = match.Groups[2].Value;
            if (argument.Length > 0)
            {
                argument = argument.Substring(1);
            }

            char preChar = '\0', postChar = '\0';
            if (indexStart - 2 >= 0)
            {
                preChar = format[indexStart - 2];
            }

            if (indexEnd + 1 < format.Length)
            {
                postChar = format[indexEnd + 1];
            }

            var value = args[index];
            if (IsMatchingPairOfBrackets(preChar, postChar) && value is string)
            {
                value = EscapeString((string)value, false);
                sb.Append(value);
            }
            else
            {
                var formatted = FormatValue(value, argument);
                sb.Append(formatted);
            }
        }

        private static void PrintMalformedField(StringBuilder builder, string format, int start, int end)
        {
            builder.Append('{');
            for (var j = start; j < end; j++)
            {
                builder.Append(format[j]);
            }
            builder.Append('}');
        }

#endif

        private static bool IsMatchingPairOfBrackets(char first, char last)
        {
            return
                first == '(' && last == ')' ||
                first == '[' && last == ']' ||
                first == '{' && last == '}' ||
                first == '<' && last == '>' ||
                first == '\'' && last == '\'' ||
                first == '\"' && last == '\"' ||
                first == '`' && last == '`';
        }
    }
}

