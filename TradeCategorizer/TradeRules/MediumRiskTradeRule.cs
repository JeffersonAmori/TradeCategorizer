using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;
using TradeCategorizer.TradeRules.Interfaces;

namespace TradeCategorizer.TradeRules
{
    /// <summary>
    /// Rule for Medium Risk Trades
    /// </summary>
    public class MediumRiskTradeRule : ITradeRule
    {
        /// <summary>
        /// Gets the precedence of the rule.
        /// </summary>
        /// <returns>The precedence of the rule.</returns>
        public TradeCategory GetCategory()
        {
            return TradeCategory.Mediumrisk;
        }

        /// <summary>
        /// Determine if the trade fit this category.
        /// </summary>
        /// <returns>True if the trade fits this category. False otherwise.</returns>
        public int GetPrecedence()
        {
            return (int)TradeCategory.Mediumrisk;
        }

        /// <summary>
        /// Gets the category of the trade
        /// </summary>
        /// <returns>The category of the trade</returns>
        public bool IsMatch(Trade trade)
        {
            return (trade.Value > 1000000 && trade.ClientSector.ToUpper() == "PUBLIC");
        }
    }
}
