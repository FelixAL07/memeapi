using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using api.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using CoenM.ImageHash.HashAlgorithms;
using CoenM.ImageHash;

namespace api.Controllers
{
    [ApiController]
    [Route("api")]
    public class MemeController : ControllerBase
    {
        private static readonly int MaxMemesInDictionary = 25; // Maximum number of memes to store
        private static readonly List<string> urlArr = [];
        private static readonly Dictionary<ulong, string> hashDict = [];
        private static readonly HttpClient httpClient = new();
        private static readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        [HttpGet("meme-of-the-day")]
        public async Task<IActionResult> GetMemeOfTheDay()
        {
            var hasher = new PerceptualHash();
            Meme? meme = null;
            string url = "";
            ulong hash = 0;

            int maxAttempts = 500;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                attempts++;
                var response = await httpClient.GetAsync("https://meme-api.com/gimme");
                if (!response.IsSuccessStatusCode)
                    continue;
                var content = await response.Content.ReadAsStringAsync();
                meme = JsonSerializer.Deserialize<Meme>(content, options);

                if (meme == null || meme.Url == null)
                    continue;

                url = meme.Url;

                try
                {
                    // Download image and compute hash
                    var imgBytes = await httpClient.GetByteArrayAsync(url);
                    using var image = Image.Load<Rgba32>(imgBytes);
                    hash = hasher.Hash(image);

                    // Check against existing hashes
                    bool isDuplicate = hashDict.Keys.Any(existing =>
                        CompareHash.Similarity(existing, hash) > 95 // threshold %
                    );

                    if (!isDuplicate)
                    {
                        break; // found a unique meme
                    }
                }
                catch
                {
                    continue; // if image fails to load, try again
                }
            }

            if (meme == null || meme.Url == null)
                return StatusCode(500, "Failed to find a unique meme");

            // Check if we've reached the maximum limit
            if (hashDict.Count >= MaxMemesInDictionary && urlArr.Count > 0)
            {
                // Remove the oldest meme from the dictionary
                string oldestUrl = urlArr[0];
                ulong oldestHash = hashDict.FirstOrDefault(x => x.Value == oldestUrl).Key;
                hashDict.Remove(oldestHash);
                urlArr.RemoveAt(0);
                
                Console.WriteLine($"Removed oldest meme to stay within limit of {MaxMemesInDictionary}");
            }

            // Store hash + url
            hashDict[hash] = url;
            urlArr.Add(url);

            Console.WriteLine($"Unique meme found after {attempts} attempts");
            Console.WriteLine($"Hashes stored: {hashDict.Count}/{MaxMemesInDictionary}");

            return Content(url);
        }
    }
}
