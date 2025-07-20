using System.Security.Claims;
using HumanCapitalManagement.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HumanCapitalManagement.Services;
using HumanCapitalManagement.Models.InputModels;
using Microsoft.AspNetCore.Authorization;

namespace HumanCapitalManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService; 
        private readonly IDepartmentService _departmentService;

        public EmployeesController(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        [Authorize(Roles = $"{nameof(RoleType.EMPLOYEE)}, {nameof(RoleType.MANAGER)}, {nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Index()
        {
            var email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var user = await _employeeService.GetByEmail(email);

            var employees = await _employeeService.GetAll();
            if (User.IsInRole($"{nameof(RoleType.MANAGER)}"))
            {
                employees = employees.Where(x => x.DepartmentId == user.DepartmentId);
            }

            if (User.IsInRole($"{nameof(RoleType.EMPLOYEE)}"))
            {
                employees = employees.Where(x => x.Email == email);
            }

            return View(employees);
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}, {nameof(RoleType.MANAGER)}, {nameof(RoleType.EMPLOYEE)}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null || !id.HasValue)
            {
                return BadRequest();
            }
            
            var email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var user = await _employeeService.GetByEmail(email);

            if (User.IsInRole($"{nameof(RoleType.EMPLOYEE)}"))
            {
                if (id != user.Id)
                {
                    return Unauthorized();
                }
            }
            
            var employee = await _employeeService.GetById(id.Value);
            
            if (employee == null)
            {
                return NotFound();
            }
            
            if (User.IsInRole($"{nameof(RoleType.MANAGER)}"))
            {
                if (employee.DepartmentId != user.DepartmentId)
                {
                    return Unauthorized();
                }
            }

            return View(employee);
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.GetAll();

            ViewData["DepartmentId"] = new SelectList(departments, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Create(CreateEmployeeInput employeeInput)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAll();

                ViewData["DepartmentId"] = new SelectList(departments, "Id", "Name", employeeInput.DepartmentId);

                return View(employeeInput);
            }

            await _employeeService.CreateEmployee(employeeInput);

            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}, {nameof(RoleType.MANAGER)}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null || !id.HasValue)
            {
                return BadRequest();
            }

            var employee = await _employeeService.GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            
            var email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var user = await _employeeService.GetByEmail(email);
            
            if (User.IsInRole($"{nameof(RoleType.MANAGER)}"))
            {
                if (employee.DepartmentId != user.DepartmentId)
                {
                    return Unauthorized();
                }
            }

            var departments = await _departmentService.GetAll();
            ViewData["DepartmentId"] = new SelectList(departments, "Id", "Name", employee.DepartmentId);

            var updateEmployeeInput = new UpdateEmployeeInput
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                JobTitle = employee.JobTitle,
                Salary = employee.Salary,
                DepartmentId = employee.DepartmentId
            };

            return View(updateEmployeeInput);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)},{nameof(RoleType.MANAGER)}")]
        public async Task<IActionResult> Edit(UpdateEmployeeInput employeeInput)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAll();
                ViewData["DepartmentId"] = new SelectList(departments, "Id", "Name", employeeInput.DepartmentId);

                return View(employeeInput);
            }
            
            var email = User.Claims.First(x => x.Type == ClaimTypes.Email).Value;
            var user = await _employeeService.GetByEmail(email);
            
            if (User.IsInRole($"{nameof(RoleType.MANAGER)}"))
            {
                if (employeeInput.DepartmentId != user.DepartmentId)
                {
                    return Unauthorized();
                }
            }

            await _employeeService.UpdateEmployee(employeeInput);

            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || !id.HasValue)
            {
                return BadRequest();
            }

            var employee =  await _employeeService.GetById(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployee(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
