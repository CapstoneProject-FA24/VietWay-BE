using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Cloudinary
{
    public class CloudinaryApiConfig(string apiKey, string apiSecret, string cloudName)
    {
        private readonly string _apiKey = apiKey;
        private readonly string _apiSecret = apiSecret;
        private readonly string _cloudName = cloudName;

        public string ApiKey { get => _apiKey; }
        public string ApiSecret { get => _apiSecret; }
        public string CloudName { get => _cloudName; }
    }
}
