using System.Collections.Generic;
#if NET45
using System.Runtime.Remoting.Messaging;
#endif
using System.Threading;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Идентификатор цепочки асинхронных вызовов
    /// </summary>
    internal static class AsyncScope
    {
#if NETSTANDARD1_6 || NET46
        private static readonly AsyncLocal<AsyncScopeData> AsyncLocalScopeData = new AsyncLocal<AsyncScopeData>();
#endif

        private sealed class AsyncScopeData
        {
            public ushort Id { get; set; }
            public object LockObject { get; } = new object();
            public Stack<string> AsyncMethodNames { get; } = new Stack<string>();
        }

#if NET45
        private const string DataKey = "_AsyncScope";
#endif
        private static int _idGenerator = 0x0001;

        /// <summary>
        ///     Пустое значение
        /// </summary>
        public const ushort None = 0x0000;

        /// <summary>
        ///     Текущее значение Id
        /// </summary>
        public static ushort Id
        {
            get
            {
#if NETSTANDARD1_6 || NET46
                var data = AsyncLocalScopeData.Value;
#elif NET45
                var data = CallContext.LogicalGetData(DataKey) as AsyncScopeData;
#endif
                return data?.Id ?? None;
            }
        }

        /// <summary>
        ///     Текущее значение Level
        /// </summary>
        public static int Depth
        {
            get
            {
#if NETSTANDARD1_6 || NET46
                var data = AsyncLocalScopeData.Value;
#elif NET45

                var data = CallContext.LogicalGetData(DataKey) as AsyncScopeData;
#endif
                if (data == null)
                {
                    return 0;
                }

                lock (data.LockObject)
                {
                    return data.AsyncMethodNames.Count;
                }
            }
        }

        /// <summary>
        ///     Текущее значение MethodName
        /// </summary>
        [CanBeNull]
        public static string MethodName
        {
            get
            {
#if NETSTANDARD1_6 || NET46
                var data = AsyncLocalScopeData.Value;

#elif NET45
                var data = CallContext.LogicalGetData(DataKey) as AsyncScopeData;
#endif
                if (data == null)
                {
                    return null;
                }
                lock (data.LockObject)
                {
                    return data.AsyncMethodNames.Count > 0 ? data.AsyncMethodNames.Peek() : null;
                }
            }
        }

        private static ushort GenerateNextId()
        {
            int value = None;
            while (value == None)
            {
                value = Interlocked.Increment(ref _idGenerator);
            }

            return unchecked((ushort)value);
        }

#if NETSTANDARD1_6 || NET46
        public static void Clear() => AsyncLocalScopeData.Value.AsyncMethodNames.Clear(); 
#elif NET45
        public static void Clear() => CallContext.FreeNamedDataSlot(DataKey);
#endif

        public static void Push(string methodName)
        {
#if NETSTANDARD1_6 || NET46
            var data = AsyncLocalScopeData.Value; 
#elif NET45
            var data = CallContext.LogicalGetData(DataKey) as AsyncScopeData;
#endif
            if (data == null)
            {
                data = new AsyncScopeData
                {
                    Id = GenerateNextId()
                };
#if NETSTANDARD1_6 || NET46
                AsyncLocalScopeData.Value = data; 
#elif NET45
                CallContext.LogicalSetData(DataKey, data);
#endif
            }

            lock (data.LockObject)
            {
                data.AsyncMethodNames.Push(methodName);
            }
        }

        public static void Pop()
        {
#if NETSTANDARD1_6 || NET46
            var data = AsyncLocalScopeData.Value; 
#elif NET45
            var data = CallContext.LogicalGetData(DataKey) as AsyncScopeData;
#endif
            if (data == null)
            {
                return;
            }

            lock (data.LockObject)
            {
                if (data.AsyncMethodNames.Count > 0)
                {
                    data.AsyncMethodNames.Pop();
                }

                if (data.AsyncMethodNames.Count == 0)
                {
#if NET45
                    CallContext.FreeNamedDataSlot(DataKey); 
#endif
                }
            }
        }
    }
}

