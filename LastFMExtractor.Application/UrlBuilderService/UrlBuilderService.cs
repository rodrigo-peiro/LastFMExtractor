using LastFMExtractor.Domain.Settings;
using Microsoft.Extensions.Options;
using System.Web;

namespace LastFMExtractor.Application.UrlBuilderService
{
    public class UrlBuilderService : IUrlBuilderService
    {
        private readonly LastFmSettings settings;

        public UrlBuilderService(IOptions<LastFmSettings> options)
        {
            settings = options.Value;
        }

        public string BuildUrl(string latestRecordTimestamp = "", string page = "1")
        {
            var queryStringKeyValuePairs = HttpUtility.ParseQueryString(string.Empty);
            queryStringKeyValuePairs.Add("method", settings.Method);
            queryStringKeyValuePairs.Add("user", settings.User);
            queryStringKeyValuePairs.Add("api_key", settings.ApiKey);            
            queryStringKeyValuePairs.Add("format", settings.Format);
            queryStringKeyValuePairs.Add("page", page);

            if (!string.IsNullOrEmpty(latestRecordTimestamp))
            {
                queryStringKeyValuePairs.Add("from", latestRecordTimestamp);
            }

            return "?" + queryStringKeyValuePairs.ToString();
        }        
    }
}
