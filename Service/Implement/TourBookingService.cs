using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourBookingService(IUnitOfWork unitOfWork) : ITourBookingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task CreateBookingAsync(TourBooking tourBooking)
        {
            await _unitOfWork.TourBookingRepository.Create(tourBooking);
        }

        public async Task<TourBookingInfoDTO?> GetTourBookingInfoAsync(string bookingId)
        {
            return await _unitOfWork
                .TourBookingRepository
                .Query()
                .Where(x => x.BookingId == bookingId)
                .Include(x => x.Tour.TourTemplate.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .Include(x => x.BookingTourParticipants)
                .Select(x => new TourBookingInfoDTO()
                {
                    BookingId = x.BookingId,
                    ContactAddress = x.ContactAddress,
                    ContactEmail = x.ContactEmail,
                    ContactFullName = x.ContactFullName,
                    ContactPhoneNumber = x.ContactPhoneNumber,
                    CustomerId = x.CustomerId,
                    EndDate = x.Tour.EndDate,
                    ImageUrl = x.Tour.TourTemplate.TourTemplateImages.First().Image.Url,
                    NumberOfParticipants = x.NumberOfParticipants,
                    StartDate = x.Tour.StartDate,
                    StartLocation = x.Tour.StartLocation,
                    Status = x.Status,
                    TotalPrice = x.TotalPrice,
                    TourId = x.TourId,
                    TourName = x.Tour.TourTemplate.TourName,
                    Participants = x.BookingTourParticipants.Select(y => new TourParticipantDTO()
                    {
                        DateOfBirth = y.DateOfBirth,
                        FullName = y.FullName,
                        Gender = y.Gender,
                        PhoneNumber = y.PhoneNumber,
                    }).ToList()
                }).SingleOrDefaultAsync();
        }
    }
}