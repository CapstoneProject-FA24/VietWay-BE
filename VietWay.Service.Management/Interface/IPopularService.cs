using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IPopularService
    {
        Task<List<string>> GetPopularProvincesAsync();
        Task CachePopularProvincesAsync();
        Task<List<string>> GetPopularAttractionCategoriesAsync();
        Task CachePopularAttractionCategoriesAsync();
        Task<List<string>> GetPopularPostCategoriesAsync();
        Task CachePopularPostCategoriesAsync();
        Task<List<string>> GetPopularTourCategoriesAsync();
        Task CachePopularTourCategoriesAsync();
    }
}
