using Domain.Employees;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class EmployeeService
    {
        private IEmployeeRepository _employeeFetchRepo;
        private IEmployeeRepository _employeePersistenceRepo;

        public EmployeeService(IEmployeeRepository employeeFetchRepository, IEmployeeRepository employeePersistenceRepo)
        {
            _employeeFetchRepo = employeeFetchRepository;
            _employeePersistenceRepo = employeePersistenceRepo;
        }
        public async Task PrintEmployees()
        {
            try
            {
                IEnumerable<Employee> fetchedEmployees = await _employeeFetchRepo.GetAllAsync();
                foreach (Employee fetchedEmployee in fetchedEmployees)
                {
                    Console.WriteLine(fetchedEmployee.ToString());

                }
                //await _employeePersistenceRepo.SaveAsync(fetchedEmployees);
            }
            catch(Exception ex)
            {
                //noop
            }

        }
    }
}
