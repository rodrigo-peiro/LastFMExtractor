using LastFMExtractor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LastFMExtractor.Application.DatabaseService
{
    public interface IDatabaseService
    {
        Task<int> GetRecordCount();

        Task<string> GetLatestRecordTimestamp();

        Task BulkInsert(List<ExportedTracks> newTracks);

        Task LogJobCreate(Job job);

        Task LogJobCreateNoDataFound(Guid jobId);

        Task LogJobUpdate(Job job);

        Task<Job> GetJob(Guid jobId);
    }
}
