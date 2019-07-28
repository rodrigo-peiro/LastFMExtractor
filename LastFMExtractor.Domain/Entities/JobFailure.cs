using System;

namespace LastFMExtractor.Domain.Entities
{
    public class JobFailure
    {
        public Guid FailureId { get; set; }
        public Guid JobId { get; set; }
        public virtual Job Job { get; set; }
        public string ExceptionDetails { get; set; }
    }
}
