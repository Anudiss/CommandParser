using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandParser.Command
{
    #region Делегат исполения команды
    /// <summary>
    /// Делегат выполнения команды
    /// </summary>
    /// <param name="arguments">Словарь: название аргумента - его значение</param>
    /// <param name="flags">Флаги, введёные пользователем</param>
    public delegate void CommandExecutHandler(Dictionary<string, object> arguments, Flag[] flags);
    #endregion
    #region Сущность команды
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

        /// <summary>
        /// Метод выполнения команды
        /// </summary>
        public CommandExecutHandler CommandExecutor { get; set; }
    }
    #endregion
    #region Команда
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
        private Dictionary<string, object> Arguments { get; set; }

        /// <summary>
        /// Флаги, содержащиеся в команде
        /// </summary>
        public Flag[] Flags { get; set; }

        /// <summary>
        /// Метод, вызывающий CommandExecutor
        /// </summary>
        public void Execute() =>
            CommandEntity.CommandExecutor?.Invoke(Arguments, Flags);

        /// <summary>
        /// Метод проверки на наличие флага в команде
        /// </summary>
        /// <param name="flag">Проверяемый флаг</param>
        /// <returns>true, если флаг есть в команде, иначе false</returns>
        public bool HasFlag(Flag flag) => Flags.Contains(flag);

        /// <summary>
        /// Метод, который возвращает значение аргумента по имени аргумента
        /// </summary>
        /// <param name="argumentName">Имя аргумента</param>
        /// <returns>Значение аргумента</returns>
        /// <exception cref="ArgumentOutOfRangeException">Такого аргумента нет в команде</exception>
        public object GetArgumentValue(string argumentName)
        {
            if (Arguments.TryGetValue(argumentName, out object argument) == false)
                throw new ArgumentOutOfRangeException("Такого аргумента нет в команде");
            return argument;
        }

        /// <summary>
        /// Индексатор, который возвращает значение аргумента по имени агрумента
        /// </summary>
        /// <param name="argumentName">Имя аргумента</param>
        /// <returns>Значение аргумента</returns>
        public object this[string argumentName] => GetArgumentValue(argumentName);

        /// <summary>
        /// Индексатор, который возвращает наличие флага в команде
        /// </summary>
        /// <param name="flag">Флаг для проверки</param>
        /// <returns>Наличие флага</returns>
        public bool this[Flag flag]
        {
            get
            {
                return HasFlag(flag);
            }
        }

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="commandEntity">Сущность команды</param>
        /// <param name="arguments">Введённые агрументы</param>
        /// <param name="flags">Флаги, используемые в команде</param>
        public Command(CommandEntity commandEntity, Dictionary<string, object> arguments, Flag[] flags)
        {
            CommandEntity = commandEntity;
            Arguments = arguments;
            Flags = flags;
        }
    }
    #endregion
    #region Парсер команды
    /// <summary>
    /// Класс парсинга команд
    /// </summary>
    public static class CommandParser
    {
        // TODO: Add text with spacing parsing
        /// <summary>
        /// Метод парсинга команды
        /// </summary>
        /// <param name="commands">Список возможных команд</param>
        /// <param name="commandToParse">Строка, содержащая команду, которую нужно спарсить</param>
        /// <returns>Сущность команды с аргументами, их значениями и флагами</returns>
        public static Command ParseCommand(this IEnumerable<CommandEntity> commands, string commandToParse)
        {
            string[] commandParts = Regex.Split(commandToParse.Trim().ToLower(), @"\s+");
            string commandName = commandParts[0];

            CommandEntity entity = commands.FirstOrDefault(command => command.Synonyms.Contains(commandName));
            if (entity == default)
                throw new UnknownCommandException(commandName);

            Flag[] flags = entity.ParseFlags(commandParts.Skip(1));
            Dictionary<string, object> arguments = entity.ParseArguments(commandParts.Skip(1));

            return new Command(entity, arguments, flags);
        }
        
        /// <summary>
        /// Метод парсинга аргументов из команды в строковом формате
        /// </summary>
        /// <param name="commandEntity">Структура команды</param>
        /// <param name="commadParts">Части команды, разделённые пробельныим символами, без названия команды</param>
        /// <returns>Словарь: имя аргумента - значение</returns>
        private static Dictionary<string, object> ParseArguments(this CommandEntity commandEntity, IEnumerable<string> commadParts)
        {
            if (commandEntity.Arguments == null)
                return null;

            string[] arguments = commadParts.Where(part => !Regex.IsMatch(part, @"-{1,2}\w*"))
                                            .ToArray();
            if (arguments.Length < commandEntity.Arguments.Where(argument => argument.IsRequired).Count())
                throw new CommandArgumentException(commandEntity);

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
            if (commandEntity.Flags == null)
                return null;

            return (from part in commandPart
                    from flag in commandEntity.Flags
                    where flag.Regex.IsMatch(part)
                    select flag).Distinct().ToArray();
        }
    }
    #endregion
    #region Исключения
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
            Message = $"Невеное использование команды\nОжидалось: {commandEntity.Synonyms[0]} {string.Join(" ", commandEntity.Arguments.Select(argument => $"{argument.Name}:{argument.ArgumentType.Name}"))}";
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
    #endregion
}
