using System;

namespace QuotesExchangeApp.Models
{
    public class SupportedCompany
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public Guid SourceId { get; set; }
        public Source Source { get; set; }
    }
}
