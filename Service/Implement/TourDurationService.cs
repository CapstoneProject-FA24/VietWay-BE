using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourDurationService(IUnitOfWork unitOfWork): ITourDurationService
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<TourDuration>> GetAllTourDuration()
        {
            return await _unitOfWork.TourDurationRepository
                .Query()
                .ToListAsync();
        }
    }
}
