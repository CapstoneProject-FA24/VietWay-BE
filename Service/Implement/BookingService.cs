using Google.Api.Gax;
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
        public async Task CancelBookingAsync(string bookingId, string accountId, string? reason)
        {
            try
            {
                Account account = await _unitOfWork.AccountRepository.Query().SingleOrDefaultAsync(x => x.AccountId == accountId);
                if (account.Role != UserRole.Manager && account.Role != UserRole.Staff)
                {
                    throw new UnauthorizedException("You are not allowed to perform this action");
                }

                Booking booking = await _unitOfWork.BookingRepository.Query()
                    .Include(x => x.Tour)
                    .SingleOrDefaultAsync(x => x.BookingId.Equals(bookingId))
                    ?? throw new ResourceNotFoundException("Booking not found");
                if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
                {
                    throw new InvalidOperationException("Cannot cancel booking that is not pending or confirmed");
                }
                int oldStatus = (int)booking.Status;
                if (booking.Status == BookingStatus.Pending) booking.Status = BookingStatus.Cancelled;
                if (booking.Status == BookingStatus.Confirmed) booking.Status = BookingStatus.PendingRefund;

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

        public async Task<BookingDetailDTO?> GetBookingByIdAsync(string bookingId)
        {
            BookingDetailDTO result = await _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.BookingId == bookingId)
                .Include(x => x.Tour)
                .ThenInclude(x => x.TourTemplate)
                .Include(x => x.Tour)
                .ThenInclude(x => x.TourRefundPolicies)
                .Include(x => x.BookingPayments)
                .Include(x => x.BookingTourists)
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
                    Tourists = x.BookingTourists.Select(y => new BookingTouristDetailDTO
                    {
                        TouristId = y.TouristId,
                        FullName = y.FullName,
                        Gender = y.Gender,
                        PhoneNumber = y.PhoneNumber,
                        DateOfBirth = y.DateOfBirth,
                        Price = y.Price
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

            EntityStatusHistory entityStatusHistory = await _unitOfWork.EntityStatusHistoryRepository.Query()
                .Include(x => x.EntityHistory)
                .SingleOrDefaultAsync(x => x.EntityHistory.EntityId == bookingId && x.EntityHistory.Action == EntityModifyAction.ChangeStatus && x.NewStatus == (int)BookingStatus.PendingRefund);
            if (entityStatusHistory != null)
            {
                result.CancelAt = entityStatusHistory.EntityHistory.Timestamp;
                result.CancelBy = entityStatusHistory.EntityHistory.ModifierRole;
                decimal refundAmount = 0;

                if (entityStatusHistory.EntityHistory.ModifierRole == UserRole.Manager)
                {
                    refundAmount = result.TotalPrice;
                }
                else
                {
                    DateTime cancelTime = entityStatusHistory.EntityHistory.Timestamp;

                    TourRefundPolicy tourRefundPolicy = await _unitOfWork.TourRefundPolicyRepository.Query()
                        .OrderBy(x => x.CancelBefore)
                        .FirstOrDefaultAsync(x => x.CancelBefore > cancelTime && x.TourId == result.TourId);

                    if (tourRefundPolicy != null)
                    {
                        refundAmount = tourRefundPolicy.RefundPercent * result.TotalPrice / 100;
                    }
                }

                result.RefundAmount = refundAmount;
            }

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
                    ContactFullName = x.ContactFullName,
                    ContactEmail = x.ContactEmail,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                    TotalPrice = x.TotalPrice,
                    NumberOfParticipants = x.NumberOfParticipants,
                    Status = x.Status,
                    Tourists = x.BookingTourists.Select(y => new BookingTouristPreviewDTO
                    {
                        TouristId = y.TouristId,
                        FullName = y.FullName,
                        DateOfBirth = y.DateOfBirth,
                    }).ToList(),
                }).ToListAsync();
            return (count, items);
        }

        public async Task CreateRefundTransactionAsync(string accountId, string bookingId, BookingPayment bookingPayment)
        {
            Account account = await _unitOfWork.AccountRepository.Query().SingleOrDefaultAsync(x => x.AccountId == accountId);
            if (account.Role != UserRole.Manager && account.Role != UserRole.Staff)
            {
                throw new UnauthorizedException("You are not allowed to perform this action");
            }

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

            decimal refundAmount = 0;

            if (entityStatusHistory.EntityHistory.ModifierRole == UserRole.Manager || entityStatusHistory.EntityHistory.ModifierRole == UserRole.Staff)
            {
                refundAmount = booking.TotalPrice;
            }
            else
            {
                DateTime cancelTime = entityStatusHistory.EntityHistory.Timestamp;

                TourRefundPolicy tourRefundPolicy = await _unitOfWork.TourRefundPolicyRepository.Query()
                    .OrderBy(x => x.CancelBefore)
                    .FirstOrDefaultAsync(x => x.CancelBefore > cancelTime && x.TourId == booking.TourId)
                    ?? throw new ResourceNotFoundException("Refund policy not found");

                refundAmount = tourRefundPolicy.RefundPercent * booking.TotalPrice / 100;
            }

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
                        ModifiedBy = accountId,
                        ModifierRole = account.Role,
                        Reason = "",
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

        public async Task ChangeBookingTourAsync(string accountId, string bookingId, string newTourId, string reason)
        {
            try
            {
                Account account = await _unitOfWork.AccountRepository.Query().SingleOrDefaultAsync(x => x.AccountId == accountId);
                if (account.Role != UserRole.Manager && account.Role != UserRole.Staff)
                {
                    throw new UnauthorizedException("You are not allowed to perform this action");
                }

                Booking booking = await _unitOfWork
                    .BookingRepository.Query().Include(x => x.BookingTourists).SingleOrDefaultAsync(x => x.BookingId == bookingId)
                    ?? throw new ResourceNotFoundException("Booking not found");

                if (booking.Status != BookingStatus.Pending && booking.Status != BookingStatus.Confirmed)
                {
                    throw new InvalidOperationException("The booking is not in a pending or confirmed state and cannot change tour.");
                }

                Tour? oldTour = await _unitOfWork.TourRepository.Query()
                        .Include(x => x.TourPrices)
                        .SingleOrDefaultAsync(x => x.TourId == booking.TourId)
                        ?? throw new ResourceNotFoundException("Can not find any tour");

                Tour? newTour = await _unitOfWork.TourRepository.Query()
                        .Include(x => x.TourPrices)
                        .SingleOrDefaultAsync(x => x.TourId == newTourId && x.Status == TourStatus.Opened && x.IsDeleted == false)
                        ?? throw new ResourceNotFoundException("Can not find any tour");
                bool isActiveBookingExisted = await _unitOfWork.BookingRepository.Query()
                    .AnyAsync(x => x.TourId == newTourId && x.CustomerId == booking.CustomerId && (x.Status == BookingStatus.Pending || x.Status == BookingStatus.Confirmed));

                if (isActiveBookingExisted)
                {
                    throw new InvalidOperationException("Customer has already booked this tour");
                }
                if (newTour.CurrentParticipant + booking.BookingTourists.Count > newTour.MaxParticipant)
                {
                    throw new InvalidOperationException("Tour is full");
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

                if (oldPrice < booking.TotalPrice)
                {
                    booking.Status = BookingStatus.Pending;
                }

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
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
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