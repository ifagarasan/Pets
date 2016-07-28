using System.Collections.Generic;

namespace OfficeAssistant.Services.Model
{
    public class Employees : IEmployees
    {
        private readonly List<Employee> _employees;

        public Employees()
        {
            _employees = new List<Employee>();
        }

        public int Count() => _employees.Count;

        public Employee At(int index) => _employees[index];

        public void Add(Employee employee) => _employees.Add(employee);
    }
}