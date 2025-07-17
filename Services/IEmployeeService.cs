using HumanCapitalManagement.Models.InputModels;

using DataEmployee = HumanCapitalManagement.Data.Models.Employee;

namespace HumanCapitalManagement.Services
{
    public interface IEmployeeService
    {
        Task CreateEmployee(CreateEmployeeInput employeeInput);

        Task<IEnumerable<DataEmployee>> GetAll();

        Task<DataEmployee?> GetById(int id);

        Task UpdateEmployee(UpdateEmployeeInput employeeInput);

        Task DeleteEmployee(int id);
    }
}
