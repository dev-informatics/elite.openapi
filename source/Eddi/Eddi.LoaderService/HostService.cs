﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Eddi.LoaderService.Jobs;
using Eddi.LoaderService.ModelMaps.Eddi;
using Eddi.LoaderService.Providers;
using Quartz;

namespace Eddi.LoaderService
{
    public class HostService
    {
        protected IScheduler Scheduler { get; }

        protected CancellationTokenSource EddnListenerCancellationSource { get; set; }

        public HostService(IScheduler scheduler, CancellationTokenSourceProvider cancellationSourceProvider)
        {
            Scheduler = scheduler;

            EddnListenerCancellationSource = cancellationSourceProvider
                .RetrieveOrCreate<EddnListenerJob>();
        }

        public bool Start()
        {
            try
            {
                IDataStoreModelMap SystemV1Map = new SystemV1Map();

                SystemV1Map.Register();

                //ScheduleEddbLoad();

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
            EddnListenerCancellationSource.Cancel();

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
