using VietWay.Repository.EntityModel;

namespace VietWay.Service.ThirdParty.VnPay
{
    public interface IVnPayService
    {
        public string GetPaymentUrl(BookingPayment bookingPayment, string userIpAddress, int expireAfterMinutes);
        public bool VerifyTransaction(VnPayIPN vnPayIPN);
    }
}
