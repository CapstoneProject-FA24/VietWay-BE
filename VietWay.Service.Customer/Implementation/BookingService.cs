using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.IdUtil;
using VietWay.Util.CustomExceptions;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.DateTimeUtil;
using Hangfire;
using VietWay.Job.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class BookingService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper, 
        IBackgroundJobClient backgroundJobClient) : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;
        public async Task<string> BookTourAsync(Booking booking)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Tour? tour = await _unitOfWork.TourRepository.Query()
                    .Include(x => x.TourPrices)
                    .SingleOrDefaultAsync(x => x.TourId == booking.TourId)
                    ?? throw new ResourceNotFoundException("Tour not found");
                bool isActiveBookingExisted = await _unitOfWork.BookingRepository.Query()
                    .AnyAsync(x => x.TourId == booking.TourId && x.CustomerId == booking.CustomerId && (x.Status == BookingStatus.Pending || x.Status == BookingStatus.Confirmed));

                if (isActiveBookingExisted)
                {
                    throw new InvalidOperationException("Customer has already booked this tour");
                }
                if (tour.CurrentParticipant + booking.BookingTourists.Count > tour.MaxParticipant || TourStatus.Opened != tour.Status)
                {
                    throw new InvalidOperationException("Tour is full or not open");
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
                await _unitOfWork.CommitTransactionAsync();
                _backgroundJobClient.Schedule<IBookingJob>(x => x.CheckBookingForExpirationJob(booking.BookingId), DateTime.Now.AddMinutes(1));
                return booking.BookingId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CancelBookingAsync(string customerId, string bookingId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Booking booking = _unitOfWork.BookingRepository.Query()
                    .Include(x => x.Tour)
                    .SingleOrDefault(x => x.BookingId.Equals(bookingId) && x.CustomerId.Equals(customerId))
                    ?? throw new ResourceNotFoundException("Booking not found");
                if (booking.Status != BookingStatus.Pending)
                {
                    throw new InvalidOperationException("Booking is not pending");
                }
                booking.Status = BookingStatus.Cancelled;
                booking.Tour.CurrentParticipant -= booking.NumberOfParticipants;
                await _unitOfWork.BookingRepository.UpdateAsync(booking);
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
                        Price = y.Price,
                    }).ToList(),
                    Code = x.Tour.TourTemplate.Code,
                }).SingleOrDefaultAsync();
        }

        public async Task<(int count, List<BookingPreviewDTO> items)> GetCustomerBookingsAsync(string customerId,BookingStatus? bookingStatus, int pageSize, int pageIndex)
        {
            IQueryable<Booking> query = _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.CustomerId == customerId)
                .Include(x => x.Tour.TourTemplate.TourTemplateImages)
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
                    StartDate = x.Tour!.StartDate!.Value
                }).ToListAsync();
            return (count, items);
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
