using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;

namespace TradeCategorizer.TradeRules.Interfaces
{
    /// <summary>
    /// Interface for the Trades Rules
    /// </summary>
    public interface ITradeRule
    {
        /// <summary>
        /// Gets the precedence of the rule.
        /// </summary>
        /// <returns>The precedence of the rule.</returns>
        int GetPrecedence();
        /// <summary>
        /// Determine if the trade fit this category.
        /// </summary>
        /// <returns>True if the trade fits this category. False otherwise.</returns>
        bool IsMatch(Trade trade);
        /// <summary>
        /// Gets the category of the trade
        /// </summary>
        /// <returns>The category of the trade</returns>
        TradeCategory GetCategory();
    }
}
