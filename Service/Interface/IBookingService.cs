using System;
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
        public Task CancelBookingAsync(string bookingId, string accountId, string? reason);
        public Task<BookingDetailDTO> GetBookingByIdAsync(string bookingId);
        public Task<(int count, List<BookingPreviewDTO>)> GetBookingsAsync(BookingStatus? bookingStatus, int pageCount, int pageIndex, string? bookingIdSearch, string? contactNameSearch, string? contactPhoneSearch, string? tourIdSearch);
        public Task CreateRefundTransactionAsync(string accountId, string bookingId, BookingPayment bookingPayment);
        public Task ChangeBookingTourAsync(string accountId, string bookingId, string newTourId, string reason);
    }
}
