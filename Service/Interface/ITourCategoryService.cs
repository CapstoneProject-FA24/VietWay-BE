using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourCategoryService
    {
        public Task<List<TourCategoryDTO>> GetAllTourCategory();
    }
}
