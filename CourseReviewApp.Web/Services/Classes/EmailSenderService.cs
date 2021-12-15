using Microsoft.Extensions.Configuration;
using CourseReviewApp.Web.Services.Interfaces;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CourseReviewApp.Web.Services.Classes
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _config;
        private readonly IFileService _fileService;

        public EmailSenderService(SmtpClient smtpClient, IConfiguration config, IFileService fileTool)
        {
            _smtpClient = smtpClient;
            _config = config;
            _fileService = fileTool;
        }

        public async Task SendEmailAsync(string recipient, string subject, string body)
        {
            MailMessage message = new MailMessage();
            string host = _config.GetValue<string>("Email:Smtp:Username");
            message.From = new MailAddress(host);
            message.To.Add(recipient);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendDefaultMessageEmailAsync(string recipient, string subject, string message)
        {
            string body = await _fileService.LoadMessageHtml("default_message.html");
            body = body.Replace("{username}", recipient);
            body = body.Replace("{message}", message);

            await SendEmailAsync(recipient, subject, body);
        }
    }
}
