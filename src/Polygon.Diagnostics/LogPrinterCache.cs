using System.Collections.Concurrent;

namespace Polygon.Diagnostics
{
    internal sealed class LogPrinterCache<T>
        where T : LogPrinterBase, new()
    {
        private const int InitialCount = 100;
        private readonly ConcurrentQueue<T> queue = new ConcurrentQueue<T>();

        public LogPrinterCache()
        {
            for (var i = 0; i < InitialCount; i++)
            {
                queue.Enqueue(new T());
            }
        }

        public T Acquire(string loggerName, string methodName, int lineNumber)
        {
            var builder = Acquire();
            builder.LoggerName = loggerName;
            builder.MethodName = methodName;
            builder.LineNumber = lineNumber;
            return builder;
        }

        private T Acquire()
        {
            T builder;
            if(!queue.TryDequeue(out builder))
            {
                builder = new T();
            }
            return builder;
        }

        public void Release(T builder)
        {
            queue.Enqueue(builder);
        }
    }
}

