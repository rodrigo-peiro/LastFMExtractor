using AutoMapper;
using LastFMExtractor.Application.DatabaseService;
using LastFMExtractor.Application.ExtractorService;
using LastFMExtractor.Application.TransformService;
using LastFMExtractor.Application.UrlBuilderService;
using LastFMExtractor.Domain.Entities;
using LastFMExtractor.Domain.Models;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace LastFMExtractor
{
    public class Run : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDatabaseService _databaseService;
        private readonly IUrlBuilderService _urlBuilderService;
        private readonly IExtractorService _extractorService;
        private readonly ITransformService _transformService;
        private readonly IMapper _mapper;
        private readonly Stopwatch _stopWatch = new Stopwatch();

        public Run(IHttpClientFactory httpClientFactory, IDatabaseService databaseService, IUrlBuilderService urlBuilderService, 
                    IExtractorService extractorService, ITransformService transformService, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _databaseService = databaseService;
            _urlBuilderService = urlBuilderService;
            _extractorService = extractorService;
            _transformService = transformService;
            _mapper = mapper;
        }

        public async Task ExtractTransformLoad()
        {            
            var httpClient = _httpClientFactory.CreateClient("LastFmClient");
            var recordCount = await _databaseService.GetRecordCount();
            var isTableEmpty = recordCount == 0 ? true : false;
            var mostRecentTimestamp = string.Empty;
            var url = string.Empty;
            var jsonResponse = string.Empty;
            var jobId = Guid.NewGuid();
            var jobLog = new Job()
            {
                JobId = jobId,
                StartDateTime = DateTime.Now,
                EndDateTime = DateTime.Now
            };

            if (isTableEmpty)
            {
                url = _urlBuilderService.BuildUrl();
            }
            else
            {
                mostRecentTimestamp = await _databaseService.GetLatestRecordTimestamp();
                url = _urlBuilderService.BuildUrl(mostRecentTimestamp);
            }

            jsonResponse = await _extractorService.Extract(httpClient, url);

            if (string.IsNullOrEmpty(jsonResponse))
            {
                await _databaseService.LogJobCreateNoDataFound(jobId);
                return;
            }

            var lastFmData = _transformService.TransformJsonToObject(jsonResponse);
            var numberOfRecords = int.Parse(lastFmData.RecentTracks.Attr.Total);
            var numberOfPages = int.Parse(lastFmData.RecentTracks.Attr.TotalPages);
            var currentPage = int.Parse(lastFmData.RecentTracks.Attr.Page);
            var recordsProcessed = 0;                                    

            if (numberOfRecords == 0)
            {
                await _databaseService.LogJobCreateNoDataFound(jobId);
                return;
            }

            var listForDb = new List<ExportedTracks>();
            listForDb.AddRange(_mapper.Map<List<Track>, List<ExportedTracks>>(lastFmData.RecentTracks.Track));

            if (listForDb.Any())
            {
                jobLog.RecordsFound = numberOfRecords;
                await _databaseService.LogJobCreate(jobLog);

                listForDb.ForEach(x => x.JobId = jobId);
                listForDb.ForEach(x => x.DateCreated = _transformService.TransformUnixDateTimeToRegularDateTime(x.DateCreatedUnix.Value));

                await _databaseService.BulkInsert(listForDb);

                recordsProcessed += listForDb.Count();

                Console.WriteLine($"Total # of new records found: { numberOfRecords.ToString() }.\n");
                Console.WriteLine($"Page: { currentPage.ToString() } of { numberOfPages.ToString() } processed successfully.\n");
            }

            if (numberOfPages > 1)
            {
                currentPage++;

                for (int i = currentPage; i <= numberOfPages; i++)
                {
                    if (isTableEmpty)
                    {
                        url = _urlBuilderService.BuildUrl(latestRecordTimestamp: "", page: currentPage.ToString());
                    }
                    else
                    {
                        url = _urlBuilderService.BuildUrl(mostRecentTimestamp, currentPage.ToString());
                    }

                    jsonResponse = await _extractorService.Extract(httpClient, url);
                    lastFmData = _transformService.TransformJsonToObject(jsonResponse);
                    listForDb = new List<ExportedTracks>();
                    listForDb.AddRange(_mapper.Map<List<Track>, List<ExportedTracks>>(lastFmData.RecentTracks.Track));
                    listForDb.ForEach(x => x.JobId = jobId);
                    listForDb.ForEach(x => x.DateCreated = _transformService.TransformUnixDateTimeToRegularDateTime(x.DateCreatedUnix.Value));

                    await _databaseService.BulkInsert(listForDb);
                    recordsProcessed += listForDb.Count();

                    Console.WriteLine($"Page: { currentPage.ToString() } of { numberOfPages.ToString() } processed successfully.\n");

                    currentPage++;
                }                                
            }

            jobLog.RecordsProcessed = recordsProcessed;
            jobLog.EndDateTime = DateTime.Now;
            jobLog.Succeeded = true;

            await _databaseService.LogJobUpdate(jobLog);            
            Console.WriteLine($"Total # of records processed: { numberOfRecords.ToString() }\n");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _stopWatch.Start();
            Console.WriteLine("Process started.\n");

            await ExtractTransformLoad();                

            _stopWatch.Stop();

            var duration = _stopWatch.Elapsed.Duration();

            Console.WriteLine($"Process completed in { duration.Minutes } minutes and { duration.Seconds } seconds.\n");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
