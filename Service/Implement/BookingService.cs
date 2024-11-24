using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class BookingService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper) : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task CreateBookingAsync(Booking tourBooking)
        {
            await _unitOfWork.BookingRepository.CreateAsync(tourBooking);
        }
        public async Task CustomerCancelBookingAsync(string bookingId, string customerId, string? reason)
        {
            Booking? booking = await _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.BookingId == bookingId && x.CustomerId == customerId)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("Booking not found");
            if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
            {
                throw new InvalidOperationException("Cannot cancel booking that is not pending or confirmed");
            }
            int oldStatus = (int)booking.Status;
            booking.Status = BookingStatus.Cancelled;
            string entityHistoryId = _idGenerator.GenerateId();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.EntityStatusHistoryRepository.CreateAsync(new EntityStatusHistory()
                {
                    Id = entityHistoryId,
                    OldStatus = oldStatus,
                    NewStatus = (int)booking.Status,
                    EntityHistory = new EntityHistory()
                    {
                        Id = entityHistoryId,
                        Action = EntityModifyAction.ChangeStatus,
                        EntityId = bookingId,
                        EntityType = EntityType.Booking,
                        Timestamp = _timeZoneHelper.GetUTC7Now(),
                        ModifiedBy = customerId,
                        ModifierRole = UserRole.Customer,
                        Reason = reason,
                    }
                });
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<(int count, List<TourBookingPreviewDTO> items)> GetCustomerBookedToursAsync(string customerId, int pageSize, int pageIndex)
        {
            IQueryable<Booking> query = _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.Tour.TourTemplate.TourTemplateImages)
                .OrderByDescending(x => x.CreatedAt);
            int count = await query.CountAsync();
            List<TourBookingPreviewDTO> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new TourBookingPreviewDTO()
                {
                    BookingId = x.BookingId,
                    TourId = x.TourId,
                    CustomerId = x.CustomerId,
                    NumberOfParticipants = x.NumberOfParticipants,
                    TotalPrice = x.TotalPrice,
                    Status = x.Status,
                    CreatedOn = x.CreatedAt,
                    TourName = x.Tour.TourTemplate.TourName,
                    ImageUrl = x.Tour.TourTemplate.TourTemplateImages.First().ImageUrl,
                    Code = x.Tour.TourTemplate.Code
                }).ToListAsync();
            return (count, items);
        }



        public async Task<TourBookingInfoDTO?> GetTourBookingInfoAsync(string bookingId, string customerId)
        {
            return await _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.BookingId == bookingId && x.CustomerId == customerId)
                .Include(x => x.Tour.TourTemplate.TourTemplateImages)
                .Include(x => x.BookingTourists)
                .Select(x => new TourBookingInfoDTO()
                {
                    BookingId = x.BookingId,
                    ContactAddress = x.ContactAddress,
                    ContactEmail = x.ContactEmail,
                    ContactFullName = x.ContactFullName,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                    CustomerId = x.CustomerId,
                    ImageUrl = x.Tour.TourTemplate.TourTemplateImages.First().ImageUrl,
                    NumberOfParticipants = x.NumberOfParticipants,
                    StartDate = (DateTime)x.Tour.StartDate,
                    StartLocation = x.Tour.StartLocation,
                    Status = x.Status,
                    TotalPrice = x.TotalPrice,
                    TourId = x.TourId,
                    TourName = x.Tour.TourTemplate.TourName,
                    CreatedOn = x.CreatedAt,
                    Note = x.Note,
                    Participants = x.BookingTourists.Select(y => new TourParticipantDTO()
                    {
                        DateOfBirth = y.DateOfBirth,
                        FullName = y.FullName,
                        Gender = y.Gender,
                        PhoneNumber = y.PhoneNumber,
                    }).ToList(),
                    Code = x.Tour.TourTemplate.Code
                }).SingleOrDefaultAsync();
        }

        public async Task<(int count, List<BookingPreviewDTO>)> GetBookingsAsync(BookingStatus? bookingStatus, int pageCount, int pageIndex, string? bookingIdSearch, string? contactNameSearch, string? contactPhoneSearc)
        {
            IQueryable<Booking> query = _unitOfWork
                .BookingRepository
                .Query()
                .Include(x => x.Tour)
                .ThenInclude(x => x.TourTemplate)
                .OrderByDescending(x => x.CreatedAt);
            if (bookingStatus.HasValue)
            {
                query = query.Where(x => x.Status == bookingStatus);
            }
            if (!string.IsNullOrWhiteSpace(bookingIdSearch))
            {
                query = query.Where(x => x.BookingId.Contains(bookingIdSearch));
            }
            if (!string.IsNullOrWhiteSpace(contactNameSearch))
            {
                query = query.Where(x => x.ContactFullName.Contains(contactNameSearch));
            }
            if (!string.IsNullOrWhiteSpace(contactPhoneSearc))
            {
                query = query.Where(x => x.ContactPhoneNumber.Contains(contactPhoneSearc));
            }
            int count = await query.CountAsync();
            List<BookingPreviewDTO> items = await query
                .Skip((pageIndex - 1) * pageCount)
                .Take(pageCount)
                .Select(x => new BookingPreviewDTO()
                {
                    BookingId = x.BookingId,
                    TourName = x.Tour.TourTemplate.TourName,
                    TourCode = x.Tour.TourTemplate.Code,
                    StartDate = x.Tour.StartDate,
                    StartLocation = x.Tour.StartLocation,
                    CreatedAt = x.CreatedAt,
                    ContactFullName = x.ContactFullName,
                    ContactEmail = x.ContactEmail,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                    TotalPrice = x.TotalPrice,
                    NumberOfParticipants = x.NumberOfParticipants,
                    Status = x.Status
                }).ToListAsync();
            return (count, items);
        }

        public async Task CreateRefundTransactionAsync(string managerId, string bookingId, BookingPayment bookingPayment)
        {
            Booking booking = await _unitOfWork
                .BookingRepository.Query().SingleOrDefaultAsync(x => x.BookingId == bookingId)
                ?? throw new ResourceNotFoundException("Booking not found");

            if (booking.Status != BookingStatus.PendingRefund)
            {
                throw new InvalidOperationException("The booking is not in a pending refund state and cannot be refunded.");
            }

            EntityStatusHistory entityStatusHistory = await _unitOfWork.EntityStatusHistoryRepository.Query()
                .Include(x => x.EntityHistory)
                .SingleOrDefaultAsync(x => x.EntityHistory.EntityId == bookingId && x.EntityHistory.Action == EntityModifyAction.ChangeStatus && x.NewStatus == (int)BookingStatus.PendingRefund)
                ?? throw new ResourceNotFoundException("Entity status history not found");

            DateTime cancelTime = entityStatusHistory.EntityHistory.Timestamp;

            TourRefundPolicy tourRefundPolicy = await _unitOfWork.TourRefundPolicyRepository.Query()
                .OrderBy(x => x.CancelBefore)
                .FirstOrDefaultAsync(x => x.CancelBefore > cancelTime && x.TourId == booking.TourId)
                ?? throw new ResourceNotFoundException("Refund policy not found");


            decimal refundAmount = tourRefundPolicy.RefundPercent * booking.TotalPrice / 100;
            try
            {
                bookingPayment.PaymentId ??= _idGenerator.GenerateId();
                bookingPayment.BookingId = bookingId;
                bookingPayment.Status = PaymentStatus.Refunded;
                bookingPayment.CreateAt = _timeZoneHelper.GetUTC7Now();
                bookingPayment.PayTime = _timeZoneHelper.GetUTC7Now();
                bookingPayment.Amount = refundAmount;

                int oldStatus = (int)booking.Status;
                booking.Status = BookingStatus.Refunded;

                string entityHistoryId = _idGenerator.GenerateId();

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                await _unitOfWork.BookingPaymentRepository.CreateAsync(bookingPayment);
                await _unitOfWork.EntityStatusHistoryRepository.CreateAsync(new EntityStatusHistory()
                {
                    Id = entityHistoryId,
                    OldStatus = oldStatus,
                    NewStatus = (int)booking.Status,
                    EntityHistory = new EntityHistory()
                    {
                        Id = entityHistoryId,
                        Action = EntityModifyAction.ChangeStatus,
                        EntityId = bookingId,
                        EntityType = EntityType.Booking,
                        Timestamp = _timeZoneHelper.GetUTC7Now(),
                        ModifiedBy = managerId,
                        ModifierRole = UserRole.Customer,
                        Reason = "Manager refund booking",
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