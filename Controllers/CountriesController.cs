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
        public async Task<IActionResult> GetAllCountries([FromQuery] string? countryName)
        {
            var countries = await _countryService.GetAllCountriesAsync(countryName);
            if (countries == null)
            {
                return NotFound();
            }

            return Ok(countries);
        }
    }
}
