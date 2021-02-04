using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public dynamic Get()
        {
            var res = _context.Quotes.Include(x => x.Company).ToList().GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First());
            return (from quote in res.ToList()
                select new
                {
                    QuoteId = quote.Id,
                    CompanyId = quote.Company.Id,
                    CompanyName = quote.Company.Name,
                    CompanyTicker = quote.Company.Ticker,
                    QuotePrice = quote.Price,
                    QuoteDate = quote.Date,
                }).ToList();
        }
    }
}
