using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuotesExchangeApp.Models;


namespace QuotesExchangeApp.Data.Migrations
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Quote>().HasOne(x => x.Company);
            builder.Entity<Quote>().HasOne(x => x.Source);
            builder.Entity<Source>().HasData(
                new Source
                {
                    Name = "Finnhub",
                    ApiUrl = "https://finnhub.io/api/v1/quote?symbol="
                },
                new Source
                {
                    Name = "MOEX",
                    ApiUrl = "https://iss.moex.com/iss/engines/stock/markets/shares/securities/"
                }
                );

            builder.Entity<Company>().HasData(
                new Company
                {
                    Name = "Apple",
                    Ticker = "AAPL"
                },
                new Company
                {
                    Name = "Tesla",
                    Ticker = "TSLA"
                },
                new Company
                {
                    Name = "AMD",
                    Ticker = "AMD"
                },
                new Company
                {
                    Name = "Intel",
                    Ticker = "INTC"
                },
                new Company
                {
                    Name = "Amazon",
                    Ticker = "AMZN"
                },
                new Company
                {
                    Name = "Microsoft",
                    Ticker = "MSFT"
                },
                new Company
                {
                    Name = "Газпром",
                    Ticker = "GAZP"
                },
                new Company
                {
                    Name = "Яндекс",
                    Ticker = "YNDX"
                }
                );
        }
    }
}
