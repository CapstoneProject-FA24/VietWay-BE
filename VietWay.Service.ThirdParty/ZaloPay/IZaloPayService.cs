using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.ThirdParty.ZaloPay
{
    public interface IZaloPayService
    {
        public Task<string> GetPaymentUrl(BookingPayment bookingPayment, int expireAfterMinutes);
        public Task<ZaloPayResponse> VerifyTransactionLocal(ZaloPayCallback zaloPayCallback);
        public Dictionary<string, object> VerifyTransaction(CallbackData data);
    }
}
