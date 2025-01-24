using System.Net.Mail;
using System.Net;
using FinalMvc.Services.Interfaces;

namespace FinalMvc.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;

        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com", // Ensure this is correct
                Port = 587,             // Use 587 for TLS
                EnableSsl = true,       // Ensure SSL is enabled
                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Email"],
                    _configuration["EmailSettings:Password"])
            };

            using var message = new MailMessage(
                _configuration["EmailSettings:Email"], email, subject, htmlMessage)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(message);
        }
    }
}
