using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuotesExchangeApp.Jobs
{
    public class BaseGrabberJob
    {
        protected readonly IHubContext<QuotesHub> _hubContext;
        public BaseGrabberJob(IHubContext<QuotesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        protected virtual async Task NotifyClient()
        {
            await _hubContext.Clients.All.SendAsync("NewQuotes", "");
        }
       
    }
}
