using Microsoft.EntityFrameworkCore;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;

namespace VietWay.Job.Implementation
{
    public class TourCategoryJob(IUnitOfWork unitOfWork, IRedisCacheService redisCacheService) : ITourCategoryJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        public async Task CacheTourCategoryJob()
        {
            List<TourCategory> tourCategories = await _unitOfWork.TourCategoryRepository.Query()
                .Where(x => x.IsDeleted == false)
                .ToListAsync();
            await _redisCacheService.SetAsync("TourCategories", tourCategories);
        }
    }
}
