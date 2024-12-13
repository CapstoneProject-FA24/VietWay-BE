using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Net.payOS;
using Net.payOS.Types;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.ThirdParty.PayOS
{
    public class PayOSService : IPayOSService
    {
        private readonly Net.payOS.PayOS _payOS;
        private readonly string _returnUrl;
        public PayOSService(PayOSConfiguration payOSConfig)
        {
            _payOS = new Net.payOS.PayOS(payOSConfig.ClientId, payOSConfig.ApiKey, payOSConfig.ChecksumKey);
            _returnUrl = payOSConfig.ReturnUrl;
        }
        public async Task<string> CreatePaymentUrl(BookingPayment bookingPayment, string tourName)
        {
            List<ItemData> itemDatas = 
                [
                    new(tourName, 1, (int)bookingPayment.Amount)
                ];
            PaymentData paymentData = new(
                orderCode: long.Parse(bookingPayment.PaymentId),
                amount: (int)bookingPayment.Amount,
                description: tourName,
                items: itemDatas,
                expiredAt: (int)DateTime.Now.AddMinutes(15).Subtract(new DateTime(1970,1,1)).TotalSeconds,
                returnUrl: _returnUrl,
                cancelUrl: _returnUrl);
            CreatePaymentResult createPaymentResult = await _payOS.createPaymentLink(paymentData);
            return createPaymentResult.checkoutUrl;
        }

        public bool VerifyTransaction(PayOSWebhookRequest webhookRequest)
        {
            try
            {
                _ = _payOS.verifyPaymentWebhookData(webhookRequest);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
