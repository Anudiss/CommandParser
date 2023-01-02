using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public Regex ArgumentParsingRegex { get; set; } = null;

        /// <summary>
        /// Предикат, проверяющий на правильность ввода аргумента
        /// </summary>
        public bool IsValid(string argument) => ArgumentParsingRegex?.IsMatch(argument.Trim()) != false && Validator?.Invoke(argument.Trim()) != false;

        /// <summary>
        /// Метод парсинга аргумента
        /// </summary>
        public virtual Func<string, object> Parse { get; set; }

        /// <summary>
        /// Предикат валидации агрумента
        /// </summary>
        public Predicate<object> Validator { get; set; }
    }

    public class TupleType : ArgumentType
    {
        public Argument[] TypesInside { get; set; }

        public override Func<string, object> Parse => (argument) =>
        {
            Dictionary<string, object> tupleElements = new Dictionary<string, object>();

            Match match = CommandParser.GeneralPattern.Match(argument.Substring(1, argument.Length - 2));
            if (!match.Success)
                throw new ArgumentTypeException(new Argument()
                {
                    ArgumentType = this,
                    Name = Name
                });

            IEnumerator<Capture> textEnumerator = match.Groups["Token"].Captures.Cast<Capture>().GetEnumerator();
            TypesInside.Where(arg => textEnumerator.MoveNext())
                       .Select(arg => new KeyValuePair<string, object>(arg.Name, arg.Parse(Regex.Replace(textEnumerator.Current.Value, @",$", ""))))
                       .ForEach(arg => tupleElements.Add(arg.Key, arg.Value));

            return tupleElements;
        };
    }

    public class Array : ArgumentType
    {
        public ArgumentType TypeInside { get; }
        public override Func<string, object> Parse => (argument) =>
        {
            ArrayList array = new ArrayList();

            Match match = CommandParser.GeneralPattern.Match(argument.Substring(1, argument.Length - 2));
            if (!match.Success)
                throw new ArgumentTypeException(new Argument()
                {
                    ArgumentType = this,
                    Name = Name
                });

            IEnumerable<Capture> elements = match.Groups["Token"].Captures.Cast<Capture>();
            foreach (var element in elements)
                array.Add(TypeInside.Parse(Regex.Replace(element.Value, @",$", "")));

            return array.ToArray();
        };

        public Array(ArgumentType typeInside)
        {
            TypeInside = typeInside;
        }
    }
}
