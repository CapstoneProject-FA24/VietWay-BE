namespace VietWay.Service.ThirdParty.Twitter
{
    public class TwitterServiceConfiguration(string xApiKey, string xApiKeySecret, string xAccessToken, string xAccessTokenSecret, string bearerToken)
    {
        private readonly string _xApiKey = xApiKey;
        private readonly string _xApiKeySecret = xApiKeySecret;
        private readonly string _xAccessToken = xAccessToken;
        private readonly string _xAccessTokenSecret = xAccessTokenSecret;
        private readonly string _bearerToken = bearerToken;
        public string XApiKey { get => _xApiKey; }
        public string XApiKeySecret { get => _xApiKeySecret; }
        public string XAccessToken { get => _xAccessToken; }
        public string XAccessTokenSecret { get => _xAccessTokenSecret; }
        public string BearerToken { get => _bearerToken; }

    }
}