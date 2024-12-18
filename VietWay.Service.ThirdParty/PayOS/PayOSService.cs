using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS.Types;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.ThirdParty.PayOS
{
    public class PayOSService(PayOSConfiguration config) : IPayOSService
    {
        private readonly Net.payOS.PayOS _payOS = new(config.ClientId, config.ApiKey, config.ChecksumKey);
        private readonly string _returnUrl = config.ReturnUrl;

        public async Task<string> CreatePaymentUrl(BookingPayment bookingPayment, string tourName, int expireAfterMinutes)
        {
            List<ItemData> items =
                [
                    new ItemData(name: tourName, quantity: 1, price: (int)bookingPayment.Amount)
                ];
            PaymentData paymentData = new(
                orderCode: int.Parse(bookingPayment.ThirdPartyTransactionNumber!),
                amount: (int)bookingPayment.Amount,
                description: "",
                items: items,
                returnUrl: $"{_returnUrl}/{bookingPayment.BookingId}",
                cancelUrl: $"{_returnUrl}/{bookingPayment.BookingId}",
                expiredAt: (int)(DateTime.UtcNow.AddMinutes(expireAfterMinutes) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
            );
            CreatePaymentResult result = await _payOS.createPaymentLink(paymentData);
            return result.checkoutUrl;
        }

        public bool VerifyWebhook(PayOSWebhookRequest request)
        {
            try
            {
                WebhookData data = new(
                    request.Data.OrderCode, 
                    request.Data.Amount,
                    request.Data.Description,
                    request.Data.AccountNumber,
                    request.Data.Reference,
                    request.Data.TransactionDateTime,
                    request.Data.Currency,
                    request.Data.PaymentLinkId,
                    request.Data.Code,
                    request.Data.Desc,
                    request.Data.CounterAccountBankId,
                    request.Data.CounterAccountBankName,
                    request.Data.CounterAccountName,
                    request.Data.CounterAccountNumber,
                    request.Data.VirtualAccountName,
                    request.Data.VirtualAccountNumber);
                WebhookType webhookType = new(request.Code,request.Desc,request.Success,data,request.Signature);
                _ = _payOS.verifyPaymentWebhookData(webhookType);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
