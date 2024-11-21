using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Sms
{
    public class SmsConfiguration
    {
        public required string Token { get; set; }
        public required string DeviceId { get; set; }
        public required string SendTokenMessage { get; set; }
    }
}
