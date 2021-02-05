using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models.Frontend;
using System.Collections.Generic;
using System.Linq;

namespace QuotesExchangeApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly ILogger<QuotesController> _logger;
        private readonly ApplicationDbContext _context;

        public QuotesController(ILogger<QuotesController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _context = db;
        }

        [HttpGet]
        public List<ChartCompany> Get()
        {
            var res = _context.Quotes.Include(x => x.Company).ToList().GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First());
            return (from quote in res.ToList()
                    select new ChartCompany
                    {
                        Id = quote.Company.Id,
                        Name = quote.Company.Name,
                        Ticker = quote.Company.Ticker,
                        LastQuoteValue = quote.Price,
                        LastQuoteDate = quote.Date,
                    }).ToList();
        }
    }
}
