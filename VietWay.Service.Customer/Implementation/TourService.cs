using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.DateTimeUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class TourService(IUnitOfWork unitOfWork, ITimeZoneHelper timeZoneHelper) : ITourService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<List<TourPreviewDTO>> GetAllToursByTemplateIdsAsync(string tourTemplateId)
        {
            return await _unitOfWork.TourRepository.Query()
                .Where(x => false == x.IsDeleted && tourTemplateId == x.TourTemplateId && TourStatus.Opened == x.Status && ((DateTime)x.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && ((DateTime)x.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !x.IsDeleted)
                .Select(x => new TourPreviewDTO()
                {
                    TourId = x.TourId,
                    CurrentParticipant = x.CurrentParticipant,
                    DefaultTouristPrice = x.DefaultTouristPrice,
                    MaxParticipant = x.MaxParticipant,
                    StartDate = x.StartDate,
                    StartLocation = x.StartLocation,
                    DepositPercent = x.DepositPercent,
                    PaymentDeadline = x.PaymentDeadline,
                    RefundPolicies = x.TourRefundPolicies.Select(y => new TourRefundPolicyDTO()
                    {
                        CancelBefore = y.CancelBefore,
                        RefundPercent = y.RefundPercent,
                    }).ToList(),
                }).ToListAsync();
        }

        public async Task<TourDetailDTO?> GetTourByIdAsync(string tourId)
        {
            return await _unitOfWork.TourRepository.Query()
                .Where(x => false == x.IsDeleted && tourId == x.TourId && TourStatus.Opened == x.Status && ((DateTime)x.RegisterOpenDate).Date <= _timeZoneHelper.GetUTC7Now().Date && ((DateTime)x.RegisterCloseDate).Date >= _timeZoneHelper.GetUTC7Now().Date && !x.IsDeleted)
                .Select(x => new TourDetailDTO()
                {
                    TourId = x.TourId,
                    TourTemplateId = x.TourTemplateId,
                    CurrentParticipant = x.CurrentParticipant,
                    DefaultTouristPrice = x.DefaultTouristPrice,
                    MaxParticipant = x.MaxParticipant,
                    MinParticipant = x.MinParticipant,
                    RegisterCloseDate = x.RegisterCloseDate,
                    RegisterOpenDate = x.RegisterOpenDate,
                    StartDate = x.StartDate,
                    StartLocation = x.StartLocation,
                    StartLocationPlaceId = x.StartLocationPlaceId,
                    DepositPercent = x.DepositPercent,
                    PaymentDeadline = x.PaymentDeadline,
                    PricesByAge = x.TourPrices.Select(y => new TourPriceDTO()
                    {
                        AgeFrom = y.AgeFrom,
                        AgeTo = y.AgeTo,
                        Price = y.Price,
                        Name = y.Name,
                    }).ToList(),
                    RefundPolicies = x.TourRefundPolicies.Select(y => new TourRefundPolicyDTO()
                    {
                        CancelBefore = y.CancelBefore,
                        RefundPercent = y.RefundPercent,
                    }).ToList(),
                }).SingleOrDefaultAsync();


        }

        public async Task<TourDetailDTO?> GetTourDetailByBookingIdAsync(string customerId, string bookingId)
        {
            return await _unitOfWork.BookingRepository.Query()
                .Where(x => x.BookingId == bookingId && x.CustomerId == customerId)
                .Select(x => new TourDetailDTO
                {
                    CurrentParticipant = x.Tour.CurrentParticipant,
                    DefaultTouristPrice = x.Tour.DefaultTouristPrice,
                    DepositPercent = x.Tour.DepositPercent,
                    MaxParticipant = x.Tour.MaxParticipant,
                    MinParticipant = x.Tour.MinParticipant,
                    PaymentDeadline = x.Tour.PaymentDeadline,
                    PricesByAge = x.Tour.TourPrices.Select(y => new TourPriceDTO
                    {
                        AgeFrom = y.AgeFrom,
                        AgeTo = y.AgeTo,
                        Name = y.Name,
                        Price = y.Price,
                    }).ToList(),
                    RefundPolicies = x.Tour.TourRefundPolicies.Select(y => new TourRefundPolicyDTO
                    {
                        CancelBefore = y.CancelBefore,
                        RefundPercent = y.RefundPercent,
                    }).ToList(),
                    RegisterCloseDate = x.Tour.RegisterCloseDate,
                    RegisterOpenDate = x.Tour.RegisterOpenDate,
                    StartDate = x.Tour.StartDate,
                    StartLocation = x.Tour.StartLocation,
                    StartLocationPlaceId = x.Tour.StartLocationPlaceId,
                    TourId = x.TourId,
                    TourTemplateId = x.Tour.TourTemplateId,
                }).SingleOrDefaultAsync();
        }
    }
}
