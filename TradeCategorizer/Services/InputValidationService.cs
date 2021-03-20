using System;
using System.Linq;
using TradeCategorizer.Models;
using TradeCategorizer.Resources;

namespace TradeCategorizer.Services
{

    /// <summary>
    /// Input Validation Service for portfolios
    /// </summary>
    public class InputValidationService : IInputValidationService
    {
        private const int numberOfExpectedColumnsInTradeLine = 3;

        /// <summary>
        /// Validates the data in the Header of a portfolio
        /// </summary>
        /// <param name="fileLines">The portfolio</param>
        /// <param name="errorMessage">The error messagem describing while the validation failed. Empty when the validation passes</param>
        /// <returns>True if validation passes. False othrewise.</returns>
        public bool ValidateHeaderData(string[] fileLines, out string errorMessage)
        {
            string referenceDateLine = fileLines[0];
            errorMessage = string.Empty;

            if (!DateTime.TryParse(referenceDateLine, out _))
            {
                errorMessage = ErrorMessages.InvalidReferenceDateInFileHeader;
                return false;
            }

            string lineWithNumberOfTradesInPortfolio = fileLines[1];
            if (!uint.TryParse(lineWithNumberOfTradesInPortfolio, out _))
            {
                errorMessage = ErrorMessages.InvalidValueForNumberOfTradesInFileHeader;
                return false;
            }

            return errorMessage == string.Empty;
        }

        /// <summary>
        /// Validates a line of the portfolio
        /// </summary>
        /// <param name="fileLines">The portfolio</param>
        /// <param name="errorMessage">The error messagem describing while the validation failed. Empty when the validation passes</param>
        /// <returns>True if validation passes. False othrewise.</returns>
        public bool ValidateTradeInfo(string fileLine, out string errorMessage)
        {
            var tradeInfo = fileLine.Split(" ");
            errorMessage = string.Empty;
            if (tradeInfo.Length != numberOfExpectedColumnsInTradeLine)
            {
                errorMessage = string.Format(ErrorMessages.InvalidNumberOfColumnsInTradeLine,
                    numberOfExpectedColumnsInTradeLine, fileLine);
                return false;
            }

            if (!double.TryParse(tradeInfo[0], out _))
            {
                errorMessage = string.Format(ErrorMessages.TradeValueIsNotANumber,
                    tradeInfo[0], fileLine);
                return false;
            }

            string clientSector = tradeInfo[1];
            if (clientSector.ToUpperInvariant() != ClientSector.Private &&
                clientSector.ToUpperInvariant() != ClientSector.Public)
            {
                errorMessage = string.Format(ErrorMessages.ClientSectorNotInExpectedValues,
                    string.Join(", ", typeof(ClientSector).GetFields().Select(m => m.Name)), clientSector, fileLine);
                return false;
            }

            if (!DateTime.TryParse(tradeInfo[2], out DateTime tradeNextPaymentDate))
            {
                errorMessage = string.Format(ErrorMessages.InvalidDateOfNextPayment, tradeInfo[2], fileLine);
                return false;
            }

            return true;
        }
    }
}
