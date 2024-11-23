using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Email
{
    public class EmailClientConfig
    {
        public required string SenderEmail { get; set; }
        public required string AppPassword { get; set; }
        public required string SmtpHost { get; set; }
        public required int SmtpPort { get; set; }
    }
}
