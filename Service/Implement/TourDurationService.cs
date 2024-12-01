using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class TourDurationService(IUnitOfWork unitOfWork,
        ITimeZoneHelper timeZoneHelper,
        IIdGenerator idGenerator) : ITourDurationService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<List<TourDurationDTO>> GetAllTourDuration(string? nameSearch)
        {
            var query = _unitOfWork
                .TourDurationRepository
                .Query()
                .Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.DurationName.Contains(nameSearch));
            }

            List<TourDurationDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TourDurationDTO
                {
                    DurationId = x.DurationId,
                    DurationName = x.DurationName,
                    NumberOfDay = x.NumberOfDay,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return items;
        }
        public async Task<string> CreateTourDurationAsync(TourDuration tourDuration)
        {
            try
            {
                tourDuration.DurationId ??= _idGenerator.GenerateId();
                tourDuration.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourDurationRepository.CreateAsync(tourDuration);
                await _unitOfWork.CommitTransactionAsync();
                return tourDuration.DurationId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task UpdateTourDurationAsync(TourDuration newTourDuration)
        {
            TourDuration? tourDuration = await _unitOfWork.TourDurationRepository.Query()
                .SingleOrDefaultAsync(x => x.DurationId.Equals(newTourDuration.DurationId)) ??
                throw new ResourceNotFoundException("Tour Duration not found");

            tourDuration.DurationName = newTourDuration.DurationName;
            tourDuration.NumberOfDay = newTourDuration.NumberOfDay;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourDurationRepository.UpdateAsync(tourDuration);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<TourDurationDTO?> GetTourCategoryByIdAsync(string tourDurationId)
        {
            return await _unitOfWork.TourDurationRepository
                .Query()
                .Where(x => x.DurationId.Equals(tourDurationId))
                .Select(x => new TourDurationDTO
                {
                    DurationId = x.DurationId,
                    DurationName = x.DurationName,
                    NumberOfDay = x.NumberOfDay,
                    CreatedAt = x.CreatedAt
                })
                .SingleOrDefaultAsync();
        }
        public async Task DeleteTourDuration(string tourDurationId)
        {
            TourDuration? tourDuration = await _unitOfWork.TourDurationRepository.Query()
                .SingleOrDefaultAsync(x => x.DurationId.Equals(tourDurationId)) ??
                throw new ResourceNotFoundException("Tour Duration not found");

            bool hasRelatedData = await _unitOfWork.TourDurationRepository.Query().AnyAsync(x => x.DurationId.Equals(tourDurationId));

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (hasRelatedData)
                {
                    await _unitOfWork.TourDurationRepository.SoftDeleteAsync(tourDuration);
                }
                else
                {
                    await _unitOfWork.TourDurationRepository.DeleteAsync(tourDuration);
                }
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
