using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Sms
{
    public class SmsConfiguration(string token, string deviceId, string sendTokenMessage)
    {
        private readonly string _token = token;
        private readonly string _deviceId = deviceId;
        private readonly string _sendTokenMessage = sendTokenMessage;
        public string Token { get => _token; }
        public string DeviceId { get => _deviceId; }
        public string SendTokenMessage { get => _sendTokenMessage; }
    }
}
