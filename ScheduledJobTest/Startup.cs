using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using ScheduledJobTest.Jobs;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScheduledJobTest
{
    public class Startup
    {
        private readonly IConfigurationRoot configuration;
        private IScheduler _scheduler;
        private ISchedulerFactory _schedulerFactory;
        public Startup()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
        }

        public async Task StartAsync()
        {
            Console.WriteLine("hi");

            var services = new ServiceCollection();
            ConfigureServices(services);
            var container = services.BuildServiceProvider();
            var jobFactory = new JobFactory(container);
            _schedulerFactory = new StdSchedulerFactory();
            _scheduler = await _schedulerFactory.GetScheduler();
            _scheduler.JobFactory = jobFactory;
            await _scheduler.Start();

            await CreateJobs();
        }

        public void StopAsync()
        {
            // noop
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine(configuration.GetValue<string>("testString"));
            services.AddSingleton<RestApiEmployeeRepository>();
            services.AddTransient<DataRetrievalJob>();
            //services.AddTransient<EFEmployeeRepository>();
        }

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
    }
}