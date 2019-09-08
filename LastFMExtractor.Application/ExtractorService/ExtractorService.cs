using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace LastFMExtractor.Application.ExtractorService
{
    public class ExtractorService : IExtractorService
    {
        private readonly IHttpClientFactory _clientFactory;

        public ExtractorService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public async Task<string> Extract(string requestUri)
        {
            var client = _clientFactory.CreateClient("LastFmClient");
            var response = await client.GetAsync(requestUri);
            var json = await response.Content.ReadAsStringAsync();
            
            return json;
        }
    }
}
