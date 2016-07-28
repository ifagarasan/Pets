using NSubstitute;
using NUnit.Framework;
using OfficeAssistant.Services.Model;

namespace OfficeAssistant.Services.UnitTests.Model
{
    [TestFixture]
    public class EmployeeServiceShould
    {
        private IEmployeeRepository _repository;
        private EmployeeService _service;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<IEmployeeRepository>();
            _service = new EmployeeService(_repository);
        }

        [TestCase]
        public void AddEmployee()
        {            
            var employee = new Employee();

            _service.Add(employee);

            _repository.Received().Add(employee);
        }

        [TestCase]
        public void RetrieveAllEmployees()
        {
            _service.All();

            _repository.Received().All();
        }
    }
}
