using HumanCapitalManagement.Data;
using HumanCapitalManagement.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanCapitalManagement.Services;

public class CountryService : ICountryService
{
    private readonly ApplicationDbContext _context;

    public CountryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Country>> GetAll()
    {
        return await _context.Countries.ToListAsync();
    }

    public async Task<Country?> GetById(int id)
    {
        return await _context.Countries.FirstOrDefaultAsync(x=>x.Id == id);
    }
}