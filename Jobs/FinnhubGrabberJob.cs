using Microsoft.AspNetCore.SignalR;
using Quartz;
using QuotesExchangeApp.Services.Interfaces.Grabbing;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Jobs
{
    public class FinnhubGrabberJob : BaseGrabberJob, IJob
    {

        private readonly IFinhubGrabberService _grabberService;

        public FinnhubGrabberJob(IFinhubGrabberService grabberService, IHubContext<QuotesHub> context) : base(context)
        {
            _grabberService = grabberService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _grabberService.GrabAllAsync();
            await NotifyClient();
        }
    }
}
