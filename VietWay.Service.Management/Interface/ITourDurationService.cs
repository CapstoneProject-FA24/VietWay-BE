using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourDurationService
    {
        public Task<List<TourDurationDTO>> GetAllTourDuration(string? nameSearch);
        public Task<string> CreateTourDurationAsync(TourDuration tourDuration);
        public Task UpdateTourDurationAsync(TourDuration newTourDuration);
        public Task<TourDurationDTO?> GetTourCategoryByIdAsync(string tourDurationId);
        public Task DeleteTourDuration(string tourDurationId);
    }
}
