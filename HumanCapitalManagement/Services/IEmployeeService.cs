using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;
using HumanCapitalManagement.Models.ViewModels;

namespace HumanCapitalManagement.Services
{
    public interface IEmployeeService
    {
        Task CreateEmployee(CreateEmployeeInput employeeInput);

        Task<IEnumerable<EmployeeViewModel>> GetAll();

        Task<EmployeeViewModel?> GetById(int id);
        
        Task<Employee?> GetByEmail(string email);

        Task UpdateEmployee(UpdateEmployeeInput employeeInput);

        Task DeleteEmployee(int id);
    }
}
