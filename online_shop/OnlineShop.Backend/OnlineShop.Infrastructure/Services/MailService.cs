using Microsoft.Extensions.Options;
using OnlineShop.Core.Models;
using OnlineShop.Core.Interfaces;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Configuration;

namespace OnlineShop.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService()
        {
        }
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;

            
        }
        public async Task SendMail(MailContentModel mailContent)
        {
            //var mailSettings = new MailSettings
            //{
            //    Mail = _configuration["MailSettings:Mail"],
            //    DisplayName = _configuration["MailSettings:DisplayName"],
            //    Password = _configuration["MailSettings:Password"],
            //    Host = _configuration["MailSettings:Host"],
            //    Port = int.Parse(_configuration["MailSettings:Port"])
            //};
            var mailSettings = new MailSettings
            {
                Mail = "hxuan190@gmail.com",
                DisplayName = "Crawford Cummings",
                Password = "emvqfjcbawuxlemo",
                Host = "smtp.gmail.com",
                Port = 587
            };

            var email = new MimeMessage();

            email.Sender = MailboxAddress.Parse(mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
            }

            smtp.Disconnect(true);

        }
    }
}
