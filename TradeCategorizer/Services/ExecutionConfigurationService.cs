using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeCategorizer.Services
{
    public class ExecutionConfigurationService : IExecutionConfigurationService
    {
        private DateTime _referenceDate { get; set; }

        public void SetReferenceDate(DateTime referenceDate)
        {
            _referenceDate = referenceDate;
        }

        public DateTime GetRefenceDate()
        {
            return _referenceDate;
        }
    }
}
