using VietWay.Service.Customer.DataTransferObject;
namespace VietWay.Service.Customer.Interface
{
    public interface ITourTemplateService
    {
        Task<PaginatedList<TourTemplateWithTourInfoDTO>> GetTourTemplatesWithActiveToursAsync(string? nameSearch, 
            List<string>? templateCategoryIds, List<string>? provinceIds, List<int>? numberOfDay, DateTime? startDateFrom, DateTime? startDateTo, 
            decimal? minPrice, decimal? maxPrice, int pageSize, int pageIndex);
        Task<TourTemplateDetailDTO?> GetTemplateByIdAsync(string tourTemplateId);
        public Task<List<TourTemplatePreviewDTO>> GetTourTemplatePreviewsByAttractionId(string attractionId, int previewCount);
    }
}
