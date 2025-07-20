using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;
using Microsoft.EntityFrameworkCore;

namespace HumanCapitalManagement.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAll()
        {
            var result = await _context.Departments.ToListAsync();

            return result;
        }

        public async Task<Department> GetById(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                throw new ArgumentNullException($"Department with ID {id} not found.");
            }

            return department;
        }
        
        public async Task CreateDepartment(CreateDepartmentInput input)
        {
            var department = new Department { Name = input.Name };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDepartment(UpdateDepartmentInput input)
        {
            var department = await _context.Departments.FindAsync(input.Id);
            if (department == null)
            {
                throw new ArgumentNullException($"Department with ID {input.Id} not found.");
            }

            department.Name = input.Name;
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDepartment(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                throw new ArgumentNullException($"Department with ID {id} not found.");
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }
    }
}
