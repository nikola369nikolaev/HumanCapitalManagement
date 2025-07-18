using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;
using HumanCapitalManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HumanCapitalManagement.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentsController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: Departments
        [Authorize(Roles = "HR ADMIN")]
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAll();
            return View(departments);
        }

        // GET: Departments/Details/5
        [Authorize(Roles = "HR ADMIN")]
        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        [Authorize(Roles = "HR ADMIN")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR ADMIN")]
        public async Task<IActionResult> Create(CreateDepartmentInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _departmentService.CreateDepartment(input);
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Edit/5
        [Authorize(Roles = "HR ADMIN")]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            var model = new UpdateDepartmentInput
            {
                Id = department.Id,
                Name = department.Name
            };

            return View(model);
        }

        // POST: Departments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR ADMIN")]
        public async Task<IActionResult> Edit(UpdateDepartmentInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _departmentService.UpdateDepartment(input);
            return RedirectToAction(nameof(Index));
        }

        // GET: Departments/Delete/5
        [Authorize(Roles = "HR ADMIN")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HR ADMIN")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentService.DeleteDepartment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
