using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces.Grabbing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Services.Grabbing
{
    public class MoexGrabberService : IMoexGrabberService
    {
        private readonly string moexSourceName = "MOEX";
        private readonly ApplicationDbContext _context;
        private readonly CultureInfo _usCulture = new CultureInfo("en-US");
        private readonly Source _moexSource;
        private int _grabMsInterval = 500;
        public MoexGrabberService(ApplicationDbContext context)
        {
            _context = context;
            _moexSource = _context.Sources.FirstOrDefault(x => x.Name == moexSourceName);
        }

        public async Task GrabAllAsync()
        {
            float multiplier = await GetCurrencyMultiplier();
         
            var moexCompanies = _context.SupportedCompanies
                .Include(x => x.Company).Where(x => x.Source.Name == moexSourceName)
                .Select(x => x.Company);

            var isFirstLaunch = false;

            if (!moexCompanies.Any())
            {
                moexCompanies = _context.Companies;
                isFirstLaunch = true;
            }

            foreach (var company in moexCompanies)
            {
                var quote = await GrabCompanyQuote(company, multiplier);

                if (isFirstLaunch)
                {
                    _context.SupportedCompanies.Add(new SupportedCompany { Company = company, Source = _moexSource });
                }
                _context.Add(quote);
                await Task.Delay(_grabMsInterval);
            }
            await _context.SaveChangesAsync();
        }

        private async Task<float> GetCurrencyMultiplier()
        {
            //Текущий курс рубля в разных валютах
            var usdRateJson = await new WebClient()
                   .DownloadStringTaskAsync("https://www.cbr-xml-daily.ru/latest.js");

            dynamic usd = JObject.Parse(usdRateJson);
            string usdRate = usd.rates.USD;
            float multiplier = float.Parse(usdRate, _usCulture);

            return multiplier;
        }

        public async Task<Quote> GrabCompanyQuote(Company company, double multiplier = 1)
        {
            Quote result = null;
            string url = _moexSource.ApiUrl + company.Ticker + ".json";

            var response = await new WebClient().DownloadStringTaskAsync(url);
            dynamic moex = JObject.Parse(response);
            if (moex.marketdata.data.Count == 0) return result;
            string moexstring = moex.marketdata.data[2][12];
            if (moexstring == null) return result;
            float rawPrice = float.Parse(moexstring, _usCulture);
            if (rawPrice <= 0) return result;

            return new Quote
            {
                Company = company,
                Price = (float)Math.Round(rawPrice * multiplier, 2),
                Date = DateTime.Now,
                Source = _moexSource
            };
        }
    }
}
