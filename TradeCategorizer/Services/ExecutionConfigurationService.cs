using System;

namespace TradeCategorizer.Services
{
    /// <summary>
    /// Service for managing the configuration of the curent execution
    /// </summary>
    public class ExecutionConfigurationService : IExecutionConfigurationService
    {
        private DateTime _referenceDate { get; set; }

        /// <summary>
        /// Set the Reference Date for the current execution
        /// </summary>
        /// <param name="referenceDate">The Reference Date</param>
        public void SetReferenceDate(DateTime referenceDate)
        {
            _referenceDate = referenceDate;
        }

        /// <summary>
        /// Gets the Reference for the current execution
        /// </summary>
        /// <returns>The Reference Date</returns>
        public DateTime GetRefenceDate()
        {
            return _referenceDate;
        }
    }
}
