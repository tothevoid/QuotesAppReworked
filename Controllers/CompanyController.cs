using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Services.Interfaces.Grabbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<QuotesController> _logger;
        private readonly ApplicationDbContext _context;
   
        public CompanyController(ILogger<QuotesController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _context = db;
        }

        [HttpGet]
        public async Task<IEnumerable<Company>> Get() => await _context.Companies.ToListAsync();

        [HttpPost]
        public async Task<Company> Post(Quote quote)
        {
            if (quote.Company != null && quote.Source != null)
            {
                var company = await _context.Companies.AddAsync(quote.Company);
                await _context.SupportedCompanies
                       .AddAsync(new SupportedCompany { CompanyId = quote.Company.Id, SourceId = quote.Source.Id });
                await _context.SaveChangesAsync();
                return company.Entity;
            }
            return null;
        }
    }

}
