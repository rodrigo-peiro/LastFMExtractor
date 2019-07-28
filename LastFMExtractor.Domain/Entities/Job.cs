using System;
using System.Collections.Generic;

namespace LastFMExtractor.Domain.Entities
{
    public class Job
    {
        public Job()
        {
            ExportedTracks = new HashSet<ExportedTracks>();
            JobFailure = new JobFailure();
        }

        public Guid JobId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? RecordsProcessed { get; set; }
        public bool? Succeeded { get; set; }
        public int? RecordsFound { get; set; }

        public virtual ICollection<ExportedTracks> ExportedTracks { get; set; }
        public virtual JobFailure JobFailure { get; set; }
    }
}
