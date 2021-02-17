using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Options;
using QuotesExchangeApp.Services.Interfaces.Grabbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Services.Grabbing
{
    public class FinhubGrabberService : IFinhubGrabberService
    {
        private readonly Source _finhubSource;
        private readonly string _finnhubSourceName = "Finnhub";
        private readonly ApplicationDbContext _context;
        private readonly FinhubOptions _options;
        private int _grabMsInterval = 500;

        public FinhubGrabberService(ApplicationDbContext context, IOptions<FinhubOptions> options)
        {
            _context = context;
            _options = options.Value;
            _finhubSource = _context.Sources.FirstOrDefault(x => x.Name == _finnhubSourceName);
        }

        public async Task GrabAllAsync()
        {
            var finnhubCompanies = _context.SupportedCompanies.Include(x => x.Company)
                .Where(x => x.Source.Name == _finnhubSourceName).Select(x => x.Company);
            var isFirstLaunch = false;

            if (!finnhubCompanies.Any())
            {
                isFirstLaunch = true;
                finnhubCompanies = _context.Companies;
            }

            foreach (var company in finnhubCompanies)
            {
                var quote = GrabCompanyQuote(company);
                if (isFirstLaunch)
                {
                    _context.SupportedCompanies.Add(new SupportedCompany { Company = company, Source = _finhubSource });
                }
                _context.Quotes.Add(quote);
                await Task.Delay(_grabMsInterval);
            }
            await _context.SaveChangesAsync();
        }

        public Quote GrabCompanyQuote(Company company)
        {
            string response = new WebClient().DownloadString($"{_finhubSource.ApiUrl}{company.Ticker}&token={_options.Token}");
            string rawPrice = JObject.Parse(response).SelectToken("c").ToString();
            var price = float.Parse(rawPrice);
            if (price <= 0) return null;

            return new Quote
            {
                Company = company,
                Price = price,
                Date = DateTime.Now,
                Source = _finhubSource
            };
        }
    }
}
