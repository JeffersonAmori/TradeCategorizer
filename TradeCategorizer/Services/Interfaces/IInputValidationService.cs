namespace TradeCategorizer.Services
{
    /// <summary>
    /// Interface for the Input Validation Service
    /// </summary>
    public interface IInputValidationService
    {
        /// <summary>
        /// Validates the data in the Header of a portfolio
        /// </summary>
        /// <param name="fileLines">The portfolio</param>
        /// <param name="errorMessage">The error messagem describing while the validation failed. Empty when the validation passes</param>
        /// <returns>True if validation passes. False othrewise.</returns>
        bool ValidateHeaderData(string[] fileLines, out string errorMessage);

        /// <summary>
        /// Validates a line of the portfolio
        /// </summary>
        /// <param name="fileLines">The portfolio</param>
        /// <param name="errorMessage">The error messagem describing while the validation failed. Empty when the validation passes</param>
        /// <returns>True if validation passes. False othrewise.</returns>
        bool ValidateTradeInfo(string fileLine, out string errorMessage);
    }
}