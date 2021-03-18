using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;
using TradeCategorizer.Services;
using TradeCategorizer.TradeRules.Interfaces;

namespace TradeCategorizer.TradeRules
{
    /// <summary>
    /// Rule for Trades passed the Next Payment date
    /// </summary>
    public class DefaultedTradeRule : ITradeRule
    {
        private readonly IExecutionConfigurationService _executionConfigurationService;

        public DefaultedTradeRule(IExecutionConfigurationService executionConfigurationService)
        {
            _executionConfigurationService = executionConfigurationService;
        }

        /// <summary>
        /// Gets the precedence of the rule.
        /// </summary>
        /// <returns>The precedence of the rule.</returns>
        public TradeCategory GetCategory()
        {
            return TradeCategory.Defaulted;
        }

        /// <summary>
        /// Determine if the trade fit this category.
        /// </summary>
        /// <returns>True if the trade fits this category. False otherwise.</returns>
        public int GetPrecedence()
        {
            return (int)TradeCategory.Defaulted;
        }

        public bool IsMatch(Trade trade)
        {
            return (_executionConfigurationService.GetRefenceDate() - trade.NextPaymentDate).Days > 30;
        }
    }
}
