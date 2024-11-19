namespace VietWay.Service.ThirdParty.Twitter
{
    public class TwitterServiceConfiguration
    {
        public required string XApiKey { get; set; }
        public required string XApiKeySecret { get; set; }
        public required string XAccessToken { get; set; }
        public required string XAccessTokenSecret { get; set; }
        public required string BearerToken { get; set; }

    }
}