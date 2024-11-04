using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class TourDurationService(IUnitOfWork unitOfWork) : ITourDurationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<TourDurationDTO>> GetAllTourDuration()
        {
            return await _unitOfWork.TourDurationRepository
                .Query()
                .Select(x => new TourDurationDTO()
                {
                    DurationId = x.DurationId,
                    DurationName = x.DurationName,
                    NumberOfDay = x.NumberOfDay,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
        }
    }
}
