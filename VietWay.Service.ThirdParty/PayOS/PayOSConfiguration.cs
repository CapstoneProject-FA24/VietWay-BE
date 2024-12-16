using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.PayOS
{
    public class PayOSConfiguration(string apiKey, string checksumKey, string clientId, string returnUrl)
    {
        private readonly string _apiKey = apiKey;
        private readonly string _checksumKey = checksumKey;
        private readonly string _clientId = clientId;
        private readonly string _returnUrl = returnUrl;
        public string ApiKey { get => _apiKey; }
        public string ChecksumKey { get => _checksumKey; }
        public string ClientId { get => _clientId; }
        public string ReturnUrl { get => _returnUrl; }
    }
}
