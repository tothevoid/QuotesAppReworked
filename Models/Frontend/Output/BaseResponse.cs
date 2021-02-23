using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Models.Frontend.Output
{
    public class BaseResponse
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public BaseResponse()
        {
            IsSuccessful = true;
        }

        public BaseResponse(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}
