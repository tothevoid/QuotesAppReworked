using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models.Frontend.Output
{
    public class GetByCompanyResponse: BaseResponse
    {
        public GetByCompanyResponse(string errorMessage): base(errorMessage) {}

        public GetByCompanyResponse(IEnumerable<Quote> quotes): base()
        {
            Quotes = quotes;
        }

        public IEnumerable<Quote> Quotes { get; set; }
    }
}
