using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourBookingService(IUnitOfWork unitOfWork) : ITourBookingService
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task CreateBookingAsync(Booking tourBooking)
        {
            await _unitOfWork.BookingRepository.CreateAsync(tourBooking);
        }

        public async Task<TourBookingInfoDTO?> GetTourBookingInfoAsync(string bookingId)
        {
            return await _unitOfWork
                .BookingRepository
                .Query()
                .Where(x => x.BookingId == bookingId)
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