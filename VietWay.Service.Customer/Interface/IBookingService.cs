﻿using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IBookingService
    {
        public Task<string> BookTourAsync(Booking booking);
        public Task CancelBookingAsync(string customerId, string bookingId, string? reason);
        public Task<BookingDetailDTO?> GetBookingDetailAsync(string? customerId, string bookingId);
        public Task<PaginatedList<BookingPreviewDTO>> GetCustomerBookingsAsync(string customerId,BookingStatus? bookingStatus, int pageSize, int pageIndex);
    }
}
