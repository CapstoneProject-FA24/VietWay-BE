using Microsoft.AspNetCore.Http;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IAttractionService
    {
        public Task<string> CreateAttractionAsync(Attraction attraction);
        public Task DeleteAttractionAsync(string attractionId);
        public Task UpdateAttractionAsync(Attraction newAttraction);
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
    }
}
