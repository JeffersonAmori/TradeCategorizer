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
        private const int numberOfExpectedColumnsInTradeLine = 3;
        private static IExecutionConfigurationService _executionConfigurationService;
        private static ITradeRulesCategorizerService _tradeRulesCategorizerService;

        static void Main(string[] args)
        {
            try
            {
                var serviceProvider = ConfigureServices();

                SetUp(serviceProvider);

                var fileLines = File.ReadAllLines("F:\\portfolio.txt");

                ValidateHeaderData(fileLines);

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

            return serviceCollection.BuildServiceProvider();
        }

        private static void SetUp(ServiceProvider serviceProvider)
        {
            _executionConfigurationService = serviceProvider.GetService<IExecutionConfigurationService>();
            _tradeRulesCategorizerService = serviceProvider.GetService<ITradeRulesCategorizerService>();
        }

        private static void ValidateHeaderData(string[] fileLines)
        {
            string referenceDateLine = fileLines[0];
            if (!DateTime.TryParse(referenceDateLine, out _))
            {
                throw new InvalidDataException("Reference date in file is not a valid date (line 1).");
            }

            string secondLine = fileLines[lineNumberForNumberOfTradesInPortfolio];
            if (!uint.TryParse(secondLine, out _))
            {
                throw new InvalidDataException("Number of trades in portfolio is not a valid number (line 2).");
            }
        }

        private static Trade CreateTradeFromString(string fileLine)
        {
            var tradeInfo = fileLine.Split(" ");
            ValidateTradeInfo(fileLine, tradeInfo);

            var trade = new Trade(value: double.Parse(tradeInfo[0]),
                                  clientSector: tradeInfo[1],
                                  nextPaymentDate: DateTime.Parse(tradeInfo[2]));
            return trade;
        }

        private static void ValidateTradeInfo(string fileLine, string[] tradeInfo)
        {
            if (tradeInfo.Length != numberOfExpectedColumnsInTradeLine)
            {
                throw new InvalidDataException($"Invalid Number of columns, should be {numberOfExpectedColumnsInTradeLine}. Line with error: {fileLine}");
            }

            if (!double.TryParse(tradeInfo[0], out _))
            {
                throw new InvalidDataException($"Value of trade expected to be a number but actual value is {tradeInfo[0]}. Line with error: {fileLine}");
            }

            string clientSector = tradeInfo[1];
            if (clientSector.ToUpperInvariant() != ClientSector.Private &&
                clientSector.ToUpperInvariant() != ClientSector.Public)
            {
                throw new InvalidDataException($"Sector of client is expected to be one of the following: {string.Join(", ", typeof(ClientSector).GetFields().Select(m => m.Name))}. But actual value is {clientSector}. Line with error: {fileLine}");
            }

            if (!DateTime.TryParse(tradeInfo[2], out DateTime tradeNextPaymentDate))
            {
                throw new InvalidDataException($"Next payment date of trade is expected to be a date but actual value is {tradeNextPaymentDate}. Line with error: {fileLine}");
            }
        }
    }
}
