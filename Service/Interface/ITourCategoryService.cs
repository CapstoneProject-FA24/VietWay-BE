using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface ITourCategoryService
    {
        public Task<List<TourCategoryDTO>> GetAllTourCategory();
    }
}
