using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System.Threading.Tasks;

namespace Polyrific.Project.Mvc
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<SmtpSetting> optionsAccessor)
        {
            _smtpSetting = optionsAccessor.Value;
        }

        private readonly SmtpSetting _smtpSetting;

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(_smtpSetting.SenderEmail));
            mail.To.Add(new MailboxAddress(email));

            mail.Subject = subject;

            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSetting.Server, _smtpSetting.Port, SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_smtpSetting.Username, _smtpSetting.Password);

                await client.SendAsync(mail);
                await client.DisconnectAsync(true);
            }
        }
    }
}

