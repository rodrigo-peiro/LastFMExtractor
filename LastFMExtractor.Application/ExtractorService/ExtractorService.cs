using System.Net.Http;
using System.Threading.Tasks;

namespace LastFMExtractor.Application.ExtractorService
{
    public class ExtractorService : IExtractorService
    {
        //private readonly IUrlBuilderService _urlBuilderService;

        public ExtractorService()
        {
            //_urlBuilderService = urlBuilderService;
        }

        public async Task<string> Extract(HttpClient httpClient, string requestUri)
        {            
            //var requestUri = string.IsNullOrEmpty(page) ? 
            //                _urlBuilderService.BuildUrlAddLatestRecordTimestamp(latestRecordTimestamp) :
            //                _urlBuilderService.BuildUrlAddPage(latestRecordTimestamp, page);

            var response = await httpClient.GetAsync(requestUri);
            var json = await response.Content.ReadAsStringAsync();
            
            return json;
        }
    }
}
