using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandParser
{
    /// <summary>
    /// Класс структуры команды
    /// </summary>
    public class CommandEntity
    {
        private Argument[] arguments;

        /// <summary>
        /// Возможные вызовы команды
        /// </summary>
        public string[] Synonyms { get; set; }

        /// <summary>
        /// Аргументы, принимаемые командой
        /// </summary>
        public Argument[] Arguments
        {
            get => arguments;
            set
            {
                int requiredArgumentsCount = value.Count(argument => argument.IsRequired);
                if (requiredArgumentsCount != value.TakeWhile(argument => argument.IsRequired).Count())
                    throw new ArgumentsPositionException();

                var sameArguments = value.GetDuplicates(arguments => arguments.Name);
                if (sameArguments.Count() != 0)
                    throw new ArgumentNameException(sameArguments.ToArray());

                arguments = value;
            }
        }

        /// <summary>
        /// Флаги, принимаемые командой
        /// </summary>
        public Flag[] Flags { get; set; }
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
        /// Словарь, содержащий имя агрумента и его значения
        /// </summary>
        public Dictionary<string, object> Arguments { get; set; }

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
            string[] commandParts = Regex.Split(commandToParse.Trim(), @"\s+");
            string commandName = commandParts[0];

            CommandEntity entity = commands.FirstOrDefault(command => command.Synonyms.Contains(commandName));
            if (entity == default)
                throw new UnknownCommandException(commandName);

            Flag[] flags = entity.ParseFlags(commandParts.Skip(1));
            Dictionary<string, object> arguments = entity.ParseArguments(commandParts.Skip(1));

            return new Command()
            {
                Arguments = arguments,
                CommandEntity = entity,
                Flags = flags
            };
        }
        
        /// <summary>
        /// Метод парсинга аргументов из команды в строковом формате
        /// </summary>
        /// <param name="commandEntity">Структура команды</param>
        /// <param name="commadParts">Части команды, разделённые пробельныим символами, без названия команды</param>
        /// <returns>Словарь: имя аргумента - значение</returns>
        private static Dictionary<string, object> ParseArguments(this CommandEntity commandEntity, IEnumerable<string> commadParts)
        {
            string[] arguments = commadParts.Where(part => !Regex.IsMatch(part, @"-{1,2}\w*"))
                                            .ToArray();

            Dictionary<string, object> argumentValues = new Dictionary<string, object>();
            for (int i = 0; i < arguments.Length; i++)
            {
                Argument argument = commandEntity.Arguments[i];
                argumentValues.Add(argument.Name, argument.Parse(arguments[i]));
            }
            return argumentValues;
        }

        /// <summary>
        /// Метод парсинга флагов из команды в строковом формате
        /// </summary>
        /// <param name="commandEntity">Структура команды</param>
        /// <param name="commandPart">Части команды, разделённые пробельныим символами, без названия команды</param>
        /// <returns>Массив флагов</returns>
        private static Flag[] ParseFlags(this CommandEntity commandEntity, IEnumerable<string> commandPart)
        {
            return (from part in commandPart
                    from flag in commandEntity.Flags
                    where flag.Regex.IsMatch(part)
                    select flag).ToArray();
        }
    }

    /// <summary>
    /// Исключение неизвестной команды
    /// </summary>
    public class UnknownCommandException : Exception
    {
        public override string Message { get; }

        public UnknownCommandException(string command) =>
            Message = $"Неизвестная команда: {command}";
    }

    /// <summary>
    /// Исключение неверно введённых аргументов
    /// </summary>
    public class CommandArgumentException : Exception
    {
        public override string Message { get; }

        public CommandArgumentException(CommandEntity commandEntity) =>
            Message = $"Невеное использование команды\nОжидалось: {commandEntity.Synonyms[0]} {string.Join(" ", commandEntity.Arguments.Select(argument => $"{argument.Name}"))}";
    }

    /// <summary>
    /// Исключение, неверного расположения аргументов: необязательные аргументы, должны находиться в конце
    /// </summary>
    public class ArgumentsPositionException : Exception
    {
        public override string Message => "Необязательные аргументы, должны находиться в конце";
    }

    /// <summary>
    /// Исключение, неверного типа аргумента
    /// </summary>
    public class ArgumentTypeException : Exception
    {
        public override string Message { get; }

        public ArgumentTypeException(Argument argument) =>
            Message = $"Неверный тип аргумента: {argument.Name}, ожидалось: {argument.ArgumentType.Name}";
    }

    /// <summary>
    /// Исключение, одинаковых имён аргументов в одной команде
    /// </summary>
    public class ArgumentNameException : Exception
    {
        public override string Message { get; }

        public ArgumentNameException(params Argument[] arguments) =>
            Message = $"Аргументы с именами {string.Join(", ", arguments.Select(argument => argument.Name))} уже есть в этой команде";
    }
}
