using NSubstitute;
using NUnit.Framework;
using OfficeAssistant.Services.Infrastructure.Repository;
using OfficeAssistant.Services.Model;

namespace OfficeAssistant.Services.UnitTests.Infrastructure
{
    [TestFixture]
    public class InMemoryEmployeeRepositoryShould
    {
        private IEmployees _employees;
        private InMemoryEmployeeRepository _repository;

        [SetUp]
        public void Setup()
        {
            _employees = Substitute.For<IEmployees>();
            _repository = new InMemoryEmployeeRepository(_employees);
        }

        [TestCase]
        public void StoreEmployees()
        {            
            var employee = new Employee();

            _repository.Add(employee);

            _employees.Received().Add(employee);
        }

        [TestCase]
        public void RetrieveAllEmployees()
        {
            Assert.That(_repository.All(), Is.EqualTo(_employees));
        }
    }
}
