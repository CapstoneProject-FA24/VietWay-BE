using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface ITourCategoryService
    {
        public Task<List<TourCategory>> GetAllTourCategory();
    }
}
