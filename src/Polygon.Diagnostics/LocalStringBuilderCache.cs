using System;
using System.Collections.Generic;
using System.Text;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Кеш <see cref="StringBuilder"/>-ов, которые используются только внутри метода
    /// </summary>
    internal static class LocalStringBuilderCache
    {
        [ThreadStatic]
        private static Stack<StringBuilder> _cachedInstances;

        public static StringBuilder New(int capacity = 16)
        {
            if (_cachedInstances == null)
            {
                _cachedInstances = new Stack<StringBuilder>();
            }

            if (_cachedInstances.Count > 0)
            {
                var sb = _cachedInstances.Pop();
                sb.EnsureCapacity(capacity);
                return sb;
            }
            
            return new StringBuilder(capacity);
        }

        public static void Release(StringBuilder sb)
        {
            if (_cachedInstances == null)
            {
                _cachedInstances = new Stack<StringBuilder>();
            }

            sb.Clear();
            _cachedInstances.Push(sb);
        }

        public static string GetStringAndRelease(StringBuilder sb)
        {
            var str = sb.ToString();
            Release(sb);
            return str;
        }
    }
}

