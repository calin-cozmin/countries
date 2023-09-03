using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<Country>> GetAllCountriesAsync(string? countryName = null, int? maxPopulation = null, string? sort = null, int? take = null)
        {
            try
            {
                using (var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all"))
                {
                    response.EnsureSuccessStatusCode();

                    using (var stream = await response.Content.ReadAsStreamAsync())
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

                        countries = FilterCountriesByName(countries, countryName);
                        countries = FilterCountriesByMaxPopulation(countries, maxPopulation);
                        countries = SortCountries(countries, sort);
                        countries = LimitCountries(countries, take);

                        return countries;
                    }
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogError($"An error occurred while fetching countries: {httpRequestException.Message}");
                throw;
            }
        }

        private List<Country> FilterCountriesByName(List<Country> countries, string? countryName)
        {
            if (!string.IsNullOrEmpty(countryName))
            {
                return countries.Where(c => c.Name?.Common != null &&
                                             c.Name.Common.Contains(countryName, StringComparison.OrdinalIgnoreCase))
                                .ToList();
            }
            return countries;
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
                return countries.Where(c => c.Population.HasValue && c.Population.Value < populationLimit).ToList();
            }

            return countries;
        }

        private List<Country> SortCountries(List<Country> countries, string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                if (sort.Equals("ascend", StringComparison.OrdinalIgnoreCase))
                {
                    return countries.OrderBy(c => c.Name?.Common).ToList();
                }
                else if (sort.Equals("descend", StringComparison.OrdinalIgnoreCase))
                {
                    return countries.OrderByDescending(c => c.Name?.Common).ToList();
                }
                else
                {
                    _logger.LogWarning("Invalid sort parameter provided.");
                    throw new ArgumentException("Invalid sort parameter. Use 'ascend' or 'descend'.");
                }
            }

            return countries;
        }

        private List<Country> LimitCountries(List<Country> countries, int? take)
        {
            if (take.HasValue)
            {
                return countries.Take(take.Value).ToList();
            }

            return countries;
        }
    }
}
