using System;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Задает имя класса для вывода в лог через <see cref="ObjectLogFormatter"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    [PublicAPI]
    public sealed class ObjectNameAttribute : Attribute
    {
        /// <summary>
        ///     Конструктор
        /// </summary>
        public ObjectNameAttribute(string name)
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

