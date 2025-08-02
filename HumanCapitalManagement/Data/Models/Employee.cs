using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanCapitalManagement.Data.Models;

public class Employee
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
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }
    
    [Required]
    public int DepartmentId { get; set; }

    [ForeignKey("DepartmentId")]
    public Department Department { get; set; }
    
    [Required]
    public int CountryId { get; set; }

    [ForeignKey("CountryId")]
    public Country Country { get; set; }
    
    [StringLength(50)]
    public string IBAN { get; set; }
}