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
                .Where(x => false == x.IsDeleted && tourTemplateId == x.TourTemplateId && TourStatus.Opened == x.Status)
                .Select(x => new TourPreviewDTO()
                {
                    TourId = x.TourId,
                    CurrentParticipant = x.CurrentParticipant,
                    DefaultTouristPrice = x.DefaultTouristPrice,
                    MaxParticipant = x.MaxParticipant,
                    StartDate = x.StartDate,
                    StartLocation = x.StartLocation,
                }).ToListAsync();
        }

        public async Task<TourDetailDTO?> GetTourByIdAsync(string tourId)
        {
            return await _unitOfWork.TourRepository.Query()
                .Where(x => false == x.IsDeleted && tourId == x.TourId && TourStatus.Opened == x.Status)
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
    }
}
