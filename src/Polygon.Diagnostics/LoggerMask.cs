using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Polygon.Diagnostics
{
    /// <summary>
    ///     Маска имени логгера
    /// </summary>
    internal sealed class LoggerMask
    {
        private readonly string mask;
        private readonly HashSet<string> knownLoggerNames = new HashSet<string>();

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="mask">
        ///     Маска (описание формата см. <see cref="LogManager.ForbidLogging"/>)
        /// </param>
        public LoggerMask(string mask)
        {
            // Замена "." на "\."
            mask = mask.Replace(".", "\\.");

            // Замена "**" на "[a-zA-Z0-9_\.]+"
            mask = mask.Replace("**", "[a-zA-Z0-9_\\.]+");

            // Замена "*" на "[a-zA-Z0-9_]+"
            mask = mask.Replace("*", "[a-zA-Z0-9_]+");

            this.mask = mask;
        }

        /// <summary>
        ///     Попадает имя логгера под маску. Если падает, то оно будет запомнено.
        /// </summary>
        /// <param name="loggerName">
        ///     Имя логгера
        /// </param>
        /// <returns>
        ///     true, если имя логгера попадает под маску, false - в противном случае.
        /// </returns>
        public bool IsMatch(string loggerName)
        {
            var isMatch = Regex.IsMatch(loggerName, mask);
            if (isMatch)
            {
                // Имя попало под маску
                knownLoggerNames.Add(loggerName);
                return true;
            }

            // Имя не попало в маску
            return false;
        }

        /// <summary>
        ///     Получить список всех имен логгеров, когда-либо попавших под эту маску
        /// </summary>
        public IEnumerable<string> GetAffectedLoggerNames()
        {
            return knownLoggerNames;
        }
    }
}

