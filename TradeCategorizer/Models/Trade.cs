using System;

namespace TradeCategorizer.Models
{
    /// <summary>
    /// Represents a commercial negotiation between a bank and a client.
    /// </summary>
    public class Trade
    {
        public Trade() { }

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
