using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface IBookingPaymentService
    {
        public Task<BookingPayment?> GetBookingPaymentAsync(string id);
        public Task<string> GetVnPayBookingPaymentUrl(string bookingId, string customerId, string ipAddress);
        public Task HandleVnPayIPN(VnPayIPN vnPayIPN);
    }
}
