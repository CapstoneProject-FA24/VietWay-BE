using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
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
    }
}
