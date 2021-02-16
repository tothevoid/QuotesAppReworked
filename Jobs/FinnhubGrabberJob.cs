using Microsoft.AspNetCore.SignalR;
using Quartz;
using QuotesExchangeApp.Services.Interfaces.Grabbing;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Jobs
{
    public class FinnhubGrabberJob : IJob
    {
        private readonly IHubContext<QuotesHub> _hubContext;
        private readonly IFinhubGrabberService _grabberService;

        public FinnhubGrabberJob(IHubContext<QuotesHub> hubContext, IFinhubGrabberService grabberService)
        {
            _hubContext = hubContext;
            _grabberService = grabberService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _grabberService.GrabAllAsync();
            await _hubContext.Clients.All.SendAsync("NewQuotes", "");
        }
    }
}
