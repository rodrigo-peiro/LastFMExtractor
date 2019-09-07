using AutoMapper;
using LastFMExtractor.Application.DatabaseService;
using LastFMExtractor.Application.ExtractorService;
using LastFMExtractor.Application.TransformService;
using LastFMExtractor.Application.UrlBuilderService;
using LastFMExtractor.Domain.Settings;
using LastFMExtractor.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Registry;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LastFMExtractor
{
    class Program
    {
        private static IConfigurationRoot _config;

        static async Task Main(string[] args)
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();               

            var builder = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<LastFmSettings>(_config.GetSection("LastFmSettings"));

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
                        client.BaseAddress = new Uri(_config.GetValue<string>("LastFmSettings:BaseUrl"));
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    }).AddPolicyHandlerFromRegistry((policyRegistry, httpRequestMessage) =>
                    {
                        if (httpRequestMessage.Method == HttpMethod.Get || httpRequestMessage.Method == HttpMethod.Delete)
                        {
                            return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("SimpleWaitAndRetryPolicy");
                        }
                        return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("NoOpPolicy");
                    });                   

                    services.AddAutoMapper();
                    services.AddSingleton<IHostedService, Run>();
                    services.AddSingleton<IDatabaseService, DatabaseService>();
                    services.AddSingleton<IExtractorService, ExtractorService>();
                    services.AddSingleton<IUrlBuilderService, UrlBuilderService>();
                    services.AddSingleton<ITransformService, TransformService>();
                    AddMusicContextDb(services);
                });

            await builder.RunConsoleAsync();
        }

        protected static void AddMusicContextDb(IServiceCollection services)
        {
            services.AddDbContext<MusicContext>(options => options.UseSqlServer
            (
                _config.GetConnectionString("LastFmDb")
            ));
        }
    }
}
