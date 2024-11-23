using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Email
{
    public class GmailService(EmailClientConfig config) : IEmailService
    {
        private readonly string _senderEmail = config.SenderEmail;
        private readonly string _appPassword = config.AppPassword;
        private readonly string _smtpHost = config.SmtpHost;
        private readonly int _smtpPort = config.SmtpPort;
        public async Task SendEmailAsync(string email, string subject, string body)
        {

            MimeMessage message = new();
            message.From.Add(new MailboxAddress("VietWay", _senderEmail));
            message.To.Add(new MailboxAddress(string.Empty, email));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };
            using SmtpClient client = new();
            await client.ConnectAsync(_smtpHost, _smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_senderEmail, _appPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
