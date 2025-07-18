namespace HumanCapitalManagement.Models.ViewModels;
    
public class EmployeeViewModel
{
    public int Id { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Email { get; set; }
    
    public string JobTitle { get; set; }
    
    public decimal Salary { get; set; }
    
    public int DepartmentId { get; set; }
    
    public string DepartmentName { get; set; }
    
    public string Role { get; set; }
}