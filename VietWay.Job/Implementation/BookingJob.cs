using Microsoft.EntityFrameworkCore;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Job.Implementation
{
    public class BookingJob(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper, IIdGenerator idGenetator) : IBookingJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IIdGenerator _idGenerator = idGenetator;
        public async Task CheckBookingForExpirationAsync(string bookingId)
        {
            Booking? booking = await _unitOfWork.BookingRepository
                .Query()
                .Include(x => x.Tour.TourRefundPolicies)
                .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId) && BookingStatus.Pending == x.Status);
            if (null == booking)
            {
                return;
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                booking.Status = BookingStatus.Cancelled;
                booking.Tour!.CurrentParticipant -= booking.NumberOfParticipants;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                if (booking.PaidAmount > 0)
                {
                    decimal refundPercentCost = booking.Tour.TourRefundPolicies?
                        .Where(x => x.CancelBefore > _timeZoneHelper.GetUTC7Now())
                        .OrderBy(x => x.CancelBefore)
                        .Select(x => x.RefundPercent)
                        .FirstOrDefault() ?? 100;

                    decimal refundAmount = booking.PaidAmount - (booking.TotalPrice * refundPercentCost / 100);
                    if (refundAmount > 0)
                    {
                        await _unitOfWork.BookingRefundRepository.CreateAsync(new BookingRefund
                        {
                            RefundId = Guid.NewGuid().ToString(),
                            BookingId = booking.BookingId,
                            RefundAmount = refundAmount,
                            RefundStatus = RefundStatus.Pending,
                            CreatedAt = _timeZoneHelper.GetUTC7Now()
                        });
                    }
                }
                string newHistoryId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory
                {
                    EntityId = booking.BookingId,
                    EntityType = EntityType.Booking,
                    Action = EntityModifyAction.ChangeStatus,
                    Id = newHistoryId,
                    ModifiedBy = "",
                    ModifierRole = UserRole.Admin,
                    Reason = "Booking expired",
                    StatusHistory =  new()
                    {
                        Id = newHistoryId,
                        NewStatus = (int)BookingStatus.Cancelled,
                        OldStatus = (int)BookingStatus.Pending,
                    }
                });
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
