using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VietWay.Job.Interface;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.ThirdParty.Redis;

namespace VietWay.Job.Implementation
{
    public class TourDurationJob(IUnitOfWork unitOfWork, IRedisCacheService redisCacheService) : ITourDurationJob
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        public async Task CacheTourDurationJob()
        {
            List<TourDuration> tourDurations = await _unitOfWork.TourDurationRepository.Query()
                .Where(x=>x.IsDeleted == false)
                .ToListAsync();
            await _redisCacheService.SetAsync("TourDurations", tourDurations);
        }
    }
}
