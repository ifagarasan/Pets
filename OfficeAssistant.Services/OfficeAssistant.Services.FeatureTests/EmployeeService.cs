using NUnit.Framework;
using OfficeAssistant.Services.Model;
using OfficeAssistant.Services.Infrastructure.Repository;

namespace OfficeAssistant.Services.FeatureTests
{
    [TestFixture]
    public class EmployeeService
    {
        [TestCase]
        public void StoresEmployees()
        {
            var employee = new Employee();
            var employeeService = new Model.EmployeeService(
                new InMemoryEmployeeRepository(new Employees()));

            employeeService.Add(employee);

            var employees = employeeService.All();
            Assert.AreEqual(1, employees.Count());
            Assert.AreEqual(employee, employees.At(0));
        }
    }
}
