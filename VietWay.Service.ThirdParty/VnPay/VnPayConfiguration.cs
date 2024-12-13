namespace VietWay.Service.ThirdParty.VnPay
{
    public class VnPayConfiguration
    {
        public required string VnpHashSecret { get; set; }
        public required string VnpTmnCode { get; set; }
        public required string ReturnUrl { get; set; }
    }
}