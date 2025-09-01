using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class FunfactController : ControllerBase
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [HttpGet("fun-fact")]
        public async Task<IActionResult> GetFunFact()
        {
            var response = await httpClient.GetAsync("https://uselessfacts.jsph.pl/api/v2/facts/random");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch fun fact");

            var content = await response.Content.ReadAsStringAsync();
            var funFact = JsonSerializer.Deserialize<Funfact>(content);

            if (funFact != null && !string.IsNullOrEmpty(funFact.Text))
            {
                return Ok(funFact);
            }
            return StatusCode(500, "Failed to parse fun fact response");
        }
    }
}