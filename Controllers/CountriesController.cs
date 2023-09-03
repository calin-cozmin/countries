using countries.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace countries.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountries([FromQuery] string? countryName, [FromQuery] int? maxPopulation)
        {
            try
            {
                var countries = await _countryService.GetAllCountriesAsync(countryName, maxPopulation);
                if (countries == null || !countries.Any())
                {
                    return NotFound("No countries match the given criteria.");
                }

                return Ok(countries);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }
    }
}
