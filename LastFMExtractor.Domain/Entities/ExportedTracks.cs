using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LastFMExtractor.Domain.Entities
{
    public partial class ExportedTracks
    {
        public long Id { get; set; }
        public long? DateCreatedUnix { get; set; }
        public string Track { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string TrackId { get; set; }
        public string ArtistId { get; set; }
        public string AlbumId { get; set; }
        public DateTime? DateCreated { get; set; }

        [NotMapped]
        public DateTime? DateExtracted { get; set; }
        public Guid? JobId { get; set; }

        public virtual Job Job { get; set; }
    }
}
