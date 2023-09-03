using countries.Models;

namespace countries.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllCountriesAsync(string? countryName, int? maxPopulation = null);
    }
}
