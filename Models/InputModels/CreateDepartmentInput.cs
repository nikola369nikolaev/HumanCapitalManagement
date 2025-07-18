using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagement.Models.InputModels;

public class CreateDepartmentInput
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}