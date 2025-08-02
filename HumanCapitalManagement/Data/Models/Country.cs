using System.ComponentModel.DataAnnotations;

namespace HumanCapitalManagement.Data.Models;

public class Country
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(2)]
    public string Code { get; set; }
}