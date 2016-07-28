using System.Collections;
using System.Collections.Generic;

namespace OfficeAssistant.Services.Model
{
    public interface IEmployees
    {
        void Add(Employee employee);
        Employee At(int index);
        int Count();
    }
}