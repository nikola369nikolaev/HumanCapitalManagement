using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagement.Data.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Department name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        // Навигационно свойство към служителите
        public ICollection<Employee> Employees { get; set; }
    }
}