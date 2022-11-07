using CommandParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CommandParserTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            ArgumentType Int = new ArgumentType()
            {
                Name = "int",
                Regex = new Regex(@"\d+", RegexOptions.Compiled),
                Parse = (argument) => int.Parse(argument)
            };
            ArgumentType Double = new ArgumentType()
            {
                Name = "double",
                Regex = new Regex(@"\d+(\,\d+)?", RegexOptions.Compiled),
                Parse = (argument) => double.Parse(argument)
            };
            ArgumentType String = new ArgumentType()
            {
                Name = "string",
                Regex = new Regex(@".*", RegexOptions.Compiled),
                Parse = (argument) => argument
            };

            List<CommandEntity> entities = new List<CommandEntity>()
            {
                new CommandEntity()
                {
                    Synonyms = new[] { "help", "h" },
                    Flags = new[]
                    { new Flag()
                        {
                            Name = "dick",
                            Shortname = "d"
                        }
                    },
                    Arguments = new[]
                    {
                        new Argument()
                        {
                            Name = "age",
                            IsRequired = true,
                            ArgumentType = Int
                        },
                        new Argument()
                        {
                            Name = "height",
                            IsRequired = true,
                            ArgumentType = Double
                        },
                        new Argument()
                        {
                            Name = "name",
                            IsRequired = false,
                            ArgumentType = String
                        }
                    }
                }
            };

            Command command = entities.ParseCommand("  HelP   --dick  43  -d  48,231 --dick Alex    -dick");
            Console.WriteLine($"Arguments: {string.Join(", ", command.Arguments.Select(argument => $"{argument.Key}:{argument.Value}"))}\nFlags: {string.Join(", ", command.Flags.Select(flag => flag.Name))}");
        }
    }
}
