using System;
using System.Text.RegularExpressions;

namespace CommandParser
{
    /// <summary>
    /// Класс аргумента
    /// </summary>
    public struct Argument
    {
        /// <summary>
        /// Имя аргумента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип аргумента
        /// </summary>
        public ArgumentType ArgumentType { get; set; }

        /// <summary>
        /// Регулярное выражение для парсинга аргумента
        /// </summary>
        public Regex Regex => ArgumentType.Regex;
    }
}
