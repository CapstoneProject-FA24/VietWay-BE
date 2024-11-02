using VietWay.Repository.EntityModel;

namespace VietWay.Service.Management.Interface
{
    public interface ITourService
    {
        public Task<string> CreateTour(Tour tour);
        public Task EditTour(Tour updatedTour);
        public Task<(int totalCount, List<Tour> items)> GetAllTour(int pageSize, int pageIndex);
        public Task<Tour?> GetTourById(string id);
        public Task<(int totalCount, List<Tour> items)> GetAllScheduledTour(int pageSize, int pageIndex);
        public Task<List<Tour>> GetAllToursByTemplateIdsAsync(
            string tourTemplateIds);
    }
}
