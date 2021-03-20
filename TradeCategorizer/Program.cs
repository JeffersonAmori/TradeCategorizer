using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using TradeCategorizer.Models;
using TradeCategorizer.Services;
using TradeCategorizer.TradeRules.Interfaces;

namespace TradeCategorizer
{

    class Program
    {
        private const int lineNumberForNumberOfTradesInPortfolio = 1;
        private static IExecutionConfigurationService _executionConfigurationService;
        private static ITradeRulesCategorizerService _tradeRulesCategorizerService;
        private static IInputValidationService _InputValidationService;

        static void Main(string[] args)
        {
            try
            {
                var serviceProvider = ConfigureServices();

                Init(serviceProvider);

                var fileLines = File.ReadAllLines("F:\\portfolio.txt");

                if (!_InputValidationService.ValidateHeaderData(fileLines, out string errorMessage))
                    throw new InvalidDataException(errorMessage);

                string referenceDateLine = fileLines[0];
                _executionConfigurationService.SetReferenceDate(DateTime.Parse(referenceDateLine));

                uint numberOfTradesInPortfolio = uint.Parse(fileLines[lineNumberForNumberOfTradesInPortfolio]);

                for (int i = 2; i <= numberOfTradesInPortfolio + 1; i++)
                {
                    Trade trade = CreateTradeFromString(fileLines[i]);

                    Console.WriteLine(_tradeRulesCategorizerService.CategorizeTrade(trade)
                        .ToString()
                        .ToUpper());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            var interfaceType = typeof(ITradeRule);
            var implementationTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
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

        private static void Init(ServiceProvider serviceProvider)
        {
            _executionConfigurationService = serviceProvider.GetService<IExecutionConfigurationService>();
            _tradeRulesCategorizerService = serviceProvider.GetService<ITradeRulesCategorizerService>();
            _InputValidationService = serviceProvider.GetService<IInputValidationService>();
        }

        private static Trade CreateTradeFromString(string fileLine)
        {
            var tradeInfo = fileLine.Split(" ");
            if (!_InputValidationService.ValidateTradeInfo(fileLine, out string errorMessage))
                throw new InvalidDataException(errorMessage);

            var trade = new Trade(value: double.Parse(tradeInfo[0]),
                                  clientSector: tradeInfo[1],
                                  nextPaymentDate: DateTime.Parse(tradeInfo[2]));

            return trade;
        }
    }
}
