using Microsoft.AspNetCore.Http;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ITourTemplateService
    {
        public Task<(int totalCount, List<TourTemplate> items)> GetAllTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            TourTemplateStatus? status,
            int pageSize,
            int pageIndex);
        public Task<TourTemplate?> GetTemplateByIdAsync(string id);
        public Task<string> CreateTemplateAsync(TourTemplate tourTemplate);
        public Task UpdateTemplateImageAsync(TourTemplate tourTemplate, List<IFormFile> ImageFiles, List<string> removedImageId);
        public Task UpdateTemplateAsync(TourTemplate tourTemplate, List<TourTemplateSchedule> newSchedule);
        public Task DeleteTemplateAsync(TourTemplate tourTemplate);
        public Task<(int totalCount, List<TourTemplate> items)> GetAllApprovedTemplatesAsync(
            string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<string>? durationIds,
            int pageSize,
            int pageIndex);

        public Task<(int count, List<TourTemplateWithTourInfoDTO> items)> GetAllTemplateWithActiveToursAsync(string? nameSearch,
            List<string>? templateCategoryIds,
            List<string>? provinceIds,
            List<int>? numberOfDay,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            decimal? minPrice,
            decimal? maxPrice,
            int pageSize,
            int pageIndex);

        public Task<List<TourTemplatePreviewDTO>> GetTourTemplatesPreviewRelatedToAttractionAsync(string attractionId, int previewCount);
        Task UpdateTourTemplateImageAsync(string tourTemplateId, string staffId, List<IFormFile>? newImages, List<string>? deletedImageIds);
        public Task ChangeTourTemplateStatusAsync(string tourTemplateId, string accountId, TourTemplateStatus templateStatus, string? reason);
    }
}
