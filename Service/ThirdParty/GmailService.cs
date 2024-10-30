using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.ThirdParty
{
    public class GmailService : IEmailService
    {
        private readonly string _senderEmail = Environment.GetEnvironmentVariable("GOOGLE_SENDER_EMAIL")
            ?? throw new Exception("GOOGLE_SENDER_EMAIL is not set in environment variables");
        private readonly string _appPassword = Environment.GetEnvironmentVariable("GOOGLE_APP_PASSWORD")
            ?? throw new Exception("GOOGLE_APP_PASSWORD is not set in environment variables");
        public async Task SendEmailAsync(string email, string subject, string body)
        {

            MimeMessage message = new();
            message.From.Add(new MailboxAddress("VietWay", "dotoan22112003@gmail.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using SmtpClient client = new();
            await client.ConnectAsync("smtp.google.com", 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_senderEmail, _appPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
