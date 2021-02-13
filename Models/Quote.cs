using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models
{
    public class Quote
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Company Company { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public Source Source { get; set; }
    }
}
