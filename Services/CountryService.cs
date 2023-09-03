using countries.Models;
using countries.Services.Interfaces;
using System.Text.Json;

namespace countries.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CountryService> _logger;

        public CountryService(IHttpClientFactory httpClientFactory, ILogger<CountryService> logger)
        {
            _httpClient = httpClientFactory.CreateClient("CountryClient");
            _logger = logger;
        }

        public async Task<List<Country>> GetAllCountriesAsync(string? countryName = null, int? maxPopulation = null)
        {
            try
            {
                using (var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all"))
                {
                    response.EnsureSuccessStatusCode(); 

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        try
                        {
                            var options = new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true,
                            };

                            var countries = await JsonSerializer.DeserializeAsync<List<Country>>(stream, options);

                            if (countries == null)
                            {
                                _logger.LogWarning("Received null or empty data during deserialization.");
                                throw new InvalidOperationException("Data could not be fetched or was empty.");
                            }

                            if (!string.IsNullOrEmpty(countryName))
                            {
                                countries = countries.Where(c => c.Name?.Common != null &&
                                                                 c.Name.Common.IndexOf(countryName, StringComparison.OrdinalIgnoreCase) >= 0)
                                                     .ToList();
                            }

                            countries = FilterCountriesByMaxPopulation(countries, maxPopulation);

                            return countries;
                        }
                        catch (JsonException jsonException)
                        {
                            _logger.LogError($"Failed to deserialize the received content: {jsonException.Message}");
                            throw new InvalidOperationException("Could not deserialize the data.", jsonException);
                        }
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogError($"An error occurred while fetching countries: {httpRequestException.Message}");
                throw;
            }
        }

        private List<Country> FilterCountriesByMaxPopulation(List<Country> countries, int? maxPopulation)
        {
            if (maxPopulation.HasValue)
            {
                if (maxPopulation.Value < 0)
                {
                    _logger.LogWarning("Negative maxPopulation value provided.");
                    throw new ArgumentException("maxPopulation must be a non-negative integer.");
                }

                int populationLimit = maxPopulation.Value * 1_000_000; 
                return countries.Where(c => c.Population < populationLimit).ToList();
            }

            return countries; 
        }
    }
}
