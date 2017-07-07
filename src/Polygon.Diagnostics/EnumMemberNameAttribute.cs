using System;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Задает имя поля enum-а для вывода в лог через <see cref="ObjectLogFormatter"/>
    /// </summary>
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumMemberNameAttribute : Attribute
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public EnumMemberNameAttribute(string name)
        {
            Name = Normalize(name);
        }

        /// <summary>
        ///     Имя класса
        /// </summary>
        public string Name { get; }

        private static string Normalize(string name)
        {
            var sb = LocalStringBuilderCache.New(name.Length);
            foreach (var c in name)
            {
                if (char.IsDigit(c) || char.IsUpper(c) || c == '_')
                {
                    sb.Append(c);
                }
                else if (char.IsLower(c))
                {
                    sb.Append(char.ToUpperInvariant(c));
                }
            }
            return LocalStringBuilderCache.GetStringAndRelease(sb);
        }
    }
}

