﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IBookingService
    {
        public Task CreateBookingAsync(Booking tourBooking);
        public Task<TourBookingInfoDTO?> GetTourBookingInfoAsync(string bookingId, string customerId);
        public Task<(int count, List<TourBookingPreviewDTO> items)> GetCustomerBookedToursAsync(string customerId, int pageSize, int pageIndex);
        public Task CancelBookingAsync(string bookingId, string accountId, string? reason);
        public Task<BookingDetailDTO> GetBookingByIdAsync(string bookingId);
        public Task<(int count, List<BookingPreviewDTO>)> GetBookingsAsync(BookingStatus? bookingStatus, int pageCount, int pageIndex, string? bookingIdSearch, string? contactNameSearch, string? contactPhoneSearch, string? tourIdSearch);
        public Task CreateRefundTransactionAsync(string accountId, string bookingId, BookingPayment bookingPayment);
    }
}
