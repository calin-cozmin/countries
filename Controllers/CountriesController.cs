using Microsoft.AspNetCore.Mvc;
using countries.Models;
using countries.Services.Interfaces;

namespace countries.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController(ICountryService countryService, ILogger<CountriesController> logger)
        {
            _countryService = countryService ?? throw new ArgumentNullException(nameof(countryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<ActionResult<List<Country>>> GetAllCountries(
            [FromQuery] string? countryName,
            [FromQuery] int? maxPopulation,
            [FromQuery] string? sort,
            [FromQuery] int? take)
        {
            try
            {
                var countries = await _countryService.GetAllCountriesAsync(countryName, maxPopulation, sort, take);
                return Ok(countries);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get countries: {ex.Message}");
                return BadRequest("An error occurred while fetching country data.");
            }
        }
    }
}
