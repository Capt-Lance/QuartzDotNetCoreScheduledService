using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Employees
{
    public interface IEmployeeRepository : IDisposable
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task SaveAsync(IEnumerable<Employee> employees);
    }
}
