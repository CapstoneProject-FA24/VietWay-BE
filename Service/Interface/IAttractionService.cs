using Microsoft.AspNetCore.Http;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface IAttractionService
    {
        public Task<string> CreateAttractionAsync(Attraction attraction);
        public Task DeleteAttractionAsync(string attractionId);
        public Task UpdateAttractionAsync(Attraction newAttraction);
        public Task<(int totalCount, List<AttractionPreviewWithCreateAtDTO> items)> GetAllAttractionsWithCreatorAsync(
            string? nameSearch,
            List<string>? provinceIds,
            List<string>? attractionTypeIds,
            AttractionStatus? status,
            int pageSize,
            int pageIndex);
        public Task<AttractionDetailWithCreatorDTO?> GetAttractionWithCreatorByIdAsync(string attractionId);

        public Task UpdateAttractionImageAsync(string attractionId, List<IFormFile>? imageFiles, List<string>? imageIdsToRemove);
    }
}
