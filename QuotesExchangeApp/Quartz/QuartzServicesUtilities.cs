using Quartz;
using System;

namespace QuotesExchangeApp.Quartz
{
    public static class QuartzServicesUtilities
    {
        public static void StartJob<TJob>(IScheduler scheduler, TimeSpan runInterval) where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;

            var job = JobBuilder.Create<TJob>()
            .WithIdentity(jobName)
            .Build();

            var trigger = TriggerBuilder.Create()
            .WithIdentity($"{jobName}.trigger")
            .StartNow()
            .WithSimpleSchedule(x => x.WithInterval(runInterval).RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        public static async void ChangeJobInterval<TJob>(IScheduler scheduler, TimeSpan runInterval) where TJob : IJob
        {
            var jobName = typeof(TJob).FullName;

            var oldTrigger = await scheduler.GetTrigger(new TriggerKey($"{jobName}.trigger"));
            var triggerBuilder = oldTrigger.GetTriggerBuilder();

            triggerBuilder = triggerBuilder.WithSimpleSchedule(x => x.WithInterval(runInterval).RepeatForever());

            var newTrigger = triggerBuilder.Build();
            var newSimpleTrigger = newTrigger as ISimpleTrigger;
            if (newSimpleTrigger != null && oldTrigger is ISimpleTrigger oldSimpleTrigger)
            {
                newSimpleTrigger.TimesTriggered = oldSimpleTrigger.TimesTriggered;
            }

            await scheduler.RescheduleJob(new TriggerKey($"{jobName}.trigger"), newSimpleTrigger ?? triggerBuilder.Build());
        }
    }
}
