using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace QuotesExchangeApp.Quartz
{
    public class JobFactory : IJobFactory
    {
        protected readonly IServiceProvider Container;
        public JobFactory(IServiceProvider container)
        {
            Container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scope = Container.CreateScope();
            var service = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            return service;
        }

        public void ReturnJob(IJob job)
        {
            if (job is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
