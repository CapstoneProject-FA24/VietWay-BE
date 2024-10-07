using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface ITourBookingService
    {
        public Task CreateBookingAsync(TourBooking tourBooking);
        public Task<TourBookingInfoDTO?> GetTourBookingInfoAsync(string bookingId);
    }
}
