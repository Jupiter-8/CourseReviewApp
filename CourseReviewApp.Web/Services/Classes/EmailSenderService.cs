using Microsoft.Extensions.Configuration;
using CourseReviewApp.Web.Services.Interfaces;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

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

        public async Task SendEmailAsync(string subject, string body, string recipient = null, IList<string> bccs = null)
        {
            MailMessage message = new();

            if (string.IsNullOrEmpty(recipient) && bccs == null)
                throw new ArgumentNullException("Email recipient or bccs not provided.");
            if (!string.IsNullOrEmpty(recipient))
                message.To.Add(recipient);
            if (bccs != null)
                foreach (var adress in bccs)
                    message.Bcc.Add(adress);

            string host = _config.GetValue<string>("Email:Smtp:Username");
            message.From = new MailAddress(host);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendDefaultMessageEmailAsync(string subject, string message, string recipient = null, IList<string> bccs = null)
        {
            string body = await _fileService.LoadMessageHtml("default_message.html");
            body = body.Replace("{message}", message);

            await SendEmailAsync(subject, body, recipient, bccs);
        }
    }
}
