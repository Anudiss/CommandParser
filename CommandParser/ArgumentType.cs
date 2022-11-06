using System;
using System.Text.RegularExpressions;

namespace CommandParser
{
    /// <summary>
    /// Класс типа агрумента
    /// </summary>
    public class ArgumentType
    {
        /// <summary>
        /// Имя типа аргумента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Регулярное выражение для парсинга агрумента
        /// </summary>
        public Regex Regex { get; set; }

        /// <summary>
        /// Предикат, проверяющий на правильность ввода аргумента
        /// </summary>
        public Predicate<string> IsValid { get; set; }

        /// <summary>
        /// Делегат парсинга аргумента
        /// </summary>
        public Func<string, object> Parse { get; set; }
    }
}
