using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Interface
{
    public interface IAttractionService
    {
        public Task CreateAttraction(Attraction attraction, List<IFormFile> images, bool isDraft);
        public Task DeleteAttraction(string attractionId);
        public Task UpdateAttraction(Attraction attraction, List<IFormFile> images);
        public Task<(int totalCount, List<Attraction> items)> GetAllAttractions(
            string? nameSearch,
            List<string>? provinceIds,
            List<string>? attractionTypeIds,
            AttractionStatus? status,
            int pageSize,
            int pageIndex);
        public Task<Attraction?> GetAttractionById(string attractionId);
    }
}
