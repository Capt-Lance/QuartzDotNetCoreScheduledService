using Domain.Employees;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using ScheduledJobTest.Jobs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledJobTest
{
    public class SchedulingService
    {
        private ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        private async Task CreateJobs()
        {
            IJobDetail dataRetrievalJob = JobBuilder
                .Create<DataRetrievalJob>()
                .WithIdentity("job1", "group1")
                .Build();

            ITrigger dataRetrievalJobTrigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithCronSchedule("0/1 * * ? * * *")
                .Build();

            await _scheduler.ScheduleJob(dataRetrievalJob, dataRetrievalJobTrigger);
        }

        public async Task StartAsync()
        {
            Console.WriteLine("hi");
            _schedulerFactory = new StdSchedulerFactory();
            _scheduler = await _schedulerFactory.GetScheduler();
            var services = new ServiceCollection();
            services.AddSingleton<RestApiEmployeeRepository>();
            services.AddTransient<DataRetrievalJob>();
            //services.AddTransient<EFEmployeeRepository>();
            var container = services.BuildServiceProvider();
            var jobFactory = new JobFactory(container);
            _scheduler.JobFactory = jobFactory;
            await _scheduler.Start();

            await CreateJobs();

        }

        public void StopAsync()
        {
            // noop
        }
    }
}
