namespace VietWay.Service.ThirdParty.ZaloPay
{
    public class ZaloPayServiceConfiguration(string zaloPayAppId, string zaloPayAppUser, string zaloPayKey1, string zaloPayKey2, string returnUrl)
    {
        private readonly string _zaloPayAppId = zaloPayAppId;
        private readonly string _zaloPayAppUser = zaloPayAppUser;
        private readonly string _zaloPayKey1 = zaloPayKey1;
        private readonly string _zaloPayKey2 = zaloPayKey2;
        private readonly string _returnUrl = returnUrl;
        public string ZaloPayAppId { get => _zaloPayAppId; }
        public string ZaloPayAppUser { get => _zaloPayAppUser; }
        public string ZaloPayKey1 { get => _zaloPayKey1; }
        public string ZaloPayKey2 { get => _zaloPayKey2; }
        public string ReturnUrl { get => _returnUrl; }
    }
}