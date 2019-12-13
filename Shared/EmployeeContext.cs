using Domain.Employees;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class EmployeeContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=AlephInspiron2\SQLEXPRESS;Initial Catalog=Employee;Integrated Security=True");
            }
        }
        public virtual DbSet<Employee> Employees { get; set; }
    }
}
