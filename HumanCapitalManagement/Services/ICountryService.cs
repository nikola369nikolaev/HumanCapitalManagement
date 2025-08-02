using HumanCapitalManagement.Data.Models;

namespace HumanCapitalManagement.Services;

public interface ICountryService
{
    Task<IEnumerable<Country>> GetAll();
        
    Task<Country?> GetById(int id);
}