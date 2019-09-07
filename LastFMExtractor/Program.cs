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
using System;
using System.IO;
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
                    
                    services.AddHttpClient("LastFmClient", client =>
                    {
                        client.BaseAddress = new Uri(_config.GetValue<string>("LastFmSettings:BaseUrl"));
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
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
