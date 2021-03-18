using System;

namespace TradeCategorizer.Configuration
{
    sealed class ExecutionConfiguration
    {
        private static ExecutionConfiguration _instance;
        private ExecutionConfiguration() { }

        public static ExecutionConfiguration Instance
        {
            get => _instance ??= new ExecutionConfiguration();
        }


    }
}
