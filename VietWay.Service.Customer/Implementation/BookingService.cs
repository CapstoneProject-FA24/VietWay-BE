﻿using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.IdUtil;
using VietWay.Util.CustomExceptions;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.DateTimeUtil;
using Hangfire;
using VietWay.Service.Customer.Configuration;
using VietWay.Job.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class BookingService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper, 
        IBackgroundJobClient backgroundJobClient, BookingServiceConfiguration config) : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        private readonly int _pendingBookingExpireAfterMinutes = config.PendingBookingExpireAfterMinutes;

        public async Task<string> BookTourAsync(Booking booking)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Tour? tour = await _unitOfWork.TourRepository.Query()
                    .Include(x => x.TourPrices)
                    .SingleOrDefaultAsync(x => x.TourId == booking.TourId && x.Status == TourStatus.Opened && x.IsDeleted == false)
                    ?? throw new ResourceNotFoundException("Can not find any tour");
                bool isActiveBookingExisted = await _unitOfWork.BookingRepository.Query()
                    .AnyAsync(x => x.TourId == booking.TourId && x.CustomerId == booking.CustomerId && 
                        (x.Status == BookingStatus.Pending || x.Status == BookingStatus.Deposited || x.Status == BookingStatus.Paid));

                if (isActiveBookingExisted)
                {
                    throw new InvalidOperationException("Customer has already booked this tour");
                }
                if (tour.CurrentParticipant + booking.BookingTourists.Count > tour.MaxParticipant)
                {
                    throw new InvalidOperationException("Tour is full");
                }
                booking.BookingId = _idGenerator.GenerateId();
                foreach (BookingTourist tourist in booking.BookingTourists)
                {
                    tourist.TouristId = _idGenerator.GenerateId();
                    tourist.BookingId = booking.BookingId;
                    int age = CalculateAge(tourist.DateOfBirth, _timeZoneHelper.GetUTC7Now());
                    TourPrice? tourPrice = tour.TourPrices?.SingleOrDefault(x => x.AgeFrom <= age && age <= x.AgeTo);
                    if (tourPrice == null)
                    {
                        tourist.Price = tour.DefaultTouristPrice!.Value;
                        booking.TotalPrice += tourist.Price;
                    }
                    else
                    {
                        tourist.Price = tourPrice.Price;
                        booking.TotalPrice += tourist.Price;
                    }
                };
                booking.Status = BookingStatus.Pending;
                booking.CreatedAt = _timeZoneHelper.GetUTC7Now();
                tour.CurrentParticipant = tour.CurrentParticipant + booking.NumberOfParticipants;
                await _unitOfWork.BookingRepository.CreateAsync(booking);
                await _unitOfWork.TourRepository.UpdateAsync(tour);

                await _unitOfWork.EntityHistoryRepository.CreateAsync(new()
                {
                    Id = _idGenerator.GenerateId(),
                    Action = EntityModifyAction.Create,
                    EntityId = booking.BookingId,
                    EntityType = EntityType.Booking,
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    ModifiedBy = booking.CustomerId,
                    ModifierRole = UserRole.Customer,
                });
                await _unitOfWork.CommitTransactionAsync();
                _backgroundJobClient.Schedule<IBookingJob>(
                    x => x.CheckBookingForExpirationAsync(booking.BookingId), 
                    DateTime.Now.AddMinutes(_pendingBookingExpireAfterMinutes));
                _backgroundJobClient.Enqueue<IEmailJob>( x => 
                    x.SendBookingConfirmationEmail(booking.BookingId, booking.CreatedAt.AddMinutes(_pendingBookingExpireAfterMinutes)));
                return booking.BookingId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CancelBookingAsync(string customerId, string bookingId, string? reason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Booking booking = _unitOfWork.BookingRepository.Query()
                    .Include(x => x.Tour.TourRefundPolicies)
                    .Include(x => x.BookingRefunds)
                    .SingleOrDefault(x => x.BookingId.Equals(bookingId) && x.CustomerId.Equals(customerId))
                    ?? throw new ResourceNotFoundException("Booking not found");
                if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Deposited && booking.Status != BookingStatus.Paid)
                {
                    throw new InvalidOperationException("You cannot cancel this booking");
                }
                int oldStatus = (int)booking.Status;
                booking.Status = BookingStatus.Cancelled;
                booking.Tour.CurrentParticipant -= booking.NumberOfParticipants;

                decimal refundPercentCost = booking.Tour.TourRefundPolicies?
                    .Where(x => x.CancelBefore > _timeZoneHelper.GetUTC7Now())
                    .OrderByDescending(x => x.RefundPercent)
                    .Select(x => x.RefundPercent)
                    .FirstOrDefault() ?? 100;

                decimal refundAmount = booking.PaidAmount - (booking.TotalPrice * refundPercentCost / 100);

                if (refundAmount > 0)
                {
                    await _unitOfWork.BookingRefundRepository.CreateAsync(new BookingRefund()
                    {
                        RefundId = _idGenerator.GenerateId(),
                        BookingId = booking.BookingId,
                        BankCode = null,
                        BankTransactionNumber = null,
                        CreatedAt = _timeZoneHelper.GetUTC7Now(),
                        RefundAmount = refundAmount,
                        RefundDate = null,
                        RefundNote = null,
                        RefundReason = reason,
                        RefundStatus = RefundStatus.Pending,
                    });
                }
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                string entityHistoryId = _idGenerator.GenerateId();
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
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<BookingDetailDTO?> GetBookingDetailAsync(string? customerId, string bookingId)
        {
            return await _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.BookingId == bookingId && x.CustomerId == customerId)
                .Include(x => x.Tour.TourTemplate.TourTemplateImages)
                .Include(x => x.BookingTourists)
                .Select(x => new BookingDetailDTO()
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
                    DepositPercent = x.Tour.DepositPercent,
                    PaymentDeadline = x.Tour.PaymentDeadline,
                    Status = x.Status,
                    TotalPrice = x.TotalPrice,
                    TourId = x.TourId,
                    DurationName = x.Tour.TourTemplate.TourDuration.DurationName,
                    NumberOfDay = x.Tour.TourTemplate.TourDuration.NumberOfDay,
                    TourName = x.Tour.TourTemplate.TourName,
                    CreatedOn = x.CreatedAt,
                    Note = x.Note,
                    PaidAmount = x.PaidAmount,
                    Participants = x.BookingTourists.Select(y => new TourParticipantDTO()
                    {
                        DateOfBirth = y.DateOfBirth,
                        FullName = y.FullName,
                        Gender = y.Gender,
                        PhoneNumber = y.PhoneNumber,
                        Price = y.Price,
                    }).ToList(),
                    Code = x.Tour.TourTemplate.Code,
                }).SingleOrDefaultAsync();
        }

        public async Task<PaginatedList<BookingPreviewDTO>> GetCustomerBookingsAsync(string customerId,BookingStatus? bookingStatus, int pageSize, int pageIndex)
        {
            IQueryable<Booking> query = _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.Tour.TourTemplate.TourTemplateImages)
                .Include(x => x.TourReview)
                .OrderByDescending(x => x.CreatedAt);
            if (bookingStatus.HasValue)
            {
                query = query.Where(x => x.Status == bookingStatus);
            }
                int count = await query.CountAsync();
            List<BookingPreviewDTO> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new BookingPreviewDTO()
                {
                    BookingId = x.BookingId,
                    TourId = x.TourId,
                    CustomerId = x.CustomerId,
                    NumberOfParticipants = x.NumberOfParticipants,
                    TotalPrice = x.TotalPrice,
                    Status = x.Status,
                    CreatedOn = x.CreatedAt,
                    TourName = x.Tour!.TourTemplate!.TourName,
                    ImageUrl = x.Tour!.TourTemplate!.TourTemplateImages.Select(x=>x.ImageUrl).First(),
                    Code = x.Tour.TourTemplate.Code,
                    StartDate = x.Tour!.StartDate!.Value,
                    IsReviewed = x.TourReview != null,
                }).ToListAsync();
            return new PaginatedList<BookingPreviewDTO>
            {
                Total = count,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = items
            };
        }

        private static int CalculateAge(DateTime birthDay, DateTime currentDate)
        {
            int age = currentDate.Year - birthDay.Year;
            if (currentDate < birthDay.AddYears(age))
            {
                age--;
            }
            return age;
        }

        public async Task<List<BookingHistoryDTO>> GetBookingHistoryAsync(string customerId, string bookingId)
        {
            bool isBookingExist = _unitOfWork.BookingRepository.Query()
                .Any(x => x.BookingId == bookingId && x.CustomerId == customerId);
            if (!isBookingExist)
            {
                throw new ResourceNotFoundException("Booking not found");
            }
            List<BookingHistoryDTO> result = await _unitOfWork.EntityHistoryRepository.Query()
                .Where(x => x.EntityType == EntityType.Booking && x.EntityId == bookingId)
                .OrderByDescending(x => x.Timestamp)
                .Select(x => new BookingHistoryDTO()
                {
                    NewStatus = x.StatusHistory.NewStatus,
                    OldStatus = x.StatusHistory.OldStatus,
                    Reason = x.Reason,
                    Timestamp = x.Timestamp,
                    ModifierRole = x.ModifierRole,
                    Action = x.Action,
                }).ToListAsync();
            return result;
        }
    }
}
