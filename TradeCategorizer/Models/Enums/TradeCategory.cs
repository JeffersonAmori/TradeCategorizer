using System.ComponentModel;

namespace TradeCategorizer.Models.Enums
{
    /// <summary>
    /// Represents the possible values of a Trade.
    /// This Enum also control the order in which the Trade Rules are evaluated - the lesser the number higher the precedence.
    /// </summary>
    public enum TradeCategory
    {
        [Description("DEFAULTED")]
        Defaulted = 0,
        [Description("HIGHRISK")]
        Highrisk,
        [Description("MEDIUMRISK")]
        Mediumrisk,
        [Description("UNCATEGORIZED")]
        Uncategorized = int.MaxValue
    }
}
