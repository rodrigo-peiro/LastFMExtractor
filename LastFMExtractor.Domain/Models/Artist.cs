using Newtonsoft.Json;

namespace LastFMExtractor.Domain.Models
{
    public class Artist
    {
        [JsonProperty("#text")]
        public string Name { get; set; }

        public string Mbid { get; set; }
    }
}
