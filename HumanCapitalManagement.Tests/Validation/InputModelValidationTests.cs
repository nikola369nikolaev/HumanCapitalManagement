using HumanCapitalManagement.Models.InputModels;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace HumanCapitalManagement.Tests.Validation
{
    public class InputModelValidationTests
    {
        [Fact]
        public void CreateEmployeeInput_ShouldFailValidation_WhenRequiredFieldsMissing()
        {
            var input = new CreateEmployeeInput();

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(input, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateEmployeeInput.FirstName)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateEmployeeInput.LastName)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateEmployeeInput.Email)));
        }

        [Fact]
        public void CreateEmployeeInput_ShouldFail_WhenEmailIsInvalid()
        {
            var input = new CreateEmployeeInput
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "invalid-email",
                JobTitle = "Dev",
                Salary = 1000,
                DepartmentId = 1
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(input, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(CreateEmployeeInput.Email)));
        }

        [Fact]
        public void CreateEmployeeInput_ShouldPass_WhenValid()
        {
            var input = new CreateEmployeeInput
            {
                FirstName = "Maria",
                LastName = "Petrova",
                Email = "maria@example.com",
                JobTitle = "QA",
                Salary = 1500,
                DepartmentId = 2
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(input, context, results, true);

            Assert.True(isValid);
        }


        [Fact]
        public void UpdateEmployeeInput_ShouldFailValidation_WhenRequiredFieldsMissing()
        {
            var input = new UpdateEmployeeInput(); 

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(input, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UpdateEmployeeInput.FirstName)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UpdateEmployeeInput.LastName)));
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UpdateEmployeeInput.Email)));
        }

        [Fact]
        public void UpdateEmployeeInput_ShouldFail_WhenEmailIsInvalid()
        {
            var input = new UpdateEmployeeInput
            {
                Id = 1,
                FirstName = "Ivan",
                LastName = "Ivanov",
                Email = "invalid-email",
                JobTitle = "Dev",
                Salary = 1200,
                DepartmentId = 2
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(input, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(UpdateEmployeeInput.Email)));
        }

        [Fact]
        public void UpdateEmployeeInput_ShouldPass_WhenValid()
        {
            var input = new UpdateEmployeeInput
            {
                Id = 1,
                FirstName = "Maria",
                LastName = "Petrova",
                Email = "maria@example.com",
                JobTitle = "QA",
                Salary = 1500,
                DepartmentId = 3
            };

            var context = new ValidationContext(input, null, null);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(input, context, results, true);

            Assert.True(isValid);
        }
    }
}
