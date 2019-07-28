using EFCore.BulkExtensions;
using LastFMExtractor.Domain.Entities;
using LastFMExtractor.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LastFMExtractor.Application.DatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        private readonly MusicContext _dbContext;

        public DatabaseService(MusicContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> GetRecordCount()
        {
            return await _dbContext.ExportedTracks.CountAsync();
        }

        public async Task<string> GetLatestRecordTimestamp()
        {
            var latestRecordTimestamp =  await _dbContext.ExportedTracks.MaxAsync(x => x.DateCreatedUnix.Value);
            latestRecordTimestamp++;

            return latestRecordTimestamp.ToString();
        }

        public async Task BulkInsert(List<ExportedTracks> newTracks)
        {
            await _dbContext.BulkInsertAsync(newTracks);
        }

        public async Task LogJobCreate(Job job)
        {
            _dbContext.Add(job);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LogJobCreateNoDataFound(Guid jobId)
        {
            var job = new Job()
            {
                JobId = jobId,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now,
                RecordsFound = 0,
                RecordsProcessed = 0,
                Succeeded = true
            };

            _dbContext.Add(job);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LogJobUpdate(Job job)
        {
            var jobRecord = await _dbContext.Jobs.FirstOrDefaultAsync(j => j.JobId == job.JobId);
            jobRecord.RecordsProcessed = job.RecordsProcessed;
            jobRecord.EndDateTime = job.EndDateTime;
            jobRecord.Succeeded = job.Succeeded;

            _dbContext.Jobs.Update(jobRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Job> GetJob(Guid jobId)
        {
            return await _dbContext.Jobs.Include(j => j.ExportedTracks).FirstOrDefaultAsync(j => j.JobId == jobId);
        }
    }
}
