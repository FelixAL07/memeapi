using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            using var doc = JsonDocument.Parse(content);
            if (doc.RootElement.TryGetProperty("text", out var textElement))
            {
                return Content(textElement.GetString() ?? "");
            }
            return StatusCode(500, "No 'text' property found in response");
        }
    }
}