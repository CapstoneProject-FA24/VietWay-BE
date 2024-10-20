using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Implement
{
    public class TourService : ITourService
    {
        public readonly IUnitOfWork _unitOfWork;
        public TourService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Tour> CreateTour(Tour tour)
        {
            await _unitOfWork.TourRepository
                .CreateAsync(tour);
            return tour;
        }

        public async Task<Tour> EditTour(Tour updatedTour)
        {
            await _unitOfWork.TourRepository
                .UpdateAsync(updatedTour);
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
                .ToListAsync();
            return (count, items);
        }

        public async Task<Tour?> GetTourById(string id)
        {
            return await _unitOfWork.TourRepository
                .Query()
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)
                .SingleOrDefaultAsync(x => x.TourId.Equals(id));
        }

        public async Task<(int totalCount, List<Tour> items)> GetAllScheduledTour(int pageSize, int pageIndex)
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
                .Where(x => x.Status == TourStatus.Scheduled)
                .ToListAsync();
            return (count, items);
        }
        public async Task<List<Tour>> GetAllToursByTemplateIdsAsync(
            string tourTemplateId)
        {
            return await _unitOfWork
                .TourRepository
                .Query()
                .Where(x => x.IsDeleted == false && x.TourTemplateId.Equals(tourTemplateId) &&x.Status == TourStatus.Scheduled)
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)
                .ToListAsync();
        }
    }
}
