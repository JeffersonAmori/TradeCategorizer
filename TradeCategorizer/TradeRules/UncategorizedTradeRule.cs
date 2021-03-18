using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;
using TradeCategorizer.TradeRules.Interfaces;

namespace TradeCategorizer.TradeRules
{
    /// <summary>
    /// Fallback rule. This will be checked when any other Rule match.
    /// </summary>
    public class UncategorizedTradeRule : ITradeRule
    {

        /// <summary>
        /// Gets the precedence of the rule.
        /// </summary>
        /// <returns>The precedence of the rule.</returns>
        public TradeCategory GetCategory()
        {
            return TradeCategory.Uncategorized;
        }

        /// <summary>
        /// Determine if the trade fit this category.
        /// </summary>
        /// <returns>True if the trade fits this category. False otherwise.</returns>
        public int GetPrecedence()
        {
            return (int)TradeCategory.Uncategorized;
        }


        /// <summary>
        /// Gets the category of the trade
        /// </summary>
        /// <returns>The category of the trade</returns>
        public bool IsMatch(Trade trade)
        {
            return true;
        }
    }
}
