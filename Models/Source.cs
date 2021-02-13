using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models
{
    public class Source
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string ApiUrl { get; set; }
    }
}
