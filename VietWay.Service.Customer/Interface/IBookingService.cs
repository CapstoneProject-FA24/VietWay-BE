using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IBookingService
    {
        public Task BookTourAsync(Booking booking);
        public Task CancelBookingAsync(string customerId, string bookingId);
        public Task<BookingDetailDTO?> GetBookingDetailAsync(string? customerId, string bookingId);
        public Task<(int count, List<BookingPreviewDTO> items)> GetCustomerBookingsAsync(string customerId, int pageSize, int pageIndex);
    }
}
