using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagement.Models.InputModels;

public class UpdateDepartmentInput
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }
}