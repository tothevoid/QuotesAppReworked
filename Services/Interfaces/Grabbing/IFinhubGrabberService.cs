﻿using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Services.Interfaces.Grabbing
{
    public interface IFinhubGrabberService
    {
        Task GrabAllAsync();

        Task<Quote> GrabCompanyQuote(Company company);
    }
}
