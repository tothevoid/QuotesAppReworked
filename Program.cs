using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using Microsoft.Extensions.DependencyInjection;
using QuotesExchangeApp.Jobs;
using QuotesExchangeApp.Quartz;
using System.Globalization;

namespace QuotesExchangeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                ScheduleJobs(services);
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void ScheduleJobs(IServiceProvider services)
        {
            var scheduler = services.GetService<IScheduler>();
            var grabDelay = TimeSpan.FromMinutes(1);
            QuartzServicesUtilities.StartJob<FinnhubGrabberJob>(scheduler, grabDelay);
            QuartzServicesUtilities.StartJob<MoexGrabberJob>(scheduler, grabDelay);
        }
    }
}
