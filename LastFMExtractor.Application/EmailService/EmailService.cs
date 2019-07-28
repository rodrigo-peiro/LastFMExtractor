using LastFMExtractor.Domain.Entities;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LastFMExtractor.Application.EmailService
{
    public class EmailService : IEmailService
    {
        public void Notify(Job job)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress("r_peiro_s@yahoo.com"));
                message.From = new MailAddress("no-reply@lastfmetl.com");
                message.Subject = "ETL job completed successfully";
                message.Body = BuildEmailBody(job);
                message.IsBodyHtml = false;

                using (var client = new SmtpClient("relay-hosting.secureserver.net"))
                {
                    client.Port = 25;
                    //client.Credentials = new NetworkCredential("rodrigo.peiro@gmail.com", "cuauht3m0c!");                    
                    client.EnableSsl = false;
                    client.Send(message);
                }
            }
        }

        private string BuildEmailBody(Job job)
        {
            var sb = new StringBuilder();
            sb.Append($"Total # of records processed: { job.RecordsProcessed.ToString() }.");
            sb.AppendLine();
            sb.Append($"Start date & time: { job.StartDateTime.ToShortTimeString() }.");
            sb.AppendLine();
            sb.Append($"End date & time: { job.StartDateTime.ToShortTimeString() }.");

            return sb.ToString();
        }
    }
}
