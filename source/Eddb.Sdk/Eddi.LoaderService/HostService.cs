using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Eddi.LoaderService.Jobs;
using Eddi.LoaderService.Providers;
using Quartz;

namespace Eddi.LoaderService
{
    public class HostService
    {
        protected IScheduler Scheduler { get; }

        protected CancellationTokenSource EddnListernerCancellationSource { get; set; }

        public HostService(IScheduler scheduler, CancellationTokenSourceProvider cancellationSourceProvider)
        {
            Scheduler = scheduler;

            EddnListernerCancellationSource = cancellationSourceProvider
                .RetrieveOrCreate<EddnListenerJob>();
        }

        public bool Start()
        {
            try
            {
                ScheduleEddbLoad();

                ScheduleEddnListener();

                //TODO: Log

                Scheduler.Start();

                //TODO: Log

                return true;
            }
            catch (Exception)
            {
                //TODO: Log
                return false;
            }
        }

        public bool Stop()
        {
            //TODO: Log
            EddnListernerCancellationSource.Cancel();

            Scheduler.Shutdown(true);

            return true;
        }

        protected void ScheduleEddbLoad()
        {
            Scheduler.ScheduleJob(
                JobBuilder.Create<EddbLoadJob>().Build(),
                TriggerBuilder.Create()
                    .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInHours(24))
                    //Not sure if this is necessary or if the StartAt method is smart enough to move to the next day.
                    .StartAt(DateTime.Now.Hour < 1
                        ? DateBuilder.TodayAt(1, 0, 0)
                        : DateBuilder.TomorrowAt(1, 0, 0))
                    .Build());
        }

        protected void ScheduleEddnListener()
        {
            Scheduler.ScheduleJob(
                JobBuilder.Create<EddnListenerJob>().Build(),
                TriggerBuilder.Create()
                    .StartNow()
                    .Build());
        }
    }
}

