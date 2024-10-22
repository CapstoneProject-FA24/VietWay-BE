using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourDurationService(IUnitOfWork unitOfWork): ITourDurationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<TourDuration>> GetAllTourDuration()
        {
            return await _unitOfWork.TourDurationRepository
                .Query()
                .ToListAsync();
        }

        public async Task<List<TourDurationPreviewDTO>> GetTourDurationPreviews()
        {
            return await _unitOfWork.TourDurationRepository
                .Query()
                .Select(x => new TourDurationPreviewDTO()
                {
                    DurationId = x.DurationId,
                    DurationName = x.DurationName
                })
                .ToListAsync();
        }
    }
}
