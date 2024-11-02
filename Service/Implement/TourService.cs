using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.Interface;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class TourService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper) : ITourService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task<string> CreateTour(Tour tour)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                tour.CreatedAt = _timeZoneHelper.GetUTC7Now();
                tour.TourId = _idGenerator.GenerateId();
                foreach (TourPrice item in tour.TourPrices)
                {
                    item.PriceId = _idGenerator.GenerateId();
                    item.TourId = tour.TourId;
                }
                foreach (TourRefundPolicy item in tour.TourRefundPolicies)
                {
                    item.TourRefundPolicyId = _idGenerator.GenerateId();
                    item.TourId = tour.TourId;
                }
                await _unitOfWork.TourRepository
                    .CreateAsync(tour);
                await _unitOfWork.CommitTransactionAsync();
                return tour.TourId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task EditTour(Tour updatedTour)
        {
            await _unitOfWork.TourRepository
                .UpdateAsync(updatedTour);
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
                .Where(x => x.Status == TourStatus.Opened)
                .ToListAsync();
            return (count, items);
        }
        public async Task<List<Tour>> GetAllToursByTemplateIdsAsync(
            string tourTemplateId)
        {
            return await _unitOfWork
                .TourRepository
                .Query()
                .Where(x => x.IsDeleted == false && x.TourTemplateId.Equals(tourTemplateId) && x.Status == TourStatus.Opened)
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)
                .ToListAsync();
        }
    }
}
