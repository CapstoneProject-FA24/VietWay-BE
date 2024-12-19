using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IBookingService
    {
        public Task<string> BookTourAsync(Booking booking, int attempt = 0);
        public Task CancelBookingAsync(string customerId, string bookingId, string? reason, int attempt = 0);
        public Task<BookingDetailDTO?> GetBookingDetailAsync(string? customerId, string bookingId);
        Task<List<BookingHistoryDTO>> GetBookingHistoryAsync(string customerId, string bookingId);
        public Task<PaginatedList<BookingPreviewDTO>> GetCustomerBookingsAsync(string customerId,BookingStatus? bookingStatus, int pageSize, int pageIndex);
        public Task ConfirmTourChangeAsync(string customerId, string bookingId);
        public Task DenyTourChangeAsync(string customerId, string bookingId, int attempts = 0);
    }
}
