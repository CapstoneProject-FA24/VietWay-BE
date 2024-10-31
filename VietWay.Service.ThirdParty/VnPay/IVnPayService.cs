using VietWay.Repository.EntityModel;

namespace VietWay.Service.ThirdParty.VnPay
{
    public interface IVnPayService
    {
        public string GetPaymentUrl(BookingPayment bookingPayment, string userIpAddress);
        public bool VerifyTransaction(VnPayIPN vnPayIPN);
    }
}
