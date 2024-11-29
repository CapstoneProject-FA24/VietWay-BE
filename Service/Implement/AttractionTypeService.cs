using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class AttractionTypeService(IUnitOfWork unitOfWork,
        IIdGenerator idGenerator) : IAttractionTypeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;

        public async Task<List<AttractionCategoryDTO>> GetAllAttractionType(string? nameSearch)
        {
            var query = _unitOfWork
                .AttractionCategoryRepository
                .Query()
                .Where(x => x.IsDeleted == false);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.Name.Contains(nameSearch));
            }

            return await query.Select(x => new AttractionCategoryDTO()
            {
                AttractionCategoryId = x.AttractionCategoryId,
                Name = x.Name,
                Description = x.Description,
                CreatedAt = x.CreatedAt
            }).ToListAsync();
        }
        public async Task<string> CreateAttractionCategoryAsync(AttractionCategory attractionCategory)
        {
            attractionCategory.CreatedAt = DateTime.Now;
            try
            {
                attractionCategory.AttractionCategoryId ??= _idGenerator.GenerateId();
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.AttractionCategoryRepository.CreateAsync(attractionCategory);
                await _unitOfWork.CommitTransactionAsync();
                return attractionCategory.AttractionCategoryId;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task UpdateAttractionCategoryAsync(string attractionCategoryId,
            AttractionCategory newAttractionCategory)
        {
            AttractionCategory? attractionCategory = await _unitOfWork.AttractionCategoryRepository.Query()
                .SingleOrDefaultAsync(x => x.AttractionCategoryId.Equals(attractionCategoryId)) ??
                throw new ResourceNotFoundException("Attraction Category not found");

            attractionCategory.Name = newAttractionCategory.Name;
            attractionCategory.Description = newAttractionCategory.Description;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.AttractionCategoryRepository.UpdateAsync(attractionCategory);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task DeleteAttractionCategoryAsync(string attractionCategoryId)
        {
            AttractionCategory? attractionCategory = await _unitOfWork.AttractionCategoryRepository.Query()
                .SingleOrDefaultAsync(x => x.AttractionCategoryId.Equals(attractionCategoryId)) ??
                throw new ResourceNotFoundException("Attraction Category not found");

            bool hasRelatedData = await _unitOfWork.AttractionRepository.Query().AnyAsync(x => x.AttractionCategoryId.Equals(attractionCategoryId));

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (hasRelatedData)
                {
                    await _unitOfWork.AttractionCategoryRepository.SoftDeleteAsync(attractionCategory);
                }
                else
                {
                    await _unitOfWork.AttractionCategoryRepository.DeleteAsync(attractionCategory);
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task<AttractionCategoryDTO?> GetAttractionCategoryByIdAsync(string attractionCategoryId)
        {
            return await _unitOfWork.AttractionCategoryRepository
                .Query()
                .Where(x => x.AttractionCategoryId.Equals(attractionCategoryId))
                .Select(x => new AttractionCategoryDTO
                {
                    AttractionCategoryId = x.AttractionCategoryId,
                    Name = x.Name,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                })
                .SingleOrDefaultAsync();
        }
    }
}
