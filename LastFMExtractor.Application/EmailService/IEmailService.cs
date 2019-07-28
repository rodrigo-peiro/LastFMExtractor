using LastFMExtractor.Domain.Entities;

namespace LastFMExtractor.Application.EmailService
{
    public interface IEmailService
    {
        void Notify(Job job);
    }
}
