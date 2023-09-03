using countries.Models;

namespace countries.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllCountriesAsync(string? countryName = null, int? maxPopulation = null, string? sort = null, int? take = null);
    }
}
