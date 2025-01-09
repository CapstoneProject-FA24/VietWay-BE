using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IPopularService
    {
        Task<List<PopularProvinceDTO>> GetPopularProvincesAsync();
        Task CachePopularProvincesAsync();
        Task<List<PopularAttractionCategoryDTO>> GetPopularAttractionCategoriesAsync();
        Task CachePopularAttractionCategoriesAsync();
        Task<List<PopularPostCategoryDTO>> GetPopularPostCategoriesAsync();
        Task CachePopularPostCategoriesAsync();
        Task<List<string>> GetPopularTourCategoriesAsync();
        Task CachePopularTourCategoriesAsync();
        public Task<List<string>> GetPopularHashtagsAsync(bool isTwitter);
    }
}
