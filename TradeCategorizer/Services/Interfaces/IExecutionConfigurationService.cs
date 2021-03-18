using System;

namespace TradeCategorizer.Services
{
    /// <summary>
    /// Interface for the service that manages the configuration of the curent execution
    /// </summary>
    public interface IExecutionConfigurationService
    {
        /// <summary>
        /// Set the Reference Date for the current execution
        /// </summary>
        /// <param name="referenceDate">The Reference Date</param>
        DateTime GetRefenceDate();
        /// <summary>
        /// Gets the Reference for the current execution
        /// </summary>
        /// <returns>The Reference Date</returns>
        void SetReferenceDate(DateTime referenceDate);
    }
}