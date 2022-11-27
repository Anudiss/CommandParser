using CommandParser.Command;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CommandParser.Hints
{
    #region Подсказка
    /// <summary>
    /// Класс подсказки
    /// </summary>
    public class Hint
    {
        /// <summary>
        /// Единицы подсказки
        /// </summary>
        public IEnumerable<HintUnit> HintUnits { get; set; }

        /// <summary>
        /// Единицы подсказки, сгруппированные по типу
        /// </summary>
        public IEnumerable<IGrouping<HintPart, HintUnit>> HintGroups => HintUnits.GroupBy(unit => unit.HintPart);

        /// <summary>
        /// Конструктор подсказки
        /// </summary>
        /// <param name="entity">Сущность команды</param>
        public Hint(CommandEntity entity) => HintUnits = ParseCommandEntityToHint(entity);

        /// <summary>
        /// Метод парсинга сущности команды в подсказку
        /// </summary>
        /// <param name="entity">Сущность команды</param>
        /// <returns>Перечисление единиц подсказки</returns>
        public static IEnumerable<HintUnit> ParseCommandEntityToHint(CommandEntity entity)
        {
            yield return new HintUnit()
            {
                HintPart = HintPart.CommandName,
                Value = entity.Synonyms[0]
            };

            foreach (var argument in entity.Arguments)
                yield return new HintUnit()
                {
                    HintPart = argument.IsRequired ? HintPart.RequiredArgument : HintPart.NotRequiredArgument,
                    Value = argument.Name,
                    Dependency = new HintUnit()
                    {
                        HintPart = HintPart.ArgumentType,
                        Value = argument.ArgumentType.Name
                    }
                };
        }
    }
    #endregion
    #region Единица подсказки
    public class HintUnit
    {
        private string _value;

        /// <summary>
        /// Часть подсказки
        /// </summary>
        public HintPart HintPart { get; set; }

        /// <summary>
        /// Значение единицы подсказки
        /// </summary>
        public string Value
        {
            get => _value;
            set
            {
                switch (HintPart)
                {
                    case HintPart.ArgumentType:
                        _value = CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(value);
                        break;
                    default:
                        _value = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Часть подсказки от которой зависит эта
        /// </summary>
        public HintUnit Dependency { get; set; }

        /// <summary>
        /// Имеет ли зависимость
        /// </summary>
        public bool HasDependecy => Dependency != null;
    }
    #endregion
    #region Часть подсказки
    /// <summary>
    /// Перечисление возможных частей подсказки
    /// </summary>
    public enum HintPart
    {
        CommandName,
        ArgumentType,
        NotRequiredArgument,
        RequiredArgument,
        Flag
    }
    #endregion
}
