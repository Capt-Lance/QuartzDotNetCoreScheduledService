using Domain.Employees;
using Domain.Services;
using Infrastructure;
using Quartz;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ScheduledJobTest.Jobs
{
    [DisallowConcurrentExecution]
    public class DataRetrievalJob : IJob, IDisposable
    {
        private IEmployeeRepository _employeeRepository;
        private IEmployeeRepository _employeePersistenceRepository;
        public DataRetrievalJob(RestApiEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _employeePersistenceRepository = new EFEmployeeRepository();
        }

        public void Dispose()
        {
            _employeePersistenceRepository.Dispose();
            _employeePersistenceRepository = null;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Hello from the DataRetrievalJob");
            try
            {
                EmployeeService employeeService = new EmployeeService(_employeeRepository, _employeePersistenceRepository);
                await employeeService.PrintEmployees();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
