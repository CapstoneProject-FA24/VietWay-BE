using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourCategoryService
    {
        public Task<(int totalCount, List<TourCategoryDTO> items)> GetAllTourCategoryAsync(
            string? nameSearch,
            int pageSize,
            int pageIndex);
        public Task<string> CreateTourCategoryAsync(TourCategory tourCategory);
        public Task UpdateTourCategoryAsync(TourCategory newTourCategory);
        public Task<TourCategoryDTO?> GetTourCategoryByIdAsync(string tourCategoryId);
        public Task DeleteTourCategory(string tourCategoryId);
    }
}
