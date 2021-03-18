using Microsoft.Extensions.DependencyInjection;
using System;
using TradeCategorizer.Models;
using TradeCategorizer.Models.Enums;
using TradeCategorizer.Services;
using Xunit;

namespace TradeCategorizer.Tests
{
    public class TradeCategorizerTests : IClassFixture<TradeCategorizerTestsFixture>
    {
        private readonly ITradeRulesCategorizerService _tradeRulesCategorizerService;
        private readonly IExecutionConfigurationService _executionConfigurationService;

        public TradeCategorizerTests(TradeCategorizerTestsFixture fixture)
        {
            _tradeRulesCategorizerService = fixture.ServiceProvider.GetService<ITradeRulesCategorizerService>();
            _executionConfigurationService = fixture.ServiceProvider.GetService<IExecutionConfigurationService>();
        }

        [Theory(DisplayName ="Category should be DEFAULTED")]
        [InlineData(400000, "Public", "07/01/2020", "12/11/2020")]
        [InlineData(4000000, "Public", "01/01/1990", "12/11/2050")]
        [InlineData(2000000, "Public", "01/01/1900", "12/11/1901")]
        public void Categorizer_Should_Return_Category_Defaulted(double value, string clientSector, string nextPaymentDate, string referenceDate)
        {
            var trade = new Trade(value, clientSector, DateTime.Parse(nextPaymentDate));
            _executionConfigurationService.SetReferenceDate(DateTime.Parse(referenceDate));

            var category = _tradeRulesCategorizerService.CategorizeTrade(trade);

            Assert.Equal(TradeCategory.Defaulted, category);
        }

        [Theory(DisplayName = "Category should be HIGHRISK")]
        [InlineData(2000000, "Private", "12/29/2025", "12/11/2020")]
        [InlineData(4000000, "Private", "01/01/2021", "12/11/2020")]
        [InlineData(8000000, "Private", "05/05/2005", "12/11/2000")]
        public void Categorizer_Should_Return_Category_HighRisk(double value, string clientSector, string nextPaymentDate, string referenceDate)
        {
            var trade = new Trade(value, clientSector, DateTime.Parse(nextPaymentDate));
            _executionConfigurationService.SetReferenceDate(DateTime.Parse(referenceDate));

            var category = _tradeRulesCategorizerService.CategorizeTrade(trade);

            Assert.Equal(TradeCategory.Highrisk, category);
        }

        [Theory(DisplayName = "Category should be MEDIUMRISK")]
        [InlineData(5000000, "Public", "01/02/2024", "12/11/2020")]
        [InlineData(3000000, "Public", "10/26/2023", "12/11/2020")]
        [InlineData(8000000, "Public", "10/26/2025", "12/11/2020")]
        public void Categorizer_Should_Return_Category_MediumRisk(double value, string clientSector, string nextPaymentDate, string referenceDate)
        {
            var trade = new Trade(value, clientSector, DateTime.Parse(nextPaymentDate));
            _executionConfigurationService.SetReferenceDate(DateTime.Parse(referenceDate));

            var category = _tradeRulesCategorizerService.CategorizeTrade(trade);

            Assert.Equal(TradeCategory.Mediumrisk, category);
        }

        [Theory(DisplayName = "Category should be UNCATEGORIZED")]
        [InlineData(200000, "Private", "12/29/2025", "12/11/2020")]
        [InlineData(700000, "Public", "12/29/2025", "12/11/2020")]
        [InlineData(999999, "Private", "01/01/2021", "12/11/2020")]
        public void Categorizer_Should_Return_Category_Uncategorized(double value, string clientSector, string nextPaymentDate, string referenceDate)
        {
            var trade = new Trade(value, clientSector, DateTime.Parse(nextPaymentDate));
            _executionConfigurationService.SetReferenceDate(DateTime.Parse(referenceDate));

            var category = _tradeRulesCategorizerService.CategorizeTrade(trade);

            Assert.Equal(TradeCategory.Uncategorized, category);
        }
    }
}
