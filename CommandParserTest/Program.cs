using CommandParser.Command;
using CommandParser.Hints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandParserTest
{
    public class Program
    {
        /*
         * ^[^\[\]]*(((?'Open'\[)[^\[\]]*)+((?'Array-Open'\])[^\[\]]*)+)*(?(Open)(?!))$ - array pattern
         * ([\u0022](?<Text>[^\u0022]*)[\u0022]) - string pattern
         */
        static void Main(string[] args)
        {
            Dictionary<HintPart, ConsoleColor> ColorScheme = new Dictionary<HintPart, ConsoleColor>()
            {
                { HintPart.CommandName, ConsoleColor.Blue },
                { HintPart.ArgumentType, ConsoleColor.Yellow },
                { HintPart.RequiredArgument, ConsoleColor.DarkRed },
                { HintPart.NotRequiredArgument, ConsoleColor.Magenta }
            };

            ArgumentType Int = new ArgumentType()
            {
                Name = "int",
                ArgumentParsingRegex = new Regex(@"\d+", RegexOptions.Compiled),
                Parse = (argument) => int.Parse(argument)
            };
            ArgumentType Double = new ArgumentType()
            {
                Name = "double",
                ArgumentParsingRegex = new Regex(@"\d+(\,\.\d+)?", RegexOptions.Compiled),
                Parse = (argument) => double.Parse(argument.Replace('.', ','))
            };
            ArgumentType String = new ArgumentType()
            {
                Name = "string",
                ArgumentParsingRegex = new Regex(@".*", RegexOptions.Compiled),
                Parse = (argument) => argument
            };

            List<CommandEntity> entities = new List<CommandEntity>()
            {
                new CommandEntity()
                {
                    Synonyms = new[] { "help", "h" },
                    Flags = new[]
                    {
                        new Flag()
                        {
                            Name = "dick",
                            Shortname = "d"
                        },
                        new Flag()
                        {
                            Name = "help",
                            Shortname = "h"
                        }
                    },
                    Arguments = new[]
                    {
                        new Argument()
                        {
                            Name = "age",
                            IsRequired = true,
                            ArgumentType = BaseArgumentTypes.Int
                        },
                        new Argument()
                        {
                            Name = "height",
                            IsRequired = true,
                            ArgumentType = BaseArgumentTypes.Double
                        },
                        new Argument()
                        {
                            Name = "name",
                            IsRequired = false,
                            ArgumentType = BaseArgumentTypes.String
                        }
                    },
                    CommandExecutor = (arguments, flags) =>
                    {
                        Console.WriteLine(string.Join(", ", arguments.Select(arg => $"{arg.Key} {arg.Value}")));
                        Console.WriteLine(string.Join(", ", flags.Select(flag => $"{flag.Name}")));
                    }
                }
            };

            string input = Console.ReadLine();
            Command command = entities.ParseCommand(input);
            command.Execute();

            Hint hint = new Hint(command.CommandEntity);
            foreach (var unit in hint.HintUnits)
            {
                Console.ResetColor();
                if (unit.HasDependecy)
                {
                    Console.ForegroundColor = ColorScheme[unit.Dependency.HintPart];
                    Console.Write($"{unit.Dependency.Value}::");
                }

                Console.ForegroundColor = ColorScheme[unit.HintPart];
                Console.Write($"{unit.Value} ");
            }
        }

        public static string ParseHintUnit(HintUnit unit)
        {
            if (unit.HasDependecy)
                return $"{unit.Dependency.Value} {unit.Value}";

            return unit.Value;
        }
    }
}
