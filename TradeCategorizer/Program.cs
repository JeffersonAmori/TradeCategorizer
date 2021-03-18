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
        private static IExecutionConfigurationService executionConfigurationService;
        private static ITradeRulesCategorizerService tradeRulesCategorizerService;

        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            SetUp(serviceProvider);

            var fileLines = File.ReadAllLines("F:\\portfolio.txt");

            if (!InputIsValid(fileLines, out string message))
            {
                Console.WriteLine("Error: " + message);
                return;
            }

            uint numberOfTradesInPortfolio = uint.Parse(fileLines[lineNumberForNumberOfTradesInPortfolio]);

            for (int i = 2; i <= numberOfTradesInPortfolio + 1; i++)
            {
                Trade trade = CreateTradeFromString(fileLines[i]);

                Console.WriteLine(tradeRulesCategorizerService.CategorizeTrade(trade)
                    .ToString()
                    .ToUpper());
            }
        }

        private static bool InputIsValid(string[] fileLines, out string validationMessage)
        {
            string referenceDateLine = fileLines[0];

            if (!DateTime.TryParse(referenceDateLine, out DateTime referenceDate))
            {
                validationMessage = "Reference date in file is not a valid date (line 1).";
                return false;
            }

            executionConfigurationService.SetReferenceDate(referenceDate);

            string secondLine = fileLines[lineNumberForNumberOfTradesInPortfolio];
            if (!uint.TryParse(secondLine, out uint numberOfTradesInPortfolio))
            {
                validationMessage = "Number of trades in portfolio is not a valid number (line 2).";
                return false;
            }

            validationMessage = string.Empty;
            return true;
        }

        private static void SetUp(ServiceProvider serviceProvider)
        {
            executionConfigurationService = serviceProvider.GetService<IExecutionConfigurationService>();
            tradeRulesCategorizerService = serviceProvider.GetService<ITradeRulesCategorizerService>();
        }

        private static Trade CreateTradeFromString(string fileLine)
        {
            var lineValues = fileLine.Split(" ");
            var trade = new Trade(value: double.Parse(lineValues[0]),
                                  clientSector: lineValues[1],
                                  nextPaymentDate: DateTime.Parse(lineValues[2]));
            return trade;
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

            return serviceCollection.BuildServiceProvider();
        }
    }
}
