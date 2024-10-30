using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface ITourTemplateService
    {
        Task<(int count, List<TourTemplateWithTourInfoDTO> items)> GetAllTemplateWithActiveToursAsync(string? nameSearch, 
            List<string>? templateCategoryIds, List<string>? provinceIds, List<int>? numberOfDay, DateTime? startDateFrom, DateTime? startDateTo, 
            decimal? minPrice, decimal? maxPrice, int checkedPageSize, int checkedPageIndex);
        Task<TourTemplateDetailDTO?> GetTemplateByIdAsync(string tourTemplateId);
        public Task<List<TourTemplatePreviewDTO>> GetTourTemplatePreviewsByAttractionId(string attractionId, int previewCount);

    }
}
