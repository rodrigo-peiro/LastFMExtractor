using System.Threading.Tasks;

namespace LastFMExtractor.Application.ExtractorService
{
    public interface IExtractorService
    {
        Task<string> Extract(string requestUri);
    }
}
