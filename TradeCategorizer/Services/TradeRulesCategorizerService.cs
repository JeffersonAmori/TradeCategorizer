using System.Collections.Generic;
using System.Linq;
using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;
using TradeCategorizer.TradeRules;
using TradeCategorizer.TradeRules.Interfaces;

namespace TradeCategorizer.Services
{
    /// <summary>
    /// Service for categorize Trades based on the implmented Rules
    /// </summary>
    public class TradeRulesCategorizerService : ITradeRulesCategorizerService
    {
        private readonly IEnumerable<ITradeRule> _tradeRules;

        /// <summary>
        /// Service for categorizing Trades
        /// </summary>
        /// <param name="tradeRules"></param>
        public TradeRulesCategorizerService(IEnumerable<ITradeRule> tradeRules)
        {
            _tradeRules = tradeRules.OrderBy(rule => rule.GetPrecedence());
        }

        /// <summary>
        /// Finds the category of the trade by evaluating all <see cref="ITradeRule"/>.
        /// There is a fallback rule <see cref="UncategorizedTradeRule"/> if all other rules fail.
        /// </summary>
        /// <param name="trade">Trade being categorized.</param>
        /// <returns>The category of the Trade.</returns>
        public TradeCategory CategorizeTrade(Trade trade)
        {
            return _tradeRules
                .Where(tradeRule => tradeRule.IsMatch(trade))
                .FirstOrDefault()
                .GetCategory();
        }
    }
}
