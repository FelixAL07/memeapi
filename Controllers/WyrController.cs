using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using api.Models;
using dotenv.net;

namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class WyrController : ControllerBase
    {
        // Update to store a properly typed cached object
        private static WouldYouRather? cachedWyr = null;
        private static readonly HttpClient httpClient = new();

        [HttpGet("wyr")]
        public async Task<IActionResult> GetWouldYouRather()
        {
            // Check if we have a cached result and if it's from today
            if (cachedWyr != null && cachedWyr.FetchDate.Date == DateTime.Today)
            {
                Console.WriteLine("Returning cached WYR from today");
                return Ok(cachedWyr);
            }

            DotEnv.Load();

            string? apiKey = Environment.GetEnvironmentVariable("RAPDID_API_WYR_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                return BadRequest("RAPDID_API_WYR_KEY environment variable is not set.");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "https://would-you-rather.p.rapidapi.com/wyr/random");
            request.Headers.Add("x-rapidapi-key", apiKey);
            request.Headers.Add("x-rapidapi-host", "would-you-rather.p.rapidapi.com");

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Add options to handle case-sensitivity
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var wouldYouRather = JsonSerializer.Deserialize<WouldYouRather[]>(responseContent, options);
            if (wouldYouRather != null && wouldYouRather.Length > 0)
            {
                // Set the fetch date to today and cache it
                wouldYouRather[0].FetchDate = DateTime.Today;
                cachedWyr = wouldYouRather[0];

                Console.WriteLine(cachedWyr);
                Console.WriteLine("Fetched new WYR from API");
                return Ok(cachedWyr);
            }

            return StatusCode(500, "Failed to load would you rather");

        }
    }
}