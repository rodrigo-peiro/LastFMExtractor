using Newtonsoft.Json;

namespace LastFMExtractor.Domain.Models
{
    public class Image
    {
        [JsonProperty("#text")]
        public string Name { get; set; }

        public string Size { get; set; }
    }
}
