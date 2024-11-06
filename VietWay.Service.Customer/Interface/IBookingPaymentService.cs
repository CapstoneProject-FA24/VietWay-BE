using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IBookingPaymentService
    {
        Task<(int count, List<BookingPaymentDTO> items)> GetBookingPaymentsAsync(string customerId, string bookingId, int pageSize, int pageIndex);
        Task<(int count, List<BookingPaymentDTO> items)> GetAllCustomerBookingPaymentsAsync(string customerId, int pageSize, int pageIndex);
        public Task<string> GetBookingPaymentUrl(PaymentMethod paymentMethod, string bookingId, string customerId, string ipAddress);
    }
}
