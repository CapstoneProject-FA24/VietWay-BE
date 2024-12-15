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
        private readonly Net.payOS.PayOS _payOS = new(config.CLientId, config.ApiKey, config.ChecksumKey);
        private readonly string _returnUrl = config.ReturnUrl;

        public async Task<string> CreatePaymentUrl(BookingPayment bookingPayment, string tourName)
        {
            List<ItemData> items =
                [
                    new ItemData(name: tourName, quantity: 1, price: (int)bookingPayment.Amount)
                ];
            PaymentData paymentData = new(
                orderCode: long.Parse(bookingPayment.PaymentId),
                amount: (int)bookingPayment.Amount,
                description: tourName,
                items: items,
                returnUrl: $"_returnUrl/{bookingPayment.PaymentId}",
                cancelUrl: $"_returnUrl/{bookingPayment.PaymentId}",
                expiredAt: (int)(DateTime.UtcNow.AddMinutes(15) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds
            );
            CreatePaymentResult result = await _payOS.createPaymentLink(paymentData);
            return result.checkoutUrl;
        }

        public bool VerifyWebhook(PayOSWebhookRequest request)
        {
            try
            {
                _ = _payOS.verifyPaymentWebhookData(request);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
