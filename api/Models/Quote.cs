using System.Text.Json.Serialization;

namespace api.Models
{
    public class QuoteModel
    {
        [JsonPropertyName("quote")]
        public string? Quote { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }
    }
}
