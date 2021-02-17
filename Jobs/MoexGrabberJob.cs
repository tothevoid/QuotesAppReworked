using Microsoft.AspNetCore.SignalR;
using Quartz;
using QuotesExchangeApp.Services.Interfaces.Grabbing;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Jobs
{
    public class MoexGrabberJob : BaseGrabberJob, IJob
    {
        private readonly IMoexGrabberService _moexGrabberService;

        public MoexGrabberJob(IMoexGrabberService moexGrabberService, IHubContext<QuotesHub> context) : base(context)
        {
            _moexGrabberService = moexGrabberService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _moexGrabberService.GrabAllAsync();
            await NotifyClient();
        }
    }
}
