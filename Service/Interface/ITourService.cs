using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourService
    {
        public Task<string> CreateTour(Tour tour);
        public Task EditTour(Tour updatedTour);
        public Task<(int totalCount, List<TourPreviewDTO> items)> GetAllTour(string? nameSearch, string? codeSearch, List<string>? provinceIds, List<string>? tourCategoryIds,List<string>? durationIds, TourStatus? status, int pageSize, int pageIndex,DateTime? startDateFrom, DateTime? startDateTo);
        public Task<Tour?> GetTourById(string id);
        public Task<(int totalCount, List<Tour> items)> GetAllScheduledTour(int pageSize, int pageIndex);
        public Task<List<TourPreviewDTO>> GetAllToursByTemplateIdsAsync(
            string tourTemplateIds);
        public Task ChangeTourStatusAsync(string tourId, string accountId, TourStatus tourStatus, string? reason);
    }
}
