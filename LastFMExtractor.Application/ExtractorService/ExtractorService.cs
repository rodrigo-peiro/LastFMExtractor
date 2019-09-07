using System.Net.Http;
using System.Threading.Tasks;

namespace LastFMExtractor.Application.ExtractorService
{
    public class ExtractorService : IExtractorService
    {
        public async Task<string> Extract(HttpClient httpClient, string requestUri)
        {   
            var response = await httpClient.GetAsync(requestUri);
            var json = await response.Content.ReadAsStringAsync();
            
            return json;
        }
    }
}
