using AutoMapper;
using LastFMExtractor.Application.DatabaseService;
using LastFMExtractor.Application.ExtractorService;
using LastFMExtractor.Application.TransformService;
using LastFMExtractor.Application.UrlBuilderService;
using LastFMExtractor.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Registry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LastFMExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string baseUrl = "http://ws.audioscrobbler.com/2.0/";

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    IPolicyRegistry<string> registry = services.AddPolicyRegistry();

                    IAsyncPolicy<HttpResponseMessage> httWaitAndpRetryPolicy =
                        Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

                    registry.Add("SimpleWaitAndRetryPolicy", httWaitAndpRetryPolicy);

                    IAsyncPolicy<HttpResponseMessage> noOpPolicy = Policy.NoOpAsync()
                        .AsAsyncPolicy<HttpResponseMessage>();

                    registry.Add("NoOpPolicy", noOpPolicy);

                    services.AddHttpClient("LastFmClient", client =>
                    {
                        client.BaseAddress = new Uri(baseUrl);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    }).AddPolicyHandlerFromRegistry((policyRegistry, httpRequestMessage) =>
                    {
                        if (httpRequestMessage.Method == HttpMethod.Get || httpRequestMessage.Method == HttpMethod.Delete)
                        {
                            return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("SimpleWaitAndRetryPolicy");
                        }
                        return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("NoOpPolicy");
                    });

                    // Auto Mapper Configurations
                    //var mappingConfig = new MapperConfiguration(mc =>
                    //{
                    //    mc.AddProfile(new DataMapper());
                    //});

                    //IMapper mapper = mappingConfig.CreateMapper();

                    //services.AddSingleton(mapper);

                    //var config = new MapperConfiguration(cfg =>
                    //{                        
                    //    cfg.AddProfile(new DataMapper());
                    //});
                    //var mapper = config.CreateMapper();

                    services.AddAutoMapper();
                    services.AddSingleton<IHostedService, Run>();
                    services.AddSingleton<IDatabaseService, DatabaseService>();
                    services.AddSingleton<IExtractorService, Application.ExtractorService.ExtractorService>();
                    services.AddSingleton<IUrlBuilderService, UrlBuilderService>();
                    services.AddSingleton<ITransformService, TransformService>();

                    services.AddSingleton<MusicContext, MusicContext>();
                });

            await builder.RunConsoleAsync();
        }
    }
}
