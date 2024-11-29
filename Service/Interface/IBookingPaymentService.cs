using VietWay.Repository.EntityModel;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Service.ThirdParty.ZaloPay;

namespace VietWay.Service.Management.Interface
{
    public interface IBookingPaymentService
    {
        public Task<BookingPayment?> GetBookingPaymentAsync(string id);
        public Task<string> GetVnPayBookingPaymentUrl(string bookingId, string customerId, string ipAddress);
        public Task HandleVnPayIPN(VnPayIPN vnPayIPN);
        public Task HandleZaloPayCallbackLocal(ZaloPayCallback zaloPayCallback);
        public Task<Dictionary<string, object>> HandleZaloPayCallback(CallbackData data);
    }
}
