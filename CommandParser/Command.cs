using System;
using System.Collections.Generic;
using System.Linq;

namespace CommandParser
{
    /// <summary>
    /// Класс структуры команды
    /// </summary>
    public class CommandEntity
    {
        /// <summary>
        /// Возможные вызовы команды
        /// </summary>
        public string[] Synonyms { get; set; }

        /// <summary>
        /// Аргументы, принимаемые командой
        /// </summary>
        public Argument[] Arguments { get; set; }
    }

    /// <summary>
    /// Класс команды
    /// </summary>
    public class Command
    {
        /// <summary>
        /// Структура команды
        /// </summary>
        public CommandEntity CommandEntity { get; set; }

        /// <summary>
        /// Словарь, содержащий тип агрумента и его значения
        /// </summary>
        public Dictionary<Argument, object> Arguments { get; set; }

        /// <summary>
        /// Флаги, содержащиеся в команде
        /// </summary>
        public Flag[] Flags { get; set; }
    }

    /// <summary>
    /// Класс парсинга команд
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        /// Метод парсинга команды
        /// </summary>
        /// <param name="commands">Список возможных команд</param>
        /// <param name="commandToParse">Строка, содержащая команду, которую нужно спарсить</param>
        /// <returns>Сущность команды с аргументами, их значениями и флагами</returns>
        public static Command ParseCommand(this IEnumerable<CommandEntity> commands, string commandToParse)
        {
            throw new NotImplementedException();
        }
    }

    public class UnknownCommandException : Exception
    {
        public override string Message { get; }

        public UnknownCommandException(string command) =>
            Message = $"Неизвестная команда: {command}";
    }

    public class InvalidCommandArgumentException : Exception
    {
        public override string Message { get; }

        public InvalidCommandArgumentException(CommandEntity commandEntity) =>
            Message = $"Невеное использование команды\nОжидалось: {commandEntity.Synonyms[0]} {string.Join(" ", commandEntity.Arguments.Select(argument => $"{argument.Name}"))}";
    }
}
