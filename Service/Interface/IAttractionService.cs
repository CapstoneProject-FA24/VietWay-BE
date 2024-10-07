using Microsoft.AspNetCore.Http;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Interface
{
    public interface IAttractionService
    {
        public Task<string> CreateAttraction(Attraction attraction);
        public Task DeleteAttraction(Attraction attraction);
        public Task UpdateAttraction(Attraction newAttraction);
        public Task<(int totalCount, List<Attraction> items)> GetAllAttractions(
            string? nameSearch,
            List<string>? provinceIds,
            List<string>? attractionTypeIds,
            AttractionStatus? status,
            int pageSize,
            int pageIndex);
        public Task<Attraction?> GetAttractionById(string attractionId);

        public Task UpdateAttractionImage(Attraction attraction, List<IFormFile>? imageFiles, List<string>? removedImageIds);
    }
}
