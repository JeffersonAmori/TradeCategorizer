using System;

namespace TradeCategorizer.Services
{
    public interface IExecutionConfigurationService
    {
        DateTime GetRefenceDate();
        void SetReferenceDate(DateTime referenceDate);
    }
}