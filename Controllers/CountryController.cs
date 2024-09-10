using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI5.DataModel;
using WebAPI5.Services;

namespace WebAPI5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private IReposotory _repo;
        public CountryController(IReposotory repo) {
            _repo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetData() {
            return Ok(_repo.GetAllCountries());
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetData(int id)
        {
            return Ok(_repo.GetCountriesOnly(id));
        }
        [HttpPost]
        public async Task<IActionResult> PostData(Country country)
        {
            _repo.AddCountries(country);
            return Ok();
        }

    }
}
