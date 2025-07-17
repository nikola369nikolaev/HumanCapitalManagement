using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;
using Microsoft.EntityFrameworkCore;

namespace HumanCapitalManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateEmployee(CreateEmployeeInput employeeInput)
        {
            var employee = new Employee
            {
                FirstName = employeeInput.FirstName,
                LastName = employeeInput.LastName,
                Email = employeeInput.Email,
                JobTitle = employeeInput.JobTitle,
                Salary = employeeInput.Salary,
                DepartmentId = employeeInput.DepartmentId
            };

            _context.Add(employee);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee == null)
            {
                throw new ArgumentNullException($"Employee with ID {id} not found.");
            }

            _context.Employees.Remove(employee);

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> GetAll()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();

            return employees;
        }

        public async Task<Employee?> GetById(int id)
        {
            var employee = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(m => m.Id == id);

            return employee;
        }

        public async Task UpdateEmployee(UpdateEmployeeInput employeeInput)
        {
            var employee = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(m => m.Id == employeeInput.Id);

            if (employee == null)
            {
                throw new ArgumentNullException($"Employee with ID {employeeInput.Id} not found.");
            }

            employee.FirstName = employeeInput.FirstName;
            employee.LastName = employeeInput.LastName;
            employee.Email = employeeInput.Email;
            employee.JobTitle = employeeInput.JobTitle;
            employee.Salary = employeeInput.Salary;
            employee.DepartmentId = employeeInput.DepartmentId;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
