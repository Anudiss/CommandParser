using System.Text.RegularExpressions;

namespace CommandParser.Command
{
    public static class BaseArgumentTypes
    {
        public static readonly ArgumentType Int = new ArgumentType()
        {
            Name = "Int",
            ArgumentParsingRegex = new Regex(@"\d+", RegexOptions.Compiled),
            Parse = (arg) => int.Parse(arg),
            Validator = (arg) => true
        };

        public static readonly ArgumentType String = new ArgumentType()
        {
            Name = "String",
            ArgumentParsingRegex = new Regex(@".+", RegexOptions.Compiled),
            Parse = (arg) => arg.ToString(),
            Validator = (arg) => true
        };

        public static readonly ArgumentType Double = new ArgumentType()
        {
            Name = "Double",
            ArgumentParsingRegex = new Regex(@"\d+([\,\.]\d+)?", RegexOptions.Compiled),
            Parse = (arg) => double.Parse(arg.ToString().Replace('.', ',')),
            Validator = (arg) => true
        };
    }
}
