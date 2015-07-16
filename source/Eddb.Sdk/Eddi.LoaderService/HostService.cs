using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eddi.LoaderService.Jobs.Eddb;
using Quartz;

namespace Eddi.LoaderService
{
    public class HostService
    {
        private readonly IScheduler _Scheduler;

        protected IScheduler Scheduler => _Scheduler;

        public HostService(IScheduler scheduler)
        {
            _Scheduler = scheduler;
        }

        public bool Start()
        {
            try
            {
                Scheduler
                    .ScheduleJob(
                        JobBuilder.Create<EddbLoadJob>().Build(),
                        TriggerBuilder.Create()
                            .WithSimpleSchedule(schedule =>
                                schedule.WithIntervalInHours(24))
                            .StartNow()
                            .Build());

                ////TODO: Log

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
            //Scheduler.Shutdown(true);

            return true;
        }
    }
}

