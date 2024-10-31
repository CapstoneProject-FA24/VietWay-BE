using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourBookingService
    {
        public Task CreateBookingAsync(Booking tourBooking);
        public Task<TourBookingInfoDTO?> GetTourBookingInfoAsync(string bookingId, string customerId);
        public Task<(int count, List<TourBookingPreviewDTO> items)> GetCustomerBookedToursAsync(string customerId, int pageSize, int pageIndex);
        public Task CustomerCancelBookingAsync(string bookingId, string customerId, string? reason);
    }
}
