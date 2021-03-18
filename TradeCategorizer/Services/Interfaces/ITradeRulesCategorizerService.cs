using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;

namespace TradeCategorizer.Services
{
    /// <summary>
    /// Interface for the service that categorizes Trades based on the implmented Rules
    /// </summary>
    public interface ITradeRulesCategorizerService
    {
        /// <summary>
        /// Finds the category of the trade by evaluating all <see cref="ITradeRule"/>.
        /// There is a fallback rule <see cref="UncategorizedTradeRule"/> if all other rules fail.
        /// </summary>
        /// <param name="trade">Trade being categorized.</param>
        /// <returns>The category of the Trade.</returns>
        TradeCategory CategorizeTrade(Trade trade);
    }
}