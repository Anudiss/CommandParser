using System;
using System.Text.RegularExpressions;

namespace CommandParser
{
    /// <summary>
    /// Класс аргумента
    /// </summary>
    public class Argument
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
        /// Является ли аргумент обязательным
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Регулярное выражение для парсинга аргумента
        /// </summary>
        public Regex Regex => ArgumentType.Regex;

        /// <summary>
        /// Метод преобразования строкового аргумента в нужный тип
        /// </summary>
        /// <param name="argument">Строковый аргумент</param>
        /// <returns>Преобразованное значение аргумента</returns>
        public object Parse(string argument) => ArgumentType.IsValid(argument) ? ArgumentType.Parse(argument) : throw new ArgumentTypeException(this);
    }
}
