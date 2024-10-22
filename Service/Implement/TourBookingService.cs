using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Implement
{
    public class TourBookingService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper) : ITourBookingService
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
            Booking? booking  = await _unitOfWork
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
                .Include(x => x.BookingTourParticipants)
                .Select(x => new TourBookingInfoDTO()
                {
                    BookingId = x.BookingId,
                    ContactAddress = x.ContactAddress,
                    ContactEmail = x.ContactEmail,
                    ContactFullName = x.ContactFullName,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                    CustomerId = x.CustomerId,
                    EndDate = (DateTime)x.Tour.EndDate,
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
                    Participants = x.BookingTourParticipants.Select(y => new TourParticipantDTO()
                    {
                        DateOfBirth = y.DateOfBirth,
                        FullName = y.FullName,
                        Gender = y.Gender,
                        PhoneNumber = y.PhoneNumber,
                    }).ToList(),
                    Code = x.Tour.TourTemplate.Code
                }).SingleOrDefaultAsync();
        }
    }
}