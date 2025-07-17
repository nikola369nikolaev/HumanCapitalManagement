using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using HumanCapitalManagement.Services;
using HumanCapitalManagement.Models.InputModels;

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

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeService.GetAll();

            return View(employees);
        }

        public async Task<IActionResult> Details(int? id)
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

            return View(employee);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.GetAll();

            ViewData["DepartmentId"] = new SelectList(departments, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Edit(UpdateEmployeeInput employeeInput)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAll();
                ViewData["DepartmentId"] = new SelectList(departments, "Id", "Name", employeeInput.DepartmentId);

                return View(employeeInput);
            }

            await _employeeService.UpdateEmployee(employeeInput);

            return RedirectToAction(nameof(Index));
        }

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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployee(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
