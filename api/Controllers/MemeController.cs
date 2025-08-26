using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class MemeController : ControllerBase
    {   
        

        private static readonly HttpClient httpClient = new();
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        [HttpGet("meme-of-the-day")]
        public async Task<IActionResult> GetMemeOfTheDay()
        {
            var response = await httpClient.GetAsync("https://meme-api.com/gimme");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to fetch meme");

            var content = await response.Content.ReadAsStringAsync();
            Meme? meme;
            string url;
            try
            {
                meme = JsonSerializer.Deserialize<Meme>(content, _jsonOptions);
                if (meme == null || meme.Url == null)
                {
                    return StatusCode(500, "Meme data or URL is null after deserialization");
                }
                url = meme.Url;
                Console.WriteLine(url);

            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to deserialize meme data");
            }
            if (meme == null)
            {
                return StatusCode(500, "Meme data is null after deserialization");
            }
            if (meme.Nsfw)
            {
                return StatusCode(403, "Fetched image not following compliance");
            }
            return Content(url);
        }
    }
}