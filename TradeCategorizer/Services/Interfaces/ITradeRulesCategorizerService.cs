using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;

namespace TradeCategorizer.Services
{
    public interface ITradeRulesCategorizerService
    {
        TradeCategory CategorizeTrade(Trade trade);
    }
}