using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models
{
    public class Result
    {
        public Guid QuoteId { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyTicker { get; set; }
        public float QuotePrice { get; set; }
        public DateTime QuoteDate { get; set; }
    }
}
