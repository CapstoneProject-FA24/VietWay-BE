using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Management.ThirdParty
{
    public interface IVnPayService
    {
        public string GetPaymentUrl(BookingPayment bookingPayment, string userIpAddress);
        public bool VerifyTransaction(VnPayIPN vnPayIPN);
    }
}
