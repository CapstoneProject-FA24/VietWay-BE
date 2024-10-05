using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Transaction = VietWay.Repository.EntityModel.Transaction;

namespace VietWay.Service.ThirdParty
{
    public class VnPayService(IConfiguration configuration) : IVnPayService
    {
        private readonly string _vnpHashSecret =
            configuration["VnPay:HashSecret"] ?? throw new Exception("Can not get vnp_HashSecret");
        private readonly string _vnpTmnCode =
            configuration["VnPay:TmnCode"] ?? throw new Exception("Can not get vnp_TmnCode");
        public string GetPaymentUrl(Transaction transaction, string userIpAddress)
        {
            const string vnpVersion = "2.1.0";
            const string vnpCommand = "pay";
            string vnpAmount = ((int)(transaction.Amount * 100)).ToString();
            string vnpCreateDate = TimeZoneInfo
                .ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh")).ToString("yyyyMMddHHmmss");
            const string vnpCurrCode = "VND";
            const string vnpLocale = "vn";
            string vnpOrderInfo = Uri.EscapeDataString($"Thanh toan tour gia {transaction.Amount} VND");
            const string vnpOrderType = "130005";
            string vnpReturnUrl = Uri.EscapeDataString("https://www.google.com");
            string vnpExpireDate = TimeZoneInfo
                .ConvertTime(DateTime.Now.AddHours(1), TimeZoneInfo.FindSystemTimeZoneById("Asia/Ho_Chi_Minh")).ToString("yyyyMMddHHmmss");
            string vnpTxnRef = transaction.TransactionId.ToString();

            string hashSource = $"vnp_Amount={vnpAmount}&" +
                                $"vnp_Command={vnpCommand}&" +
                                $"vnp_CreateDate={vnpCreateDate}&" +
                                $"vnp_CurrCode={vnpCurrCode}&" +
                                $"vnp_ExpireDate={vnpExpireDate}&" +
                                $"vnp_IpAddr={userIpAddress}&" +
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
        public bool VerifyTransaction(long amount, string bankCode, string bankTranNo, string cardType,
            string orderInfo, string payDate, string responseCode, string tmnCode, string transactionNo,
            string transactionStatus, string txnRef, string secureHash)
        {
            string hashSource = $"vnp_Amount={amount}&" +
                                $"vnp_BankCode={bankCode}&" +
                                $"vnp_BankTranNo={bankTranNo}&" +
                                $"vnp_CardType={cardType}&" +
                                $"vnp_OrderInfo={Uri.EscapeDataString(orderInfo)}&" +
                                $"vnp_PayDate={payDate}&" +
                                $"vnp_ResponseCode={responseCode}&" +
                                $"vnp_TmnCode={tmnCode}&" +
                                $"vnp_TransactionNo={transactionNo}&" +
                                $"vnp_TransactionStatus={transactionStatus}&" +
                                $"vnp_TxnRef={txnRef}";
            byte[] hashBytes =
                HMACSHA512.HashData(Encoding.UTF8.GetBytes(_vnpHashSecret), Encoding.UTF8.GetBytes(hashSource));
            string hashedSource = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hashedSource == secureHash;
        }
    }
}