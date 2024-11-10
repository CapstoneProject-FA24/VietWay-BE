using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.Interface;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;
using VietWay.Service.Management.DataTransferObject;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using VietWay.Util.CustomExceptions;

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

        public async Task<(int totalCount, List<TourPreviewDTO> items)> GetAllTour(string? nameSearch, string? codeSearch, List<string>? provinceIds, List<string>? tourCategoryIds, List<string>? durationIds, TourStatus? status, int pageSize, int pageIndex, DateTime? startDateFrom, DateTime? startDateTo)
        {
            IQueryable<Tour> query = _unitOfWork
                .TourRepository
                .Query()
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages);
            if (nameSearch != null)
            {
                query = query.Where(x => x.TourTemplate.TourName.Contains(nameSearch));
            }
            if (codeSearch != null)
            {
                query = query.Where(x => x.TourTemplate.Code.Contains(codeSearch));
            }
            if (provinceIds?.Count > 0)
            {
                query = query.Where(x => x.TourTemplate.TourTemplateProvinces.Any(y => provinceIds.Contains(y.ProvinceId)));
            }
            if (tourCategoryIds?.Count > 0)
            {
                query = query.Where(x => tourCategoryIds.Contains(x.TourTemplate.TourCategoryId));
            }
            if (durationIds?.Count > 0)
            {
                query = query.Where(x => durationIds.Contains(x.TourTemplate.DurationId));
            }
            if (status != null)
            {
                query = query.Where(x => status == x.Status);
            }
            if (startDateFrom != null)
            {
                query = query.Where(x => startDateFrom <= x.StartDate);
            }
            if (startDateTo != null)
            {
                query = query.Where(x => startDateTo >= x.StartDate);
            }
            int count = await query.CountAsync();
            List<TourPreviewDTO> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.TourTemplate)
                .ThenInclude(x => x.TourTemplateImages)

                .Select(x => new TourPreviewDTO
                {
                    TourId = x.TourId,
                    TourTemplateId = x.TourTemplateId,
                    Code = x.TourTemplate.Code,
                    TourName = x.TourTemplate.TourName,
                    Duration = x.TourTemplate.TourDuration.DurationName,
                    ImageUrl = x.TourTemplate.TourTemplateImages.FirstOrDefault().ImageUrl,
                    StartLocation = x.StartLocation,
                    StartDate = x.StartDate,
                    DefaultTouristPrice = x.DefaultTouristPrice,
                    MaxParticipant = x.MaxParticipant,
                    MinParticipant = x.MinParticipant,
                    CurrentParticipant = x.CurrentParticipant,
                    Status = x.Status
                })
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

        public async Task ChangeTourStatusAsync(string tourId, TourStatus tourStatus)
        {
            Tour? tour = await _unitOfWork.TourRepository.Query()
                .SingleOrDefaultAsync(x => x.TourId.Equals(tourId)) ??
                throw new ResourceNotFoundException("Tour not found");

            tour.Status = tourStatus;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourRepository.UpdateAsync(tour);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
