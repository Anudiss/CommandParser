using System.Text.RegularExpressions;

namespace CommandParser.Command
{
    /// <summary>
    /// Класс флага
    /// </summary>
    public class Flag
    {
        /// <summary>
        /// Короткое название флага
        /// </summary>
        public string Shortname { get; set; }

        /// <summary>
        /// Полное название флага
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Регулярное выражение для парсинга флага
        /// </summary>
        public Regex Regex => new Regex($"(?<{Name}>(--{Name})|(-{Shortname}))");
    }
}
