using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IAttractionService
    {
        public Task<(int count, List<AttractionPreviewDTO>)> GetAttractionsPreviewAsync(string? nameSearch, 
            List<string>? provinceIds, List<string>? attractionTypeIds, int pageSize, int pageIndex);

        public Task<AttractionDetailDTO?> GetAttractionDetailByIdAsync(string attractionId);
    }
}
