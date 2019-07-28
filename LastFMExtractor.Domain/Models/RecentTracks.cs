using Newtonsoft.Json;
using System.Collections.Generic;

namespace LastFMExtractor.Domain.Models
{
    public class RecentTracks
    {
        public List<Track> Track { get; set; }

        [JsonProperty("@attr")]
        public Attr Attr { get; set; }
    }
}
