using System;
using System.Text.RegularExpressions;

namespace CommandParser.Command
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
        public Regex ArgumentParsingRegex { get; set; }

        /// <summary>
        /// Предикат, проверяющий на правильность ввода аргумента
        /// </summary>
        public bool IsValid(string argument) => ArgumentParsingRegex.IsMatch(argument.Trim()) && Validator?.Invoke(argument.Trim()) == true;

        /// <summary>
        /// Метод парсинга аргумента
        /// </summary>
        public Func<string, object> Parse { get; set; }

        /// <summary>
        /// Предикат валидации агрумента
        /// </summary>
        public Predicate<object> Validator { get; set; }
    }

    public class ArrayArgument
    {
        public object[] Array { get; set; }
    }
}
