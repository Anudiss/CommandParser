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
                        return null;
                    },
                    Arguments = new[]
                    {
                        new Argument()
                        {
                            Name = "Numbers",
                            ArgumentType = new Array(BaseArgumentTypes.Double)
                            {
                                Name = "DoubleArray",
                                Validator = (arg) => true
                            }
                        }
                    }
                },
                new CommandEntity()
                {
                    Synonyms = new[] { "print" },
                    Arguments = new[]
                    {
                        new Argument()
                        {
                            Name = "message",
                            ArgumentType = BaseArgumentTypes.String
                        }
                    }
                },
                new CommandEntity()
                {
                    Synonyms = new[] { "test" },
                    Arguments = new[]
                    {
                        new Argument()
                        {
                            Name = "Age",
                            ArgumentType = BaseArgumentTypes.Int
                        },
                        new Argument()
                        {
                            Name = "Scores",
                            ArgumentType = new Array(new TupleType()
                            {
                                Name = "Scores",
                                TypesInside = new[]
                                {
                                    new Argument()
                                    {
                                        Name = "Name",
                                        ArgumentType = BaseArgumentTypes.String
                                    },
                                    new Argument()
                                    {
                                        Name = "Score",
                                        ArgumentType = BaseArgumentTypes.Double
                                    }
                                }
                            })
                            {
                                Name = "NameScoreArray",
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

            while (true)
            {
                Console.Clear();
                try
                {
                    Command command = entities.ParseCommand(Console.ReadLine().Trim());
                    Console.WriteLine($"{string.Join(", ", command.Arguments)}");
                    foreach (var array in ((object[])command.Arguments["Numbers"]).Cast<double>())
                        Console.WriteLine($"{array}");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                Console.ReadKey();
            }
        }
    }
}
