using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using HumanCapitalManagement.Models.InputModels;
using HumanCapitalManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace HumanCapitalManagement.Tests.Services
{
    public class EmployeeServiceTests
    {
        private Mock<IConfiguration> _mockConfig;
        private Mock<AesEncryptionService> _mockAesEncryptionService;

        public EmployeeServiceTests()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(cfg => cfg["EncryptionKey"]).Returns("12345678901234567890123456789012");
            _mockConfig.Setup(cfg => cfg["EncryptionIV"]).Returns("abcdefghijklmnop");
            _mockAesEncryptionService = new Mock<AesEncryptionService>(_mockConfig.Object);
        }

        private async Task<ApplicationDbContext> GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite("Data Source=database.db")
                .Options;

            var dbContext = new ApplicationDbContext(options);
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();
            return dbContext;
        }

        [Fact]
        public async Task CreateEmployee_ShouldCreateEmployeeAndUserWithRole()
        {
            var dbContext = await GetInMemoryDbContext();
            dbContext.Departments.Add(new Department { Id = 1, Name = "IT" });
            dbContext.Countries.Add(new Country { Id = 1, Name = "USA", Code = "US" });
            await dbContext.SaveChangesAsync();

            var mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            var employeeInput = new CreateEmployeeInput
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "ivan@example.com",
                JobTitle = "Developer",
                Salary = 1000,
                DepartmentId = 1,
                CountryId = 1,
                IBAN = ""
            };

            mockUserManager
                .Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mockUserManager
                .Setup(x => x.FindByEmailAsync(employeeInput.Email))
                .ReturnsAsync(new IdentityUser { Email = employeeInput.Email });

            mockUserManager
                .Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new EmployeeService(dbContext, mockUserManager.Object, _mockConfig.Object,
                _mockAesEncryptionService.Object);

            await service.CreateEmployee(employeeInput);

            var employee = dbContext.Employees.FirstOrDefault(e => e.Email == employeeInput.Email);
            Assert.NotNull(employee);
            Assert.Equal("Ivan", employee.FirstName);
            mockUserManager.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), "Employee123!"), Times.Once);
            mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "EMPLOYEE"), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployee_ShouldRemoveEmployeeAndUser()
        {
            var dbContext = await GetInMemoryDbContext();

            var employee = new Employee
            {
                FirstName = "Maria",
                LastName = "Petrova",
                Email = "maria@example.com",
                JobTitle = "Manager",
                Salary = 2000,
                DepartmentId = 1,
                CountryId = 1,
                IBAN = ""
            };
            dbContext.Employees.Add(employee);
            dbContext.Departments.Add(new Department { Id = 1, Name = "IT" });
            dbContext.Countries.Add(new Country { Id = 1, Name = "USA", Code = "US" });
            await dbContext.SaveChangesAsync();

            var mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            mockUserManager
                .Setup(x => x.FindByEmailAsync(employee.Email))
                .ReturnsAsync(new IdentityUser { Email = employee.Email });

            mockUserManager
                .Setup(x => x.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            var service = new EmployeeService(dbContext, mockUserManager.Object, _mockConfig.Object,
                _mockAesEncryptionService.Object);

            await service.DeleteEmployee(employee.Id);

            Assert.False(dbContext.Employees.Any(e => e.Id == employee.Id));
            mockUserManager.Verify(x => x.DeleteAsync(It.IsAny<IdentityUser>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllEmployees()
        {
            var dbContext = await GetInMemoryDbContext();

            dbContext.Employees.AddRange(
                new Employee
                {
                    FirstName = "A", LastName = "A", Email = "a@example.com", DepartmentId = 1,
                    JobTitle = "Data Analyst", Salary = 1000, CountryId = 1, IBAN = ""
                },
                new Employee
                {
                    FirstName = "B", LastName = "B", Email = "b@example.com", DepartmentId = 1, JobTitle = "Programmer",
                    Salary = 1000, CountryId = 1, IBAN = ""
                }
            );
            dbContext.Departments.Add(new Department { Id = 1, Name = "IT" });
            dbContext.Countries.Add(new Country { Id = 1, Name = "USA", Code = "US" });
            await dbContext.SaveChangesAsync();

            var mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            mockUserManager
                .Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());

            mockUserManager
                .Setup(x => x.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(new List<string> { $"{nameof(RoleType.EMPLOYEE)}" });

            var mockRoleManager = new Mock<RoleManager<IdentityRole>>(
                Mock.Of<IRoleStore<IdentityRole>>(), null, null, null, null);

            var service = new EmployeeService(dbContext, mockUserManager.Object, _mockConfig.Object,
                _mockAesEncryptionService.Object);

            var result = await service.GetAll();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnEmployee_WhenExists()
        {
            var dbContext = await GetInMemoryDbContext();
            var employee = new Employee
            {
                FirstName = "Test", LastName = "User", Email = "test@a.com", DepartmentId = 1, JobTitle = "HR",
                IBAN = "", CountryId = 1
            };
            dbContext.Employees.Add(employee);
            dbContext.Departments.Add(new Department { Id = 1, Name = "IT" });
            dbContext.Countries.Add(new Country { Id = 1, Name = "USA", Code = "US" });
            await dbContext.SaveChangesAsync();

            var service = new EmployeeService(dbContext, null!, _mockConfig.Object, _mockAesEncryptionService.Object);
            var result = await service.GetById(employee.Id);

            Assert.NotNull(result);
            Assert.Equal("test@a.com", result.Email);
        }


        [Fact]
        public async Task GetByEmail_ShouldReturnNull_WhenNotFound()
        {
            var dbContext = await GetInMemoryDbContext();
            var service = new EmployeeService(dbContext, null!, _mockConfig.Object, _mockAesEncryptionService.Object);
            var result = await service.GetByEmail("nonexistent@example.com");

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateEmployee_ShouldUpdateFields()
        {
            var dbContext = await GetInMemoryDbContext();

            var employee = new Employee
            {
                FirstName = "Old",
                LastName = "Name",
                Email = "old@example.com",
                DepartmentId = 1,
                JobTitle = "Dev",
                Salary = 1000,
                CountryId = 1,
                IBAN = ""
            };
            dbContext.Employees.Add(employee);
            dbContext.Departments.Add(new Department { Id = 1, Name = "IT" });
            dbContext.Countries.Add(new Country { Id = 1, Name = "USA", Code = "US" });
            await dbContext.SaveChangesAsync();

            var service = new EmployeeService(dbContext, null!, _mockConfig.Object, _mockAesEncryptionService.Object);

            var input = new UpdateEmployeeInput
            {
                Id = employee.Id,
                FirstName = "New",
                LastName = "Name",
                Email = "new@example.com",
                JobTitle = "Team Lead",
                Salary = 1500,
                DepartmentId = 1,
                CountryId = 1,
                IBAN = ""
            };

            await service.UpdateEmployee(input);

            var updated = await dbContext.Employees.FindAsync(employee.Id);
            Assert.Equal("New", updated.FirstName);
            Assert.Equal("new@example.com", updated.Email);
        }
    }
}