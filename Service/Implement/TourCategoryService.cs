using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class TourCategoryService(IUnitOfWork unitOfWork,
        IIdGenerator idGenerator) : ITourCategoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;

        public async Task<(int totalCount, List<TourCategoryDTO> items)> GetAllTourCategoryAsync(
            string? nameSearch,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .TourCategoryRepository
                .Query()
                .Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }
            int count = await query.CountAsync();

            List<TourCategoryDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TourCategoryDTO
                {
                    TourCategoryId = x.TourCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (count, items);
        }

        public async Task<string> CreateTourCategoryAsync(TourCategory tourCategory)
        {
            tourCategory.CreatedAt = DateTime.Now;
            try
            {
                tourCategory.TourCategoryId ??= _idGenerator.GenerateId();
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourCategoryRepository.CreateAsync(tourCategory);
                await _unitOfWork.CommitTransactionAsync();
                return tourCategory.TourCategoryId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task UpdateTourCategoryAsync(TourCategory newTourCategory)
        {
            TourCategory? tourCategory = await _unitOfWork.TourCategoryRepository.Query()
                .SingleOrDefaultAsync(x => x.TourCategoryId.Equals(newTourCategory.TourCategoryId)) ??
                throw new ResourceNotFoundException("Tour Category not found");

            tourCategory.Name = newTourCategory.Name;
            tourCategory.Description = newTourCategory.Description;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourCategoryRepository.UpdateAsync(tourCategory);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<TourCategoryDTO?> GetTourCategoryByIdAsync(string tourCategoryId)
        {
            return await _unitOfWork.TourCategoryRepository
                .Query()
                .Where(x => x.TourCategoryId.Equals(tourCategoryId))
                .Select(x => new TourCategoryDTO
                {
                    TourCategoryId = x.TourCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt
                })
                .SingleOrDefaultAsync();
        }

        public async Task DeleteTourCategory(string tourCategoryId)
        {
            TourCategory? tourCategory = await _unitOfWork.TourCategoryRepository.Query()
                .SingleOrDefaultAsync(x => x.TourCategoryId.Equals(tourCategoryId)) ??
                throw new ResourceNotFoundException("Tour Category not found");

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.TourCategoryRepository.DeleteAsync(tourCategory);
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
