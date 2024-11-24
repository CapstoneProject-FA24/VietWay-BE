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

        public async Task<List<TourCategoryDTO>> GetAllTourCategoryAsync(
            string? nameSearch)
        {
            var query = _unitOfWork
                .TourCategoryRepository
                .Query()
                .Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }

            List<TourCategoryDTO> items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new TourCategoryDTO
                {
                    TourCategoryId = x.TourCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

            return items;
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

            bool hasRelatedData = await _unitOfWork.TourTemplateRepository.Query().AnyAsync(x => x.TourCategoryId.Equals(tourCategoryId));

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (hasRelatedData)
                {
                    await _unitOfWork.TourCategoryRepository.SoftDeleteAsync(tourCategory);
                }
                else
                {
                    await _unitOfWork.TourCategoryRepository.DeleteAsync(tourCategory);
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
