using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
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
        public IEnumerable<Company> Get()
        {
            //var entitties = _context.SupportedCompanies.ToList();
            //foreach (var ent in entitties)
            //{
            //    _context.SupportedCompanies.Remove(ent);
            //}

            //var qu = _context.Quotes.ToList();
            //foreach (var ent in qu)
            //{
            //    _context.Quotes.Remove(ent);
            //}

            //_context.SaveChanges();

            return _context.Companies.ToList();
        }
    }
}
