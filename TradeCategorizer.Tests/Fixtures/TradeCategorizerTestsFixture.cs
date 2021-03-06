using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using TradeCategorizer.Services;
using TradeCategorizer.TradeRules.Interfaces;

namespace TradeCategorizer.Tests
{
    public class TradeCategorizerTestsFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }

        public TradeCategorizerTestsFixture()
        {
            ServiceProvider = ConfigureServices();
        }

        private static ServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            var interfaceType = typeof(ITradeRule);
            var implementationTypes = typeof(ITradeRule).Assembly
                .GetTypes()
                .Where(p => !p.IsInterface && interfaceType.IsAssignableFrom(p));

            foreach (var type in implementationTypes)
            {
                serviceCollection.AddSingleton(typeof(ITradeRule), type);
            }

            serviceCollection.AddSingleton(typeof(ITradeRulesCategorizerService), typeof(TradeRulesCategorizerService));
            serviceCollection.AddSingleton(typeof(IExecutionConfigurationService), typeof(ExecutionConfigurationService));
            serviceCollection.AddSingleton(typeof(IInputValidationService), typeof(InputValidationService));


            return serviceCollection.BuildServiceProvider();
        }
    }
}
