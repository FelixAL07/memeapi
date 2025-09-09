using Microsoft.AspNetCore.Mvc;
using dotenv.net;
using System.Text.Json;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class QuoteController : ControllerBase
    {
        private static readonly HttpClient httpClient = new(
            new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            }
        );


        [HttpGet("quote")]
        public async Task<IActionResult> GetQuote()
        {
            DotEnv.Load();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.quotable.io/quotes/random?limit=1");

            var response = await httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseContent);

            var quotes = JsonSerializer.Deserialize<QuoteModel[]>(responseContent);
            if (quotes != null && quotes.Length > 0)
            {
                return Ok(quotes);
            }
            
            return StatusCode(500, "Failed to parse quote response");
        }
    }
}