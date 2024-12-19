using Google.Api.Gax;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using VietWay.Job.Interface;
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
    public class BookingService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper, IBackgroundJobClient backgroundJobClient) : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        public async Task CancelBookingAsync(string bookingId, string accountId, string? reason, int attempts = 0)
        {
            try
            {
                Account account = await _unitOfWork.AccountRepository.Query().SingleOrDefaultAsync(x => x.AccountId == accountId);
                if (account.Role != UserRole.Manager && account.Role != UserRole.Staff)
                {
                    throw new UnauthorizedException("UNAUTHORIZED");
                }

                Booking booking = await _unitOfWork.BookingRepository.Query()
                    .Include(x => x.Tour)
                    .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId))
                    ?? throw new ResourceNotFoundException("NOT_EXIST_BOOKING");
                if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Deposited && booking.Status != BookingStatus.Paid)
                {
                    throw new InvalidActionException("INVALID_ACTION_BOOKING_CANCEL");
                }
                int oldStatus = (int)booking.Status;
                booking.Status = BookingStatus.Cancelled;

                booking.Tour.CurrentParticipant -= booking.NumberOfParticipants;
                string entityHistoryId = _idGenerator.GenerateId();

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
                        ModifiedBy = accountId,
                        ModifierRole = account.Role,
                        Reason = reason,
                    }
                });

                if (booking.PaidAmount > 0)
                {
                    await _unitOfWork.BookingRefundRepository.CreateAsync(new BookingRefund()
                    {
                        RefundId = _idGenerator.GenerateId(),
                        BookingId = booking.BookingId,
                        BankCode = null,
                        BankTransactionNumber = null,
                        CreatedAt = _timeZoneHelper.GetUTC7Now(),
                        RefundAmount = booking.PaidAmount,
                        RefundDate = null,
                        RefundNote = null,
                        RefundReason = reason,
                        RefundStatus = RefundStatus.Pending,
                    });
                }
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                await _unitOfWork.CommitTransactionAsync();
                _backgroundJobClient.Enqueue<IEmailJob>(x=>x.SendSystemCancellationEmail(bookingId, reason));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (attempts < 3)
                {
                    await CancelBookingAsync(bookingId, accountId, reason, attempts + 1);
                }
                else
                {
                    throw new ServerErrorException("DATABASE_CONCURRENT_ERROR");
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<BookingDetailDTO?> GetBookingByIdAsync(string bookingId)
        {
            BookingDetailDTO? result = await _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.BookingId == bookingId)
                .Select(x => new BookingDetailDTO
                {
                    BookingId = x.BookingId,
                    TourId = x.TourId,
                    TourName = x.Tour.TourTemplate.TourName,
                    TourCode = x.Tour.TourTemplate.Code,
                    StartDate = x.Tour.StartDate,
                    StartLocation = x.Tour.StartLocation,
                    CreatedAt = x.CreatedAt,
                    ContactFullName = x.ContactFullName,
                    ContactEmail = x.ContactEmail,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                    ContactAddress = x.ContactAddress,
                    TotalPrice = x.TotalPrice,
                    NumberOfParticipants = x.NumberOfParticipants,
                    Status = x.Status,
                    Note = x.Note,
                    PaidAmount = x.PaidAmount,
                    RefundRequests = x.BookingRefunds.Select(y => new BookingRefundDTO
                    {
                        RefundId = y.RefundId,
                        BookingId = y.BookingId,
                        RefundAmount = y.RefundAmount,
                        RefundStatus = y.RefundStatus,
                        RefundDate = y.RefundDate,
                        RefundReason = y.RefundReason,
                        RefundNote = y.RefundNote,
                        BankCode = y.BankCode,
                        BankTransactionNumber = y.BankTransactionNumber,
                        CreatedAt = y.CreatedAt,
                    }).ToList(),
                    Tourists = x.BookingTourists.Select(y => new BookingTouristDetailDTO
                    {
                        TouristId = y.TouristId,
                        FullName = y.FullName,
                        Gender = y.Gender,
                        PhoneNumber = y.PhoneNumber,
                        DateOfBirth = y.DateOfBirth,
                        Price = y.Price,
                        PIN = y.PIN,
                    }).ToList(),
                    Payments = x.BookingPayments.Select(y => new BookingPaymentDetailDTO
                    {
                        PaymentId = y.PaymentId,
                        Amount = y.Amount,
                        Note = y.Note,
                        CreateAt = y.CreateAt,
                        BankCode = y.BankCode,
                        BankTransactionNumber = y.BankTransactionNumber,
                        PayTime = y.PayTime,
                        ThirdPartyTransactionNumber = y.ThirdPartyTransactionNumber,
                        Status = y.Status
                    }).ToList(),
                    TourPolicies = x.Tour.TourRefundPolicies.Select(y => new TourPolicyPreview
                    {
                        CancelBefore = y.CancelBefore,
                        RefundPercent = y.RefundPercent
                    }).ToList()
                }).SingleOrDefaultAsync();
            return result;
        }

        public async Task<(int count, List<BookingPreviewDTO>)> GetBookingsAsync(BookingStatus? bookingStatus, int pageCount, int pageIndex, string? bookingIdSearch, string? contactNameSearch, string? contactPhoneSearch, string? tourIdSearch)
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
            if (!string.IsNullOrWhiteSpace(contactPhoneSearch))
            {
                query = query.Where(x => x.ContactPhoneNumber.Contains(contactPhoneSearch));
            }
            if (!string.IsNullOrWhiteSpace(tourIdSearch))
            {
                query = query.Where(x => x.TourId.Equals(tourIdSearch));
            }
            int count = await query.CountAsync();
            List<BookingPreviewDTO> items = await query
                .Skip((pageIndex - 1) * pageCount)
                .Take(pageCount)
                .Select(x => new BookingPreviewDTO()
                {
                    BookingId = x.BookingId,
                    TourId = x.TourId,
                    TourName = x.Tour.TourTemplate.TourName,
                    TourCode = x.Tour.TourTemplate.Code,
                    Duration = x.Tour.TourTemplate.TourDuration.DurationName,
                    Provinces = x.Tour.TourTemplate.TourTemplateProvinces.Select(y => y.Province.Name).ToList(),
                    StartDate = x.Tour.StartDate,
                    StartLocation = x.Tour.StartLocation,
                    CreatedAt = x.CreatedAt,
                    PaidAmount = x.PaidAmount,
                    ContactFullName = x.ContactFullName,
                    ContactEmail = x.ContactEmail,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                    TotalPrice = x.TotalPrice,
                    NumberOfParticipants = x.NumberOfParticipants,
                    Status = x.Status,
                    HavePendingRefund = x.BookingRefunds.Any(y => y.RefundStatus == RefundStatus.Pending),
                    Tourists = x.BookingTourists.Select(y => new BookingTouristPreviewDTO
                    {
                        TouristId = y.TouristId,
                        FullName = y.FullName,
                        DateOfBirth = y.DateOfBirth,
                        PIN = y.PIN,
                    }).ToList(),
                }).ToListAsync();
            return (count, items);
        }
        public async Task ChangeBookingTourAsync(string accountId, string bookingId, string newTourId, string reason, int attemps = 0)
        {
            try
            {
                Account account = await _unitOfWork.AccountRepository.Query().SingleOrDefaultAsync(x => x.AccountId == accountId);
                if (account.Role != UserRole.Manager && account.Role != UserRole.Staff)
                {
                    throw new UnauthorizedException("UNAUTHORIZED");
                }

                Booking booking = await _unitOfWork
                    .BookingRepository.Query().Include(x => x.BookingTourists).SingleOrDefaultAsync(x => x.BookingId == bookingId)
                    ?? throw new ResourceNotFoundException("NOT_EXIST_BOOKING");

                if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Deposited && booking.Status != BookingStatus.Paid)
                {
                    throw new InvalidActionException("INVALID_ACTION_BOOKING_CHANGE");
                }

                Tour? oldTour = await _unitOfWork.TourRepository.Query()
                        .Include(x => x.TourPrices)
                        .SingleOrDefaultAsync(x => x.TourId == booking.TourId)
                        ?? throw new ResourceNotFoundException("NOT_EXIST_TOUR");

                Tour? newTour = await _unitOfWork.TourRepository.Query()
                        .Include(x => x.TourPrices)
                        .SingleOrDefaultAsync(x => x.TourId == newTourId && x.Status == TourStatus.Opened && x.IsDeleted == false)
                        ?? throw new ResourceNotFoundException("NOT_EXIST_TOUR");
                bool isActiveBookingExisted = await _unitOfWork.BookingRepository.Query()
                    .AnyAsync(x => x.TourId == newTourId && x.CustomerId == booking.CustomerId && (x.Status == BookingStatus.Pending || x.Status == BookingStatus.Deposited || x.Status == BookingStatus.Paid));

                if (isActiveBookingExisted)
                {
                    throw new InvalidActionException("INVALID_ACTION_BOOKED_TOUR");
                }
                if (newTour.CurrentParticipant + booking.BookingTourists.Count > newTour.MaxParticipant)
                {
                    throw new InvalidActionException("INVALID_ACTION_TOUR_FULL");
                }

                oldTour.CurrentParticipant -= booking.NumberOfParticipants;
                newTour.CurrentParticipant += booking.NumberOfParticipants;
                if (newTour.CurrentParticipant == newTour.MaxParticipant)
                {
                    newTour.Status = TourStatus.Closed;
                }

                booking.TourId = newTourId;
                decimal oldPrice = booking.TotalPrice;
                booking.TotalPrice = 0;
                foreach (BookingTourist tourist in booking.BookingTourists)
                {
                    int age = CalculateAge(tourist.DateOfBirth, _timeZoneHelper.GetUTC7Now());
                    TourPrice? tourPrice = newTour.TourPrices?.SingleOrDefault(x => x.AgeFrom <= age && age <= x.AgeTo);
                    if (tourPrice == null)
                    {
                        tourist.Price = newTour.DefaultTouristPrice!.Value;
                        booking.TotalPrice += tourist.Price;
                    }
                    else
                    {
                        tourist.Price = tourPrice.Price;
                        booking.TotalPrice += tourist.Price;
                    }
                }
                booking.Status = BookingStatus.PendingChangeConfirmation;

                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
                await _unitOfWork.TourRepository.UpdateAsync(oldTour);
                await _unitOfWork.TourRepository.UpdateAsync(newTour);

                string entityHistoryId = _idGenerator.GenerateId();
                await _unitOfWork.EntityHistoryRepository.CreateAsync(new EntityHistory()
                {
                    Id = entityHistoryId,
                    Action = EntityModifyAction.Update,
                    EntityId = bookingId,
                    EntityType = EntityType.Booking,
                    Timestamp = _timeZoneHelper.GetUTC7Now(),
                    ModifiedBy = accountId,
                    ModifierRole = account.Role,
                    Reason = reason,
                });
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (attemps < 3)
                {
                    await ChangeBookingTourAsync(accountId, bookingId, newTourId, reason, attemps + 1);
                }
                else
                {
                    throw new ServerErrorException("DATABASE_CONCURRENT_ERROR");
                }
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<BookingHistoryDTO>> GetBookingHistoryAsync(string bookingId)
        {
            bool isBookingExist = _unitOfWork.BookingRepository.Query()
                .Any(x => x.BookingId == bookingId);
            if (!isBookingExist)
            {
                throw new ResourceNotFoundException("NOT_EXIST_BOOKING");
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

        private static int CalculateAge(DateTime birthDay, DateTime currentDate)
        {
            int age = currentDate.Year - birthDay.Year;
            if (currentDate < birthDay.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}