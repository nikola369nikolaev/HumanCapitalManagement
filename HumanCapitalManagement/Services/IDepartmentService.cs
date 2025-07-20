using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;

namespace HumanCapitalManagement.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAll();
        
        Task<Department> GetById(int id);
        
        Task CreateDepartment(CreateDepartmentInput input);
        Task UpdateDepartment(UpdateDepartmentInput input);
        Task DeleteDepartment(int id);
    }
}
