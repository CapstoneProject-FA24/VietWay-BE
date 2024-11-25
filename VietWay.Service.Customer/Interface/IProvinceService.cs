using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IProvinceService
    {
        Task<ProvinceWithImageDTO?> GetProvinceImagesAsync(string provinceId, int imageCount);
        public Task<List<ProvincePreviewDTO>> GetProvinces();
        public Task<PaginatedList<ProvinceDetailDTO>> GetProvincesDetails(string? nameSearch, string? zoneId, int pageIndex, int pageSize);
    }
}
