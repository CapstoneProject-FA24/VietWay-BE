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
    public class GmailService : IEmailService
    {
        private readonly string _senderEmail = Environment.GetEnvironmentVariable("GOOGLE_SENDER_EMAIL")
            ?? throw new Exception("GOOGLE_SENDER_EMAIL is not set in environment variables");
        private readonly string _appPassword = Environment.GetEnvironmentVariable("GOOGLE_APP_PASSWORD")
            ?? throw new Exception("GOOGLE_APP_PASSWORD is not set in environment variables");
        private readonly string _smtpHost = Environment.GetEnvironmentVariable("GOOGLE_SMTP_HOST")
            ?? throw new Exception("GOOGLE_SMTP_HOST is not set in environment variables");
        private readonly int _smtpPort = int.Parse(Environment.GetEnvironmentVariable("GOOGLE_SMTP_PORT")
            ?? throw new Exception("GOOGLE_SMTP_PORT is not set in environment variables"));
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
