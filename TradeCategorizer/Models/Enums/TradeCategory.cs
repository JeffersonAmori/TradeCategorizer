namespace TradeCategorizer.Models.Enums
{
    /// <summary>
    /// Represents the possible values of a Trade.
    /// This Enum also control the order in which the Trade Rules are evaluated - the lesser the number higher the precedence.
    /// </summary>
    public enum TradeCategory
    {
        Defaulted = 0,
        Highrisk,
        Mediumrisk,
        Uncategorized = int.MaxValue
    }
}
