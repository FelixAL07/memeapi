using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class MemeController : ControllerBase
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [HttpGet("meme-of-the-day")]
        public async Task<IActionResult> GetMemeOfTheDay()
        {
            var response = await httpClient.GetAsync("https://meme-api.com/gimme");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch meme");

            var content = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(content);
            Console.WriteLine(content);
            doc.RootElement.TryGetProperty("nsfw", out var nsfwElement);
            if (nsfwElement.GetBoolean())
            {
                return StatusCode(403, "Fetched image not following compliance");
            }
            return Content(content);
        }
    }
}