using System;

namespace QuotesExchangeApp.Models
{
    public class SupportedCompany
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Company Company { get; set; }
        public Source Source { get; set; }
    }
}
