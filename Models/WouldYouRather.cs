using System.Text.Json.Serialization;

namespace api.Models
{
    public class WouldYouRather
    {
        [JsonPropertyName("question")]
        public string? Question { get; set; }
        
        // Cache control - ignored in serialization
        [JsonIgnore]
        public DateTime FetchDate { get; set; }
    }
}
