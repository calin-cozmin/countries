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
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            if (countries == null)
            {
                return NotFound();
            }

            return Ok(countries);
        }
    }

}
