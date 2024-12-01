using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IBookingRefundService
    {
        public Task<PaginatedList<BookingRefundDTO>> GetBookingRefundAsync(string? bookingId, RefundStatus? refundStatus, int pageSize, int pageIndex);
        public Task UpdateBookingRefundInfo(string accountId, string refundId, BookingRefund bookingRefund);
    }
}
