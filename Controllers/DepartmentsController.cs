using HumanCapitalManagement.Data;
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
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAll();
            return View(departments);
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Details(int id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Create(CreateDepartmentInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _departmentService.CreateDepartment(input);
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
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
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Edit(UpdateDepartmentInput input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            await _departmentService.UpdateDepartment(input);
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetById(id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{nameof(RoleType.HR_ADMIN)}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentService.DeleteDepartment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
