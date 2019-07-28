using Newtonsoft.Json;

namespace LastFMExtractor.Domain.Models
{
    public class Date
    {
        public string Uts { get; set; }

        [JsonProperty("#text")]
        public string LongDateTime { get; set; }
    }
}
