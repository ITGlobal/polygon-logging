using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Отвечает за вывод enum-ов и flag-ов в строку с учетом атрибутов <see cref="EnumMemberNameAttribute"/> и <see cref="EnumMemberAttribute"/>
    /// </summary>
    [PublicAPI]
    public static class EnumPrintFormatter
    {
        /// <summary>
        ///     Вывести значение в строку
        /// </summary>
        public static string Print<T>(T value)
            where T : struct
        {
            var printer = _Printers.GetOrAdd(typeof(T), CreateEnumPrinter);
            var intValue = Convert.ToInt32(value);
            return printer.Print(typeof(T), intValue);
        }

        /// <summary>
        ///     Вывести значение в строку
        /// </summary>
        public static string Print(Type type, object value)
        {
            var printer = _Printers.GetOrAdd(type, CreateEnumPrinter);
            var intValue = Convert.ToInt32(value);
            return printer.Print(type, intValue);
        }

        private interface IEnumPrinter
        {
            string Print(Type type, int value);
        }

        private sealed class ValueEnumPrinter : IEnumPrinter
        {
            private readonly Dictionary<int, string> lookup;

            public ValueEnumPrinter(Dictionary<int, string> lookup)
            {
                this.lookup = lookup;
            }

            public string Print(Type type, int value)
            {
                string name;
                if (!lookup.TryGetValue(value, out name))
                {
                    name = Enum.Format(type, value, "D");
                }

                return name;
            }
        }

        private sealed class FlagsEnumPrinter : IEnumPrinter
        {
            private readonly Dictionary<int, string> lookup;

            public FlagsEnumPrinter(Dictionary<int, string> lookup)
            {
                this.lookup = lookup;
            }

            public string Print(Type type, int value)
            {
                string name;
                if (lookup.TryGetValue(value, out name))
                {
                    return name;
                }

                return string.Join("+", GetMatchingFlags(value));
            }

            private IEnumerable<string> GetMatchingFlags(int value)
            {
                foreach (var pair in lookup)
                {
                    if (pair.Key != 0 && (pair.Key & value) == pair.Key)
                    {
                        value = value & ~pair.Key;
                        yield return pair.Value;
                    }
                }
            }
        }

        private static readonly ConcurrentDictionary<Type, IEnumPrinter> _Printers
            = new ConcurrentDictionary<Type, IEnumPrinter>();

        private static IEnumPrinter CreateEnumPrinter(Type enumType)
        {
            var lookup = new Dictionary<int, string>();
#if NET45 || NET46
            foreach (var member in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
#endif
#if NETSTANDARD1_6
            foreach (var member in enumType.GetTypeInfo().GetFields(BindingFlags.Public | BindingFlags.Static))
#endif
            {
                if (member.IsLiteral)
                {
                    var value = Convert.ToInt32(member.GetValue(null));
                    var name = GetEnumMemberName(member);
                    lookup[value] = name;
                }
            }
#if NET45 || NET46
            if (enumType.GetCustomAttribute<FlagsAttribute>() != null)
#endif
#if NETSTANDARD1_6
            if (enumType.GetTypeInfo().GetCustomAttribute<FlagsAttribute>() != null)
#endif
            {
                return new FlagsEnumPrinter(lookup);
            }

            return new ValueEnumPrinter(lookup);
        }

        private static string GetEnumMemberName(FieldInfo member)
        {
            var attr1 = member.GetCustomAttribute<EnumMemberNameAttribute>();
            if (attr1 != null)
            {
                return attr1.Name;
            }

            var attr2 = member.GetCustomAttribute<EnumMemberAttribute>();
            if (attr2 != null)
            {
                return attr2.Value;
            }

            var name = member.Name;
            var sb = LocalStringBuilderCache.New(name.Length + 5);

            for (var i = 0; i < name.Length; i++)
            {
                var c = name[i];
                if (i > 0 && char.IsUpper(c) && char.IsLower(name[i - 1]))
                {
                    sb.Append('_');
                }

                sb.Append(char.ToUpperInvariant(c));
            }
            return LocalStringBuilderCache.GetStringAndRelease(sb);
        }
    }
}
