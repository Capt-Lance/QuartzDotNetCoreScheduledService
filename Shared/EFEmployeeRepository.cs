using Domain.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private EmployeeContext _context;
        public EFEmployeeRepository()
        {
            _context = new EmployeeContext();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<IEnumerable<Employee>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync(IEnumerable<Employee> employees)
        {
            HashSet<int> employeeIds = employees.Select(x => x.Id).ToHashSet();
            IEnumerable<Employee> matchingEmployees = _context.Employees.Where(x => employeeIds.Contains(x.Id)).ToList();
            Dictionary<int, Employee> matchingEmployeeDict = matchingEmployees.ToDictionary(x => x.Id, x => x);
            foreach (Employee employee in employees)
            {
                if (!matchingEmployeeDict.ContainsKey(employee.Id))
                {
                    _context.Employees.Add(employee);
                }
                else
                {
                    Employee existingEmployee = matchingEmployeeDict[employee.Id];
                    _context.Entry(existingEmployee).CurrentValues.SetValues(employee);
                }
            }

            try
            {
                _context.Database.OpenConnection();
                _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Employees ON");
                await _context.SaveChangesAsync();
                _context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Employees OFF");

            }
            finally
            {
                _context.Database.CloseConnection();
            }
        }
    }
}
