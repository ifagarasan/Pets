using NUnit.Framework;
using OfficeAssistant.Services.Model;

namespace OfficeAssistant.Services.UnitTests.Model
{
    [TestFixture]
    public class EmployeesShould
    {
        [TestCase]
        public void AddEmployee()
        {
            var employees = new Employees();
            var employee = new Employee();

            employees.Add(employee);

            Assert.AreEqual(1, employees.Count());
            Assert.AreEqual(employee, employees.At(0));
        }
    }
}
