using Microsoft.AspNetCore.Http;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourTemplateService
    {
        public Task<PaginatedList<TourTemplatePreviewDTO>> GetAllTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            List<TourTemplateStatus>? statuses,
            int pageSize,
            int pageIndex);
        public Task<TourTemplateDetailDTO?> GetTemplateByIdAsync(string id);
        public Task<string> CreateTemplateAsync(TourTemplate tourTemplate);
        public Task UpdateTemplateAsync(string tourTemplateId, TourTemplate newTourTemplate);
        public Task DeleteTemplateAsync(string accountId, string tourTemplateId);
        public Task<PaginatedList<TourTemplateWithTourInfoDTO>> GetAllTemplateWithActiveToursAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<int>? numberOfDay,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            decimal? minPrice,
            decimal? maxPrice,
            int pageSize,
            int pageIndex);
        Task UpdateTourTemplateImageAsync(string tourTemplateId, string staffId, List<IFormFile>? newImages, List<string>? deletedImageIds);
        public Task ChangeTourTemplateStatusAsync(string tourTemplateId, string accountId, TourTemplateStatus templateStatus, string? reason);
    }
}
