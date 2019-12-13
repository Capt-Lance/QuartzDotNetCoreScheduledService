using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Employees
{
    public class Employee
    {

        public int Id { get; private set; }
        [MaxLength(255)]
        public string Name { get; private set; }
        public int Salary { get; private set; }
        public int Age { get; private set;
        }
        public Employee(int id, string name, int salary, int age)
        {
            Id = id;
            Name = name;
            Salary = salary;
            Age = age;
        }

        public override string ToString()
        {
            return $"Id: {Id}; Name: {Name}; Salary: {Salary}; Age: {Age};";
        }
    }
}
