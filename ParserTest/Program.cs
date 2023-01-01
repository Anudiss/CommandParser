using CommandParser.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using Tuple = CommandParser.Command.TupleType;
using Array = CommandParser.Command.Array;

namespace ParserTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<CommandEntity> entities = new List<CommandEntity>()
            {
                new CommandEntity()
                {
                    Synonyms = new[] { "help", "h" },
                    Arguments = new[]
                    {
                        new Argument()
                        {
                            Name = "ASD",
                            ArgumentType = new Tuple()
                            {
                                TypesInside = new[] {
                                    new Argument()
                                    {
                                        Name = "FirstNumber",
                                        ArgumentType = BaseArgumentTypes.Int
                                    },
                                    new Argument()
                                    {
                                        Name = "tu",
                                        ArgumentType = new Tuple()
                                        {
                                            TypesInside = new[] {
                                                new Argument()
                                                {
                                                    Name = "SecondNumber",
                                                    ArgumentType = BaseArgumentTypes.String
                                                },
                                                new Argument()
                                                {
                                                    Name = "aSD",
                                                    ArgumentType = BaseArgumentTypes.Double
                                                }
                                            },
                                            Name = "iiii",
                                            Validator = (arg) => true
                                        }
                                    }
                                },
                                Name = "dddasd",
                                Validator = (arg) => true
                            }
                        }
                    }
                },
                new CommandEntity()
                {
                    Synonyms = new[] { "new", "array" },
                    Flags = System.Array.Empty<Flag>(),
                    CommandExecutor = (arguments, flags) =>
                    {

                    },
                    Arguments = new[]
                    {
                        new Argument()
                        {
                            Name = "Numbers",
                            ArgumentType = new Array(BaseArgumentTypes.String)
                            {
                                Name = "DoubleArray",
                                Validator = (arg) => true
                            }
                        }
                    }
                }
            };

            /*Command command = entities.ParseCommand("help (82, (\"Hello World\", 93,73))");
            Console.WriteLine($"{string.Join(", ", command.Arguments.Select(arg => $"{arg.Key}: {arg.Value}"))}");
            Dictionary<string, object> tuple = command.Arguments["ASD"] as Dictionary<string, object>;
            Console.WriteLine($"First: {tuple["FirstNumber"]}");
            Dictionary<string, object> tuple1 = tuple["tu"] as Dictionary<string, object>;
            Console.WriteLine($"Second: {tuple1["SecondNumber"]}");
            Console.WriteLine($"Third: {tuple1["aSD"]}");*/

            Command command = entities.ParseCommand("new   (Alexander,\"Hello, World!\")");
            Console.WriteLine($"{string.Join(", ", command.Arguments)}");
            Console.WriteLine($"{string.Join(", ", (command.Arguments["Numbers"] as object[]).Cast<string>())}");
        }
    }
}
