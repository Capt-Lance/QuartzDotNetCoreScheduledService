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
        private IScheduler scheduler;
        private ISchedulerFactory schedulerFactory;

        public Startup()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            configuration = builder.Build();
        }

        public async Task StartAsync()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            var container = services.BuildServiceProvider();
            var jobFactory = new JobFactory(container);
            schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();
            scheduler.JobFactory = jobFactory;
            await scheduler.Start();

            await ConfigureJobs();
        }

        public async Task StopAsync()
        {
            // noop
        }

        private async Task ConfigureJobs()
        {
            IJobDetail dataRetrievalJob = JobBuilder
                .Create<DataRetrievalJob>()
                .WithIdentity("MyDataRetrievalJob", "MyJobGroup")
                .Build();

            ITrigger dataRetrievalJobTrigger = TriggerBuilder.Create()
                .WithIdentity("MyTrigger", "MyJobGroup")
                .WithCronSchedule(configuration.GetValue<string>("myCron"))
                .Build();

            await scheduler.ScheduleJob(dataRetrievalJob, dataRetrievalJobTrigger);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine(configuration.GetValue<string>("testString"));
            services.AddSingleton<RestApiEmployeeRepository>();
            services.AddTransient<DataRetrievalJob>();
            //services.AddTransient<EFEmployeeRepository>();
        }
    }
}