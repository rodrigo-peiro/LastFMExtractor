using System.Collections.Generic;

namespace LastFMExtractor.Domain.Models
{
    public class Track
    {
        public Artist Artist { get; set; }
        public string Name { get; set; }
        public string Streamable { get; set; }
        public string Mbid { get; set; }
        public Album Album { get; set; }
        public string Url { get; set; }
        public List<Image> Image { get; set; }
        public Date Date { get; set; }
    }
}
