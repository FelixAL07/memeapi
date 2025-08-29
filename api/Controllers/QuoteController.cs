using Microsoft.AspNetCore.Mvc;
using dotenv.net;

namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class QuoteController : ControllerBase
    {
        private static readonly HttpClient httpClient = new();


        [HttpGet("quote")]
        public async Task<IActionResult> GetQuote()
        {
            DotEnv.Load();

            string? apiKey = Environment.GetEnvironmentVariable("NINJA_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("NINJA_API_KEY environment variable is not set.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.api-ninjas.com/v1/quotes");
            request.Headers.Add("X-Api-Key", apiKey);
            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return Content(responseContent, "application/json");
        }
    }
}