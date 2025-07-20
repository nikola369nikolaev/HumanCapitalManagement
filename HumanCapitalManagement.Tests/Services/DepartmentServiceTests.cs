using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;
using HumanCapitalManagement.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HumanCapitalManagement.Tests.Services
{
    public class DepartmentServiceTests
    {
        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=department_test.db")
                .Options;

            var dbContext = new ApplicationDbContext(options);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }

        [Fact]
        public async Task CreateDepartment_ShouldAddDepartment()
        {
            var dbContext = await GetInMemoryDbContext();
            var service = new DepartmentService(dbContext);

            var input = new CreateDepartmentInput
            {
                Name = "Finance"
            };

            await service.CreateDepartment(input);

            var department = dbContext.Departments.FirstOrDefault(d => d.Name == "Finance");
            Assert.NotNull(department);
        }

        [Fact]
        public async Task DeleteDepartment_ShouldRemoveDepartment()
        {
            var dbContext = await GetInMemoryDbContext();
            var department = new Department { Name = "Marketing" };
            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();

            var service = new DepartmentService(dbContext);
            await service.DeleteDepartment(department.Id);

            Assert.False(dbContext.Departments.Any(d => d.Id == department.Id));
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllDepartments()
        {
            var dbContext = await GetInMemoryDbContext();
            dbContext.Departments.AddRange(
                new Department { Name = "HR" },
                new Department { Name = "R&D" }
            );
            await dbContext.SaveChangesAsync();

            var service = new DepartmentService(dbContext);
            var result = await service.GetAll();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnCorrectDepartment()
        {
            var dbContext = await GetInMemoryDbContext();
            var department = new Department { Name = "IT" };
            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();

            var service = new DepartmentService(dbContext);
            var result = await service.GetById(department.Id);

            Assert.NotNull(result);
            Assert.Equal("IT", result.Name);
        }

        [Fact]
        public async Task UpdateDepartment_ShouldModifyName()
        {
            var dbContext = await GetInMemoryDbContext();
            var department = new Department { Name = "Old Name" };
            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();

            var service = new DepartmentService(dbContext);

            var input = new UpdateDepartmentInput
            {
                Id = department.Id,
                Name = "New Name"
            };

            await service.UpdateDepartment(input);

            var updated = await dbContext.Departments.FindAsync(department.Id);
            Assert.Equal("New Name", updated.Name);
        }
    }
}
