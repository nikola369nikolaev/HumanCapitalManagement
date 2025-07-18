using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;
using HumanCapitalManagement.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HumanCapitalManagement.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeService(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
            
            var result = await _userManager.CreateAsync(new IdentityUser
            {
                UserName = employeeInput.Email,
                NormalizedUserName = employeeInput.Email,
                Email = employeeInput.Email,
                NormalizedEmail = employeeInput.Email,
                EmailConfirmed = true
            }, "Employee123!");
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(employeeInput.Email);
                await _userManager.AddToRoleAsync(user, "EMPLOYEE");
            }
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
            
            var user = await _userManager.FindByEmailAsync(employee.Email);
            await _userManager.DeleteAsync(user);
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAll()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();

            var employeesModel = employees.Select(async x =>
            {
                var user = await _userManager.FindByEmailAsync(x.Email);
                var roles = await _userManager.GetRolesAsync(user);
                return new EmployeeViewModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    JobTitle = x.JobTitle,
                    Salary = x.Salary,
                    DepartmentName = x.Department.Name,
                    DepartmentId = x.DepartmentId,
                    Role = string.Join(", ", roles)
                };
            }).Select(x => x.Result);
            
            return employeesModel;
        }

        public async Task<Employee?> GetById(int id)
        {
            var employee = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(m => m.Id == id);

            return employee;
        }

        public async Task<Employee?> GetByEmail(string email)
        {
            var employee = await _context.Employees.Include(e => e.Department).FirstOrDefaultAsync(m => m.Email == email);

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
