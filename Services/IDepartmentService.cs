using HumanCapitalManagement.Data.Models;

namespace HumanCapitalManagement.Services
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department>> GetAll();
        
        Task<Department> GetById(int id);
    }
}
