using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.ThirdParty.PayOS
{
    public interface IPayOSService
    {
        public Task<string> CreatePaymentUrl(BookingPayment bookingPayment, string tourName);
        public bool VerifyTransaction(PayOSWebhookRequest payOSIPN);
    }
}
