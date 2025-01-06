using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IAttractionService
    {
        public Task<PaginatedList<AttractionPreviewDTO>> GetAttractionsPreviewAsync(string? nameSearch, 
            List<string>? provinceIds, List<string>? attractionTypeIds,string? customerId, int pageSize, int pageIndex);

        public Task<AttractionDetailDTO?> GetAttractionDetailByIdAsync(string attractionId, string? customerId, SocialMediaSite? socialMediaSite);
        Task ToggleAttractionLikeAsync(string attractionId, string customerId, bool isLike);
        Task<PaginatedList<AttractionPreviewDTO>> GetCustomerLikedAttractionsAsync(string customerId, int pageSize, int pageIndex);
    }
}
