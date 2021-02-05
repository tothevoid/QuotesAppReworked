using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Quartz;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Jobs
{
    public class MoexGrabberJob : IJob
    {
        private readonly string moexSourceName = "MOEX";
        public List<Company> Companies { get; set; }
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<QuotesHub> _hubContext;
        private readonly CultureInfo _usCulture = new CultureInfo("en-US");

        public MoexGrabberJob(ApplicationDbContext db, IHubContext<QuotesHub> hubContext)
        {
            _context = db;
            _hubContext = hubContext;
        }

        private float GetCurrencyMultiplier()
        {
            var usdRateJson = new WebClient().DownloadString("https://www.cbr-xml-daily.ru/latest.js"); //Текущий курс рубля в разных валютах

            dynamic usd = JObject.Parse(usdRateJson);
            string usdRate = usd.rates.USD;
            float multiplier = float.Parse(usdRate, _usCulture);

            return multiplier;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            float multiplier = GetCurrencyMultiplier();
            var sourceMOEX = _context.Sources.FirstOrDefault(x => x.Name == moexSourceName);
            var moexCompanies = _context.SupportedCompanies.Include(x => x.Company).Where(x => x.Source.Name == moexSourceName).Select(x => x.Company);

            var isFirstLaunch = false;

            if (!moexCompanies.Any())
            {
                moexCompanies = _context.Companies;
                isFirstLaunch = true;
            }

            foreach (var company in moexCompanies)
            {
                var response = new WebClient().DownloadString(sourceMOEX.ApiUrl + company.Ticker + ".json");

                dynamic moex = JObject.Parse(response);
                var res = moex.marketdata.data;
                if (moex.marketdata.data.Count == 0) continue;
                string moexstring = moex.marketdata.data[2][12];
                if (moexstring == null) continue;
                float rawPrice = float.Parse(moexstring, _usCulture);
                if (rawPrice <= 0) continue;
                if (isFirstLaunch)
                {
                    _context.SupportedCompanies.Add(new SupportedCompany { Company = company, Source = sourceMOEX });
                }

                float price = rawPrice * multiplier; //Перевод из рублей в доллары

                Quote newquote = new Quote
                {
                    Company = company,
                    Price = (float)Math.Round(price, 2),
                    Date = DateTime.Now,
                    Source = sourceMOEX
                };

                _context.Quotes.Add(newquote);
                await Task.Delay(500); //Задержка между запросами
            }
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("NewQuotes", "");
        }
    }
}
