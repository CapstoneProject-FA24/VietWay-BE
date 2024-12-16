using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Email
{
    public class EmailClientConfig(string senderEmail, string appPassword, string smtpHost, int smtpPort)
    {
        private readonly string _senderEmail = senderEmail;
        private readonly string _appPassword = appPassword;
        private readonly string _smtpHost = smtpHost;
        private readonly int _smtpPort = smtpPort;
        public string SenderEmail { get => _senderEmail; }
        public string AppPassword { get => _appPassword; }
        public string SmtpHost { get => _smtpHost; }
        public int SmtpPort { get => _smtpPort; }
    }
}
