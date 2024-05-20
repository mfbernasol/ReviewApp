using AutoMapper;
using ReviewApp.Data;
using ReviewApp.Interfaces;
using ReviewApp.Models;

namespace ReviewApp.Repository;

internal class CountryRepository : ICountryRepository
{
    private readonly DataContext _context;


    public CountryRepository(DataContext context)
    {
        _context = context;
    
    }
    public ICollection<Country> GetCountries()
    {
        return _context.Countries.ToList();
    }

    public Country GetCountry(int id)
    {
        return _context.Countries
            .Where(c => c.Id == id)
            .FirstOrDefault();
    }

    public Country GetCountryByOwner(int ownerId)
    {
        return _context.Owners
            .Where(o => o.Id == ownerId)
            .Select(c => c.Country).FirstOrDefault();
    }

    public ICollection<Owner> GetOwnersFromACountry(int countryId)
    {
        return _context.Owners
            .Where(c => c.Country.Id == countryId).ToList();
    }

    public bool CountryExists(int id)
    {
        return _context.Countries.Any(c => c.Id == id);
    }
}