using OfficeAssistant.Services.Model;

namespace OfficeAssistant.Services.Infrastructure.Repository
{
    public class InMemoryEmployeeRepository : IEmployeeRepository
    {
        private readonly IEmployees _employees;

        public InMemoryEmployeeRepository(IEmployees employees)
        {
            _employees = employees;
        }

        public int Add(Employee employee)
        {
            _employees.Add(employee);

            return _employees.Count();
        }

        public Employee ById(int id) => _employees.At(_employees.Count() - 1);

        public IEmployees All() => _employees;
    }
}