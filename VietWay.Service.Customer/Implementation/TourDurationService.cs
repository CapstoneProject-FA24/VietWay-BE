using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.Service.Customer.Implementation
{
    public class TourDurationService(IUnitOfWork unitOfWork) : ITourDurationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<List<TourDurationDTO>> GetTourDurationsAsync()
        {
            return await _unitOfWork.TourDurationRepository
                .Query()
                .Where(x => false == x.IsDeleted)
                .Select(x => new TourDurationDTO
                {
                    DurationId = x.DurationId,
                    DurationName = x.DurationName,
                    NumberOfDay = x.NumberOfDay,
                }).ToListAsync();
        }
    }
}
