using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagement.Models.InputModels
{
    public class UpdateEmployeeInput
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string JobTitle { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public int DepartmentId { get; set; }
    }
}
