using System.Security.Cryptography;
using System.Text;
using VietWay.Repository.EntityModel;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.ThirdParty.VnPay
{
    public class VnPayService(ITimeZoneHelper timeZoneHelper, VnPayConfiguration config) : IVnPayService
    {
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly string _vnpHashSecret = config.VnpHashSecret;
        private readonly string _vnpTmnCode = config.VnpTmnCode;
        private readonly string _returnUrl = config.VnpReturnUrl;

        public string GetPaymentUrl(BookingPayment payment, string userIpAddress, int expireAfterMinutes)
        {
            const string vnpVersion = "2.1.0";
            const string vnpCommand = "pay";
            string vnpAmount = ((int)(payment.Amount * 100)).ToString();
            string vnpCreateDate = _timeZoneHelper.GetUTC7Now().ToString("yyyyMMddHHmmss");
            const string vnpCurrCode = "VND";
            const string vnpLocale = "vn";
            string vnpOrderInfo = Uri.EscapeDataString($"Thanh+toan+tour+gia+{payment.Amount}+VND");
            const string vnpOrderType = "130005";
            string vnpReturnUrl = Uri.EscapeDataString($"{_returnUrl}/{payment.BookingId}");
            string vnpExpireDate = _timeZoneHelper.GetUTC7Now().AddMinutes(expireAfterMinutes).ToString("yyyyMMddHHmmss");
            string vnpTxnRef = payment.PaymentId;
            string ipAddress = Uri.EscapeDataString(userIpAddress);
            string hashSource = $"vnp_Amount={vnpAmount}&" +
                                $"vnp_Command={vnpCommand}&" +
                                $"vnp_CreateDate={vnpCreateDate}&" +
                                $"vnp_CurrCode={vnpCurrCode}&" +
                                $"vnp_ExpireDate={vnpExpireDate}&" +
                                $"vnp_IpAddr={ipAddress}&" +
                                $"vnp_Locale={vnpLocale}&" +
                                $"vnp_OrderInfo={vnpOrderInfo}&" +
                                $"vnp_OrderType={vnpOrderType}&" +
                                $"vnp_ReturnUrl={vnpReturnUrl}&" +
                                $"vnp_TmnCode={_vnpTmnCode}&" +
                                $"vnp_TxnRef={vnpTxnRef}&" +
                                $"vnp_Version={vnpVersion}";
            byte[] hashBytes =
                HMACSHA512.HashData(Encoding.UTF8.GetBytes(_vnpHashSecret), Encoding.UTF8.GetBytes(hashSource));
            string vnpSecureHash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            // Construct payment URL
            return $"https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?{hashSource}&vnp_SecureHash={vnpSecureHash}";
        }
        public bool VerifyTransaction(VnPayIPN vnPayIPN)
        {
            string hashSource = $"vnp_Amount={vnPayIPN.Amount}&" +
                                $"vnp_BankCode={vnPayIPN.BankCode}&" +
                                $"vnp_BankTranNo={vnPayIPN.BankTranNo}&" +
                                $"vnp_CardType={vnPayIPN.CardType}&" +
                                $"vnp_OrderInfo={Uri.EscapeDataString(vnPayIPN.OrderInfo)}&" +
                                $"vnp_PayDate={vnPayIPN.PayDate}&" +
                                $"vnp_ResponseCode={vnPayIPN.ResponseCode}&" +
                                $"vnp_TmnCode={vnPayIPN.TmnCode}&" +
                                $"vnp_TransactionNo={vnPayIPN.TransactionNo}&" +
                                $"vnp_TransactionStatus={vnPayIPN.TransactionStatus}&" +
                                $"vnp_TxnRef={vnPayIPN.TxnRef}";
            byte[] hashBytes =
                HMACSHA512.HashData(Encoding.UTF8.GetBytes(_vnpHashSecret), Encoding.UTF8.GetBytes(hashSource));
            string hashedSource = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashedSource == vnPayIPN.SecureHash;
        }
    }
}