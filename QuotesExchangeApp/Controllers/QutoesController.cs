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
    [Route("[controller]")]
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
        public IEnumerable<Quote> Get()
        {
            return _context.Quotes.ToList();
        }
    }
}
