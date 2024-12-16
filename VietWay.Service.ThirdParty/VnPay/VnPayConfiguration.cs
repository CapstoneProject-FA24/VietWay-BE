namespace VietWay.Service.ThirdParty.VnPay
{
    public class VnPayConfiguration(string vnpHashSecret, string vnpTmnCode, string returnUrl)
    {
        private readonly string _vnpHashSecret = vnpHashSecret;
        private readonly string _vnpTmnCode = vnpTmnCode;
        private readonly string _returnUrl = returnUrl;
        public string VnpHashSecret { get => _vnpHashSecret; }
        public string VnpTmnCode { get => _vnpTmnCode; }
        public string VnpReturnUrl { get => _returnUrl; }
    }
}