using System;

namespace TradeCategorizer.Models
{
    /// <summary>
    /// Represents a commercial negotiation between a bank and a client.
    /// </summary>
    public class Trade
    {
        /// <summary>
        /// Instanciate a Trade
        /// </summary>
        public Trade() { }

        /// <summary>
        /// Instanciate a Trade
        /// </summary>
        /// <param name="value">Value of the Trade</param>
        /// <param name="clientSector">Client Sector of the Trade</param>
        /// <param name="nextPaymentDate">Next Payment Date of the Trade</param>
        public Trade(double value, string clientSector, DateTime nextPaymentDate)
        {
            Value = value;
            ClientSector = clientSector;
            NextPaymentDate = nextPaymentDate;
        }

        /// <summary>
        /// Indicates the transaction amount in dollars
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        /// Indicates the client´s sector which can be "Public" or "Private"
        /// </summary>
        public string ClientSector { get; set; }
        /// <summary>
        /// Indicates when the next payment from the client to the bank is expected
        /// </summary>
        public DateTime NextPaymentDate { get; set; }
    }
}
