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
        public Task<List<ProvincePreviewDTO>> GetProvinces();
        public Task<(int, List<ProvinceDetailDTO>)> GetProvincesDetails(string? nameSearch, string? zoneId, int pageIndex, int pageSize);
    }
}
