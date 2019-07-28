using System.Net.Http;
using System.Threading.Tasks;

namespace LastFMExtractor.Application.ExtractorService
{
    public interface IExtractorService
    {
        Task<string> Extract(HttpClient httpClient, string requestUri);
    }
}
