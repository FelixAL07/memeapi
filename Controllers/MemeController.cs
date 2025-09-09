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
        private static readonly Dictionary<ulong, string> hashDict = new();
        private static readonly Queue<ulong> hashQueue = new(); // tracks oldest hashes
        private static readonly HttpClient httpClient = new();
        private static readonly object _lock = new(); // ensure thread safety

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

            // Thread-safe block
            lock (_lock)
            {
                if (hashDict.Count >= MaxMemesInDictionary)
                {
                    // Remove oldest meme
                    var oldestHash = hashQueue.Dequeue();
                    hashDict.Remove(oldestHash);

                    Console.WriteLine($"Removed oldest meme to stay within limit of {MaxMemesInDictionary}");
                }

                // Store new meme
                hashDict[hash] = url;
                hashQueue.Enqueue(hash);
            }

            Console.WriteLine($"Unique meme found after {attempts} attempts");
            Console.WriteLine($"Hashes stored: {hashDict.Count}/{MaxMemesInDictionary}");

            return Content(url);
        }
    }
}
