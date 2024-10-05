using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class TourService : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TourService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Tour> CreateTour(Tour tour)
        {
            await _unitOfWork.TourRepository
                .Create(tour);
            return tour;
        }

        public async Task<Tour> EditTour(Tour updatedTour)
        {
            await _unitOfWork.TourRepository
                .Update(updatedTour);
            return updatedTour;
        }

        public async Task<(int totalCount, List<Tour> items)> GetAllTour(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .TourRepository
                .Query();
            int count = await query.CountAsync();
            List<Tour> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .ToListAsync();
            return (count, items);
        }

        public async Task<Tour?> GetTourById(string id)
        {
            return await _unitOfWork.TourRepository
                .Query()
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)
                .ThenInclude(x => x.Image)
                .SingleOrDefaultAsync(x => x.TourId.Equals(id));
        }
    }
}
