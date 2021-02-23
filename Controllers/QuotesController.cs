using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QuotesExchangeApp.Data.Migrations;
using QuotesExchangeApp.Models;
using QuotesExchangeApp.Models.Frontend;
using QuotesExchangeApp.Models.Frontend.Output;
using QuotesExchangeApp.Services.Interfaces.Grabbing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class QuotesController : ControllerBase
    {
        private readonly ILogger<QuotesController> _logger;
        private readonly ApplicationDbContext _context;

        private readonly IMoexGrabberService _moexGrabberService;
        private readonly IFinhubGrabberService _finhubGrabberService;

        public QuotesController(ILogger<QuotesController> logger, ApplicationDbContext db,
            IMoexGrabberService moexGrabberService, IFinhubGrabberService finhubGrabberService)
        {
            _logger = logger;
            _context = db;
            _moexGrabberService = moexGrabberService;
            _finhubGrabberService = finhubGrabberService;
        }

        [HttpGet]
        public List<ChartCompany> Get()
        {
            var res = _context.Quotes.Include(x => x.Company).ToList()
                .GroupBy(x => x.Company.Id, (key, g) => g.OrderByDescending(e => e.Date).First());
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

        [HttpPost]
        public async Task<GetByCompanyResponse> GetByCompany(Company company)
        {
            var existingCompany = _context.Companies.FirstOrDefault(cmp => 
                    cmp.Name.ToLower().Trim() == company.Name.ToLower().Trim() ||
                    cmp.Ticker.ToLower().Trim() == company.Ticker.ToLower().Trim());

            if (existingCompany != null)
            {
                string exceptionMessage = (company.Name == existingCompany.Name) ?
                    $"Company with name '{company.Name}'" :
                    $"Comapny with ticker '{company.Ticker}'";
                return new GetByCompanyResponse($"{exceptionMessage} already exitst");
            }
            else
            {
                var quotes = await Task.WhenAll(_moexGrabberService.GrabCompanyQuote(company),
                    _finhubGrabberService.GrabCompanyQuote(company));
                return (quotes.Any(quote => quote != null)) ?
                    new GetByCompanyResponse(quotes) :
                    new GetByCompanyResponse($"Ticker '{company.Ticker}' not found");
            }
        }
    }
}
