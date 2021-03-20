using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TradeCategorizer.Models;
using TradeCategorizer.Resources;
using TradeCategorizer.Services;
using TradeCategorizer.Tests;
using Xunit;

namespace TradeCategorizer.Testss
{
    public class InputValidationServiceTests : IClassFixture<TradeCategorizerTestsFixture>
    {
        private readonly IInputValidationService _inputValidationService;

        public InputValidationServiceTests(TradeCategorizerTestsFixture fixture)
        {
            _inputValidationService = fixture.ServiceProvider.GetService<IInputValidationService>();
        }

        [Fact]
        public void ValidateHeaderData_Should_Return_Header_Input_Is_Valid_When_Passed_A_Valid_Inputs()
        {
            var fileLines = new[] { "01/01/2000", "3" };

            bool headerDataIsValid = _inputValidationService.ValidateHeaderData(fileLines, out string errorMessage);

            Assert.True(headerDataIsValid);
            Assert.Equal(string.Empty, errorMessage);
        }

        [Fact]
        public void ValidateHeaderData_Should_Return_Date_Not_Valid_When_Passed_A_String_Instead_Of_A_Date()
        {
            var fileLines = new[] { "InvalidDate", "3" };

            bool headerDataIsValid = _inputValidationService.ValidateHeaderData(fileLines, out string errorMessage);

            Assert.False(headerDataIsValid);
            Assert.Equal(ErrorMessages.InvalidReferenceDateInFileHeader, errorMessage);
        }

        [Fact]
        public void ValidateHeaderData_Should_Return_Number_Of_Lines_Of_Trades_Invalid_When_Passed_A_String_Instead_Of_A_Number()
        {
            var fileLines = new[] { "01/01/2000", "ABC" };

            bool headerDataIsValid = _inputValidationService.ValidateHeaderData(fileLines, out string errorMessage);

            Assert.False(headerDataIsValid);
            Assert.Equal(ErrorMessages.InvalidValueForNumberOfTradesInFileHeader, errorMessage);
        }

        [Theory]
        [InlineData("2000 Private 02/02/2022")]
        [InlineData("3000000 Public 01/01/1995")]
        [InlineData("9000000 Public 01/01/2020")]
        public void ValidateTradeInfo_Should_Return_Trade_Line_Is_Valid(string tradeLine)
        {

            bool tradeInfoIsValid = _inputValidationService.ValidateTradeInfo(tradeLine, out string errorMessage);

            Assert.True(tradeInfoIsValid);
            Assert.Equal(string.Empty, errorMessage);
        }

        [Theory]
        [InlineData("200000 ")]
        [InlineData("4000000 Public")]
        [InlineData("5000000 Public 01/01/2020 ANOTHER_COLUMN")]
        public void ValidateTradeInfo_Should_Return_Number_Of_Columns_Invalid_When_Different_Of_3_Columns(string tradeLine)
        {
            bool tradeInfoIsValid = _inputValidationService.ValidateTradeInfo(tradeLine, out string errorMessage);

            Assert.False(tradeInfoIsValid);
            Assert.Equal(string.Format(ErrorMessages.InvalidNumberOfColumnsInTradeLine,
                    3, tradeLine), errorMessage);
        }

        [Theory]
        [InlineData("ASD Private 02/02/2022")]
        [InlineData("ABC Public 01/01/1995")]
        [InlineData("NOT_A_VALID_TRADE_VALUE Public 01/01/2020")]
        public void ValidateTradeInfo_Should_Return_Trade_Invalid_When_Passed_A_String(string tradeLine)
        {
            var tradeInfo = tradeLine.Split(" ");

            bool tradeInfoIsValid = _inputValidationService.ValidateTradeInfo(tradeLine, out string errorMessage);

            Assert.False(tradeInfoIsValid);
            Assert.Equal(string.Format(ErrorMessages.TradeValueIsNotANumber,
                    tradeInfo[0], tradeLine), errorMessage);
        }

        [Theory]
        [InlineData("2000 Privat 02/02/2022")]
        [InlineData("3000000 Sector 01/01/1995")]
        [InlineData("9000000 1 01/01/2020")]
        public void ValidateTradeInfo_Should_Return_Client_Sector_Invaid_When_Passed_A_Sector_Out_Of_Expected_Values(string tradeLine)
        {
            var tradeInfo = tradeLine.Split(" ");
            string clientSector = tradeInfo[1];

            bool tradeInfoIsValid = _inputValidationService.ValidateTradeInfo(tradeLine, out string errorMessage);

            Assert.False(tradeInfoIsValid);
            Assert.Equal(string.Format(ErrorMessages.ClientSectorNotInExpectedValues,
                    string.Join(", ", typeof(ClientSector).GetFields().Select(m => m.Name)), clientSector, tradeLine), errorMessage);
        }

        [Theory]
        [InlineData("2000 Private ABC")]
        [InlineData("3000000 Public THIS_IS_NOT_A_DATE")]
        [InlineData("9000000 Public TRUE")]
        public void ValidateTradeInfo_Should_Return_Date_Of_Payment_Invaid_When_Passed_An_Invalid_Date(string tradeLine)
        {
            var tradeInfo = tradeLine.Split(" ");

            bool tradeInfoIsValid = _inputValidationService.ValidateTradeInfo(tradeLine, out string errorMessage);

            Assert.False(tradeInfoIsValid);
            Assert.Equal(string.Format(ErrorMessages.InvalidDateOfNextPayment, tradeInfo[2], tradeLine), errorMessage);
        }
    }
}
