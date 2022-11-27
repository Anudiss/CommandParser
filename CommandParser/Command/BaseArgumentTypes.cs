using System.Text.RegularExpressions;

namespace CommandParser.Command
{
    public static class BaseArgumentTypes
    {
        public static ArgumentType Int = new ArgumentType()
        {
            Name = "Int",
            ArgumentParsingRegex = new Regex(@"\d+", RegexOptions.Compiled),
            Parse = (arg) => int.Parse(arg),
            Validator = (arg) => true
        };

        public static ArgumentType String = new ArgumentType()
        {
            Name = "String",
            ArgumentParsingRegex = new Regex(@"(?<Text>[^\u0022]+)|([\u0022](?<Text>[^\u0022]*)[\u0022])", RegexOptions.Compiled),
            Parse = (arg) => arg.ToString(),
            Validator = (arg) => true
        };

        public static ArgumentType Double = new ArgumentType()
        {
            Name = "Double",
            ArgumentParsingRegex = new Regex(@"\d+([\,\.]\d+)?", RegexOptions.Compiled),
            Parse = (arg) => double.Parse(arg.ToString().Replace('.', ',')),
            Validator = (arg) => true
        };
    }
}
