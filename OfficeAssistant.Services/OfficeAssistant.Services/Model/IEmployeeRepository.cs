namespace OfficeAssistant.Services.Model
{
    public interface IEmployeeRepository
    {
        int Add(Employee employee);
        Employee ById(int id);
        IEmployees All();
    }
}