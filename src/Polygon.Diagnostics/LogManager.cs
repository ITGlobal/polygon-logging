using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using JetBrains.Annotations;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Фасад менеджера логгеров
    /// </summary>
    [PublicAPI]
    public static class LogManager
    {
        #region nested types

        /// <summary>
        ///     Действие для имени логгера
        /// </summary>
        private enum LoggerNameAction
        {
            /// <summary>
            ///     Разрешить логгер
            /// </summary>
            Allow,

            /// <summary>
            ///     Запретить логгер
            /// </summary>
            Forbid
        }

        #endregion

        #region fields

        private static readonly ReaderWriterLockSlim _LoggersLock = new ReaderWriterLockSlim();
        private static readonly Dictionary<string, ILog> _Loggers = new Dictionary<string, ILog>();

        /// <summary>
        ///     Набор приемников сообщений лога
        /// </summary>
        private static ImmutableListenerSet _listeners = ImmutableListenerSet.Empty;

        /// <summary>
        ///     Разрешено ли логирование.
        ///     Целое число, нуль означает запрет логирования, 
        ///     любое ненулевое значение означает, что логирование разрешено.
        /// </summary>
        private static int _isEnabled = 1;

        /// <summary>
        ///     Порог логирования. 
        ///     Целое число, соответствующее численному представлению значений перечисления <see cref="LogLevel"/>.
        /// </summary>
        private static int _threshold;

        private static readonly ReaderWriterLockSlim _ForbiddenLoggerMasksLock = new ReaderWriterLockSlim();

        /// <summary>
        ///     Активные маски запрещенных логгеров
        /// </summary>
        private static readonly Dictionary<string, LoggerMask> _ForbiddenLoggerMasks = new Dictionary<string, LoggerMask>();

        /// <summary>
        ///     Имена логгеров, для которых уже определено действие
        /// </summary>
        private static readonly Dictionary<string, LoggerNameAction> _KnownLoggerNames = new Dictionary<string, LoggerNameAction>();

        #endregion

        #region Logging configuration

        /// <summary>
        ///     Разрешено ли логирование
        /// </summary>
        public static bool IsEnabled
        {
            get { return _isEnabled != 0; }
            set { Interlocked.Exchange(ref _isEnabled, value ? 1 : 0); }
        }

        /// <summary>
        ///     Порог логирования
        /// </summary>
        public static LogLevel Threshold
        {
            get { return (LogLevel)_threshold; }
            set { Interlocked.Exchange(ref _threshold, (int)value); }
        }

        /// <summary>
        ///     Добавить приемник сообщений лога
        /// </summary>
        /// <param name="listener">
        ///     Приемник сообщений лога
        /// </param>
        public static void AddListener([NotNull] ILogListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener));

            Interlocked.Exchange(ref _listeners, _listeners.Add(listener));
        }

        /// <summary>
        ///     Запретить логирование всем логгерам, попадающим под маску <paramref name="mask"/>
        /// </summary>
        /// <param name="mask">
        ///     Маска имени логгера. 
        ///     Формат маски:
        ///     Namespace1.Namespace2.*.Namespace3.**
        ///     Здесь * означает любой сегмент в имени, ** - любой набор любых сегментов.
        ///     Пример: под указанную маску попадут имена вида:
        ///         Namespace1.Namespace2.xxx.Namespace3.yyy
        ///         Namespace1.Namespace2.xxx.Namespace3.yyy.zzz
        ///     Но не попадут имена вида:
        ///         Namespace1.Namespace2.Namespace3.yyy
        ///         Namespace1.Namespace2.xxx.Namespace3
        ///         Namespace1.Namespace2.Namespace3
        /// </param>
        public static void ForbidLogging([NotNull] string mask)
        {
            if (string.IsNullOrEmpty(mask))
                throw new ArgumentNullException(nameof(mask));

            _ForbiddenLoggerMasksLock.EnterWriteLock();
            try
            {
                LoggerMask loggerMask;
                if (_ForbiddenLoggerMasks.TryGetValue(mask, out loggerMask))
                {
                    // Маска уже зарегистрирована
                    return;
                }

                // Создаем маску и добавляем ее в список запрещающих масок
                loggerMask = new LoggerMask(mask);
                _ForbiddenLoggerMasks[mask] = loggerMask;

                // Очищаем список действий для логгеров
                // NOTE тут можно не очищать, а проходить по списку свежесозданной маской и записывать нужные значения
                // Но это дольше, поэтому оставим пока так
                _KnownLoggerNames.Clear();
            }
            finally
            {
                _ForbiddenLoggerMasksLock.ExitWriteLock();
            }
        }

        /// <summary>
        ///     Отменить запрет на логирование всем логгерам, попадающим под маску <paramref name="mask"/>
        /// </summary>
        /// <param name="mask">
        ///     Маска имени логгера. Описание формата маски см. метод <see cref="ForbidLogging"/>
        /// </param>
        public static void AllowLogging([NotNull] string mask)
        {
            if (string.IsNullOrEmpty(mask))
                throw new ArgumentNullException(nameof(mask));

            _ForbiddenLoggerMasksLock.EnterWriteLock();
            try
            {
                // Выбираем ранее добавленную маску
                LoggerMask loggerMask;
                if (!_ForbiddenLoggerMasks.TryGetValue(mask, out loggerMask))
                {
                    return;
                }

                // Удаляем из списка запрещенных логгеров все логгеры, которые ранее попали под данную маску
                foreach (var loggerName in loggerMask.GetAffectedLoggerNames())
                {
                    _KnownLoggerNames.Remove(loggerName);
                }

                // Удаляем маску
                _ForbiddenLoggerMasks.Remove(mask);
            }
            finally
            {
                _ForbiddenLoggerMasksLock.ExitWriteLock();
            }
        }

        #endregion

        #region Logger API

        /// <summary>
        ///     Создать логгер
        /// </summary>
        /// <typeparam name="T">
        ///     Тип для логгера
        /// </typeparam>
        /// <returns>
        ///     Логгер
        /// </returns>
        [NotNull]
        public static ILog GetLogger<T>() => GetLogger(typeof(T));

        /// <summary>
        ///     Создать логгер
        /// </summary>
        /// <param name="type">
        ///     Тип для логгера
        /// </param>
        /// <returns>
        ///     Логгер
        /// </returns>
        [NotNull]
        public static ILog GetLogger([NotNull] Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return GetLogger(type.FullName);
        }

        /// <summary>
        ///     Создать логгер
        /// </summary>
        /// <param name="name">
        ///     Название логгера
        /// </param>
        /// <returns>
        ///     Логгер
        /// </returns>
        [NotNull]
        public static ILog GetLogger([NotNull] string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            ILog log;

            _LoggersLock.EnterUpgradeableReadLock();
            try
            {
                if (_Loggers.TryGetValue(name, out log))
                {
                    return log;
                }

                // Логгер не существует, создаем его
                _LoggersLock.EnterWriteLock();
                try
                {
                    log = new LoggerImplementation(name);
                    _Loggers.Add(name, log);
                }
                finally
                {
                    _LoggersLock.ExitWriteLock();
                }
            }
            finally
            {
                _LoggersLock.ExitUpgradeableReadLock();
            }

            // Сразу вычисляем для логгера действие, если он был только что создан
            GetActionForLogger(name);

            return log;
        }

        /// <summary>
        ///     Обернуть асинхронный метод, чтобы его название правильно логировалось
        /// </summary>
        public static AsyncMethodToken Scope([CallerMemberName] string caller = null)
        {
            AsyncScope.Push(caller);
            return new AsyncMethodToken();
        }

        /// <summary>
        ///     Прервать цепочку асинхронных вызовов
        /// </summary>
        public static void BreakScope() => AsyncScope.Clear();

        #endregion

        #region Internal methods

        /// <summary>
        ///     Разрешено ли логирование для логгера <paramref name="logger"/> по уровню <paramref name="level"/>
        /// </summary>
        /// <param name="logger">
        ///     Имя логгера
        /// </param>
        /// <param name="level">
        ///     Уровень логирования
        /// </param>
        /// <returns>
        ///     Разрешено ли логирование
        /// </returns>
        internal static bool IsLoggingEnabled(string logger, LogLevel level)
        {
            // Проверяем, разрешено ли логирование вообще
            if (!IsEnabled)
            {
                return false;
            }

            // Проверяем, разрешено ли логирование с данным уровнем
            if (Threshold > level)
            {
                return false;
            }

            // Проверяем, разрешено ли логирование данному логгеру
            var action = GetActionForLogger(logger);
            switch (action)
            {
                case LoggerNameAction.Allow:
                    // Логгер уже попал в список разрешенных
                    return true;

                case LoggerNameAction.Forbid:
                    // Логгер уже попал в список запрещенных
                    return false;

                default:
                    throw new InvalidOperationException($"\"{action}\" is not a valid value for {nameof(LoggerNameAction)}");
            }
        }

        /// <summary>
        ///     Записать сообщение в лог
        /// </summary>
        /// <param name="logger">
        ///     Название логгера
        /// </param>
        /// <param name="level">
        ///     Уровень логирования
        /// </param>
        /// <param name="message">
        ///     Сообщение
        /// </param>
        /// <param name="exception">
        ///     Исключение
        /// </param>
        /// <param name="callerMethodName">
        ///     Название вызывающего метода
        /// </param>
        /// <param name="lineNumber">
        ///     Номер строки
        /// </param>
        internal static void Write(
            string logger,
            LogLevel level,
            string message,
            [CanBeNull] Exception exception,
            string callerMethodName,
            int lineNumber)
        {
            var e = new LogEvent
            {
                Time = DateTime.Now,
                ThreadId = Thread.CurrentThread.ManagedThreadId,
                ThreadName = Thread.CurrentThread.Name,
                LoggerName = logger,
                MethodName = GetMethodName(callerMethodName),
                LineNumber = lineNumber,
                Level = level,
                ScopeId = AsyncScope.Id,
                Depth = AsyncScope.Depth,
                Message = message,
                Exception = exception
            };

            // Передаем событие приемникам
            try
            {
                _listeners.Write(ref e);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }

        private static string GetMethodName(string callerMethodName)
        {
            var topAsyncMethodName = AsyncScope.MethodName;
            if (string.IsNullOrEmpty(topAsyncMethodName))
            {
                return callerMethodName;
            }

            if (callerMethodName.Contains(topAsyncMethodName))
            {
                return topAsyncMethodName;
            }

            return callerMethodName;
        }

        /// <summary>
        ///     Опеределить действие для логгера
        /// </summary>
        /// <param name="logger">
        ///     Имя логгера
        /// </param>
        /// <returns>
        ///     Действие
        /// </returns>
        private static LoggerNameAction GetActionForLogger(string logger)
        {
            LoggerNameAction action;

            _ForbiddenLoggerMasksLock.EnterReadLock();
            try
            {
                if (_KnownLoggerNames.TryGetValue(logger, out action))
                {
                    // Логгер уже попал в список
                    return action;
                }
            }
            finally
            {
                _ForbiddenLoggerMasksLock.ExitReadLock();
            }

            // Логгер еще не попал в ни в один из списков
            // Вычисляем его место в мире
            _ForbiddenLoggerMasksLock.EnterWriteLock();
            try
            {
                // Ищем первую маску, которая запретила бы этот логгер
                // Если такой маски нет, то логгер разрешен
                action = LoggerNameAction.Allow;
                foreach (var mask in _ForbiddenLoggerMasks.Values)
                {
                    if (mask.IsMatch(logger))
                    {
                        action = LoggerNameAction.Forbid;
                        break;
                    }
                }

                // NOTE тут в принципе есть гонка данных, но она не критична
                // Есть шанс, что между текущей WriteLock и предыдущей ReadLock вклинится другой поток 
                // и успеет записать для данного логгера action
                // Эта гонка данных не критична, мы учитываем, что запись в knownLoggerNames может уже существовать
                // Гонку можно убрать, если использовать UpgradableReadLock вокруг всего, но от этого хочется уйти
                _KnownLoggerNames[logger] = action;
                return action;
            }
            finally
            {
                _ForbiddenLoggerMasksLock.ExitWriteLock();
            }
        }

        #endregion
    }
}

