using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models.Frontend
{
    public class ChartCompany
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public string Ticker { get; set; }

        public float LastQuoteValue { get; set; }

        public DateTime LastQuoteDate { get; set; }
    }
}
