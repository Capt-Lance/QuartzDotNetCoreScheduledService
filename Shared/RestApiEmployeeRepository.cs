using Domain.Employees;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Infrastructure
{
    public class RestApiEmployeeRepository : IEmployeeRepository
    {
        private SemaphoreSlim _semaphoreSlim;
        private HttpClient _httpClient;
        public RestApiEmployeeRepository()
        {
            _semaphoreSlim = new SemaphoreSlim(2);
            _httpClient = new HttpClient();
        }
        private IEnumerable<Employee> ToEmployees(JArray employeesJson)
        {
            List<Employee> employees = new List<Employee>(employeesJson.Count);
            foreach(JObject employee in employeesJson.Children())
            {
                int id = employee["id"].ToObject<int>();
                string name = employee["employee_name"].ToString();
                int salary = employee["employee_salary"].ToObject<int>();
                int age = employee["employee_age"].ToObject<int>();
                employees.Add(new Employee(id, name, salary, age));
                
            }
            return employees;
        }
        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                    string url = "http://dummy.restapiexample.com/api/v1/employees";
                    var result = await _httpClient.GetAsync(url);
                    string resultString = await result.Content.ReadAsStringAsync();
                    JArray resultJson = JArray.Parse(resultString);
                    return ToEmployees(resultJson);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                _semaphoreSlim.Release();
            }

        }

        public void Dispose()
        {
            _semaphoreSlim.Dispose();
            _httpClient.Dispose();
        }

        public Task SaveAsync(IEnumerable<Employee> employees)
        {
            throw new NotImplementedException();
        }
    }
}
