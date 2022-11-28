﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommandParser.Command;
using System.Collections.Generic;
using System.Diagnostics;
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
                        Name = "age",
                        ArgumentType = BaseArgumentTypes.Int,
                        IsRequired = true
                    },
                    new Argument()
                    {
                        Name = "height",
                        ArgumentType = BaseArgumentTypes.Double,
                        IsRequired = true
                    },
                    new Argument()
                    {
                        Name = "text",
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

        public static readonly string Input = $"   help    -d    43   --help   93,232    {'\u0022'}    kakksdkka    kwkwkd     kkkkk  {'\u0022'}";

        [TestMethod()]
        public void ParseCommandTest()
        {
            // Arrange
            Command excepted = new Command(
                commandEntity: Entities[0],
                arguments: new Dictionary<string, object>()
                           {
                              { "age", 43 },
                              { "height", 93.232 },
                              { "text", "    kakksdkka    kwkwkd     kkkkk  " }
                           },
                flags: Entities[0].Flags
                );

            // Act
            Command actual = Entities.ParseCommand(Input);

            // Assert
            Assert.IsTrue(excepted == actual);
        }

        [TestMethod()]
        public void DoLexicalAnalyzeTest()
        {
            // Arrange
            string[] excepted = new[]
            {
                   "help", "-d", "43", "--help", "93,232", "kakksdkka    kwkwkd     kkkkk"
            };

            //Act
            string[] actual = CommandParser.DoLexicalAnalyze(Input).ToArray();

            //Assert
            Assert.IsTrue(excepted.Except(actual).Count() == 0);
        }
    }
}