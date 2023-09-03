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

        public async Task<List<Country>> GetAllCountriesAsync(string? countryName = null)
        {
            try
            {
                using (var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all"))
                {
                    response.EnsureSuccessStatusCode(); // throws HttpRequestException if not successful

                    // Read directly from the response stream
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

                            // Apply the filter for country name here, if provided.
                            if (!string.IsNullOrEmpty(countryName))
                            {
                                countries = countries.Where(c => c.Name?.Common != null &&
                                                                 c.Name.Common.IndexOf(countryName, StringComparison.OrdinalIgnoreCase) >= 0)
                                                     .ToList();
                            }

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
    }
}
