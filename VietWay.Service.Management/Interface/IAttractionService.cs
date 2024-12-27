using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Client;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IAttractionService
    {
        public Task<string> CreateAttractionAsync(Attraction attraction);
        public Task DeleteAttractionAsync(string attractionId);
        public Task UpdateAttractionAsync(Attraction newAttraction, string accountId);
        public Task<PaginatedList<AttractionPreviewDTO>> GetAllAttractionsWithCreatorAsync(
            string? nameSearch,
            List<string>? provinceIds,
            List<string>? attractionTypeIds,
            List<AttractionStatus>? statuses,
            int pageSize,
            int pageIndex);
        public Task<AttractionDetailDTO?> GetAttractionWithCreateDateByIdAsync(string attractionId);
        public Task UpdateAttractionImageAsync(string attractionId, List<IFormFile>? imageFiles, List<string>? imageIdsToRemove);
        Task UpdateAttractionStatusAsync(string attractionId, string accountId, AttractionStatus status, string? reason);
        public Task<PaginatedList<AttractionPreviewDTO>> GetAllApproveAttractionsAsync(
            string? nameSearch, List<string>? provinceIds, List<string>? attractionCategoryIds, List<string>? attractionIds,
            int pageSize, int pageIndex);
        public Task PostAttractionWithXAsync(string attractionId);
    }
}
