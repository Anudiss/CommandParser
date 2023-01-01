using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace CommandParser.Command.Tests
{
    [TestClass()]
    public class CommandParserTests
    {
        public static readonly List<CommandEntity> Entities = new List<CommandEntity>()
        {
            new CommandEntity()
            {
                Synonyms = new[] { "help", "h" },
                Arguments = new[]
                {
                    new Argument()
                    {
                        Name = "Age",
                        ArgumentType = BaseArgumentTypes.Int,
                        IsRequired = true
                    },
                    new Argument()
                    {
                        Name = "Height",
                        ArgumentType = BaseArgumentTypes.Double,
                        IsRequired = true
                    },
                    new Argument()
                    {
                        Name = "Text",
                        ArgumentType = BaseArgumentTypes.String,
                        IsRequired = false
                    }
                },
                Flags = new[]
                {
                    new Flag()
                    {
                        Name = "help",
                        Shortname = "h"
                    },
                    new Flag()
                    {
                        Name = "dick",
                        Shortname = "d"
                    }
                }
            }
        };

        public static readonly string Input = $"  help  -d  23   --help  93,332  {'\u0022'}  Hello  World   !{'\u0022'}";

        [TestMethod()]
        public void DoLexicalAnalyzeTest()
        {
            // Arrange
            string[] excepted = new[]
            {
                "help", "-d", "23", "--help", "93,332", "Hello  World   !"
            };

            // Act
            string[] actual = CommandParser.Tokenize(Input).ToArray();

            // Assert
            Assert.AreEqual(excepted.Count(), actual.Count());
            for (int i = 0; i < excepted.Length; i++)
                Assert.IsTrue(excepted[i] == actual[i]);
        }
    }
}