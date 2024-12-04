using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface ITourService
    {
        public Task<TourDetailDTO?> GetTourByIdAsync(string tourId);
        Task<List<TourPreviewDTO>> GetAllToursByTemplateIdsAsync(string tourTemplateId);
        Task<TourDetailDTO?> GetTourDetailByBookingIdAsync(string customerId, string bookingId);
    }
}
