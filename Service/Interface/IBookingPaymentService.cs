using VietWay.Repository.EntityModel;
using VietWay.Service.ThirdParty.VnPay;

namespace VietWay.Service.Management.Interface
{
    public interface IBookingPaymentService
    {
        public Task<BookingPayment?> GetBookingPaymentAsync(string id);
        public Task<string> GetVnPayBookingPaymentUrl(string bookingId, string customerId, string ipAddress);
        public Task HandleVnPayIPN(VnPayIPN vnPayIPN);
    }
}
