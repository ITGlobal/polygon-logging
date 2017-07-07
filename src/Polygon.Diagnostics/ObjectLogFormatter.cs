using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    [PublicAPI]
    public sealed partial class ObjectLogFormatter : IObjectPoolItem, IPrintable
    {
        private const string NullStr = "null";

        private static readonly ObjectPool<ObjectLogFormatter> _Pool = new ObjectPool<ObjectLogFormatter>(() => new ObjectLogFormatter());
        
        private readonly StringBuilder sb = new StringBuilder();
        private bool hasAnyFields = false;

        private ObjectLogFormatter() { }

        public static ObjectLogFormatter Create<T>(T obj, PrintOption option)
            where T : IPrintable
        {
#if NET45 || NET46
            var attr = typeof(T).GetCustomAttribute<ObjectNameAttribute>();
#endif
#if NETSTANDARD1_6
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute<ObjectNameAttribute>(); 
#endif
            if (attr == null)
            {
                throw new InvalidOperationException($"Type {typeof(T).Name} has no {nameof(ObjectNameAttribute)}");
            }

            return Create(option, attr.Name);
        }

        public static ObjectLogFormatter Create(PrintOption option, string objectTypeName)
        {
            var formatter = _Pool.Acquire();
            if (option != PrintOption.Nested)
            {
                formatter.sb.Append(objectTypeName);
                formatter.sb.Append(" { ");
            }
            else
            {
                formatter.sb.Append("{ ");
            }
            return formatter;
        }

        public static string Format(FormattableString str) => FormattingHelper.FormatMessage(str);

        #region Enum

        /// <summary>
        ///     Записать поле объекта, если оно не равно значению по умолчанию
        /// </summary>
        public void AddEnumField<T>(string name, T value)
            where T : struct
        {
            if (Equals(value, default(T)))
            {
                return;
            }

            AddFieldCore(name, EnumPrintFormatter.Print(value));
        }

        /// <summary>
        ///     Записать поле объекта, даже если оно равно значению по умолчанию
        /// </summary>
        public void AddEnumFieldRequired<T>(string name, T value)
            where T : struct
        {
            AddFieldCore(name, EnumPrintFormatter.Print(value));
        }

        #endregion

        #region Enum?

        /// <summary>
        ///     Записать поле объекта, если оно не равно значению по умолчанию
        /// </summary>
        public void AddEnumField<T>(string name, T? value)
            where T : struct
        {
            if (value == null)
            {
                return;
            }

            AddFieldCore(name, EnumPrintFormatter.Print(value.Value));
        }

        /// <summary>
        ///     Записать поле объекта, даже если оно равно значению по умолчанию
        /// </summary>
        public void AddEnumFieldRequired<T>(string name, T? value)
            where T : struct
        {
            if (value == null)
            {
                AddFieldCore(name, NullStr);
                return;
            }

            AddFieldCore(name, EnumPrintFormatter.Print(value.Value));
        }

        #endregion

        public override string ToString()
        {
            if (hasAnyFields)
            {
                sb.Append(" ");
            }
            sb.Append("}");
            var str = sb.ToString();
            _Pool.Release(this);
            return str;
        }

        /// <summary>
        ///     Вывести объект в лог
        /// </summary>
        public string Print(PrintOption option) => ToString();

        void IObjectPoolItem.Recycle()
        {
            sb.Clear();
            hasAnyFields = false;
        }

        private void AddFieldCore(string name, string value, string suffix = null)
        {
            if (hasAnyFields)
            {
                sb.Append(", ");
            }

            sb.Append(name);
            sb.Append("=");
            sb.Append(value); if (suffix != null)
            {
                sb.Append(suffix);
            }
            hasAnyFields = true;
        }

        private void AddListFieldCore<T>(string name, IEnumerable<T> values, Func<T, string> printer)
        {
            if (hasAnyFields)
            {
                sb.Append(", ");
            }

            sb.Append(name);
            sb.Append("=[");
            var isFirst = true;
            foreach (var value in values)
            {
                if (!isFirst)
                {
                    sb.Append(",");
                }
                else
                {
                    isFirst = false;
                }

                sb.Append(" ");
                sb.Append(printer(value));
            }

            if (!isFirst)
            {
                sb.Append(" ");
            }

            sb.Append("]");
            hasAnyFields = true;
        }
    }
}

