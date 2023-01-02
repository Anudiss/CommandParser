using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Array = CommandParser.Command.Array;

namespace CommandParser.Command.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        public static readonly List<CommandEntity> Entities = new List<CommandEntity>()
        {
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
                Synonyms = new[] { "parsingTest" },
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

        [TestMethod()]
        public void TestTokenPattern()
        {
            // User input
            string input = "parsingTest 18 [()]";
        }
        
        [TestMethod()]
        public void DoLexicalAnalyzeTest()
        {
            // User input
            string input = "print \"Hello World,  2!\"";

            // Excepted
            Command excepted = new Command(commandEntity: Entities.First(),
                                           arguments: new Dictionary<string, object>()
                                           {
                                               { "message", "Hello World,  2!" }
                                           },
                                           flags: System.Array.Empty<Flag>());

            // Act
            Command act = Entities.ParseCommand(input);

            // Assert
            Assert.IsTrue(excepted.CommandEntity == act.CommandEntity, "Неверно определена структура команды");
            
            foreach (var exceptedArgument in excepted.Arguments)
            {
                Assert.IsTrue(act.Arguments.ContainsKey(exceptedArgument.Key), $"Не определён аргумент {exceptedArgument.Key}");
                Assert.IsTrue(act.Arguments[exceptedArgument.Key] == exceptedArgument.Value, $"Значение аргумента было {act.Arguments[exceptedArgument.Key]}, ожидалось {exceptedArgument.Value}");
            }
        }
    }
}