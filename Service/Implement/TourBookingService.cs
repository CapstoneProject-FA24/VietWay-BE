using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
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
    }
}