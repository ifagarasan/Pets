namespace OfficeAssistant.Services.Model
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public void Add(Employee employee) => _repository.Add(employee);

        public IEmployees All() => _repository.All();
    }
}