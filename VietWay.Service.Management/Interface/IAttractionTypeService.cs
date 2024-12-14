using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IAttractionTypeService
    {
        public Task<List<AttractionCategoryDTO>> GetAllAttractionType(string? nameSearch);
        public Task<string> CreateAttractionCategoryAsync(AttractionCategory attractionCategory);
        public Task UpdateAttractionCategoryAsync(string attractionCategoryId,
            AttractionCategory newAttractionCategory);
        public Task DeleteAttractionCategoryAsync(string attractionCategoryId);
        public Task<AttractionCategoryDTO?> GetAttractionCategoryByIdAsync(string attractionCategoryId);
    }
}
