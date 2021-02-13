using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Models.Frontend;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuotesExchangeApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChartController : ControllerBase
    {
        private readonly ILogger<QuotesController> _logger;
        private readonly ApplicationDbContext _context;

        public ChartController(ILogger<QuotesController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _context = db;
        }

        [HttpPost]
        public List<ChartPoint> Post(QuoteCriteria criteria)
        {
            var company = _context.Companies.FirstOrDefault(x => x.Id == criteria.CompanyId);
            return _context.Quotes.Include(x => x.Company)
                .Where(x => x.Company.Id == company.Id && x.Date > DateTime.Now.AddMinutes(-criteria.Mins)).ToList()
                .OrderBy(x => x.Date)
                .Select(x => new ChartPoint {
                    Price = x.Price,
                    Date = x.Date
                }).ToList();
        }
    }
}
