using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Util.TokenUtil
{
    public class TokenConfig(string issuer, string audience, string secret, int tokenExpireAfterMinutes)
    {
        private readonly string _issuer = issuer;
        private readonly string _audience = audience;
        private readonly string _secret = secret;
        private readonly int _tokenExpireAfterMinutes = tokenExpireAfterMinutes;
        public string Issuer { get => _issuer; }
        public string Audience { get => _audience; }
        public string Secret { get => _secret; }
        public int TokenExpireAfterMinutes { get => _tokenExpireAfterMinutes; }
    }
}
