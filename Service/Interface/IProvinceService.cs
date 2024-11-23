using Microsoft.AspNetCore.Http;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IProvinceService
    {
        public Task<(int totalCount, List<ProvincePreviewDTO> items)> GetAllProvinces(
            string? nameSearch,
            int pageSize,
            int pageIndex);
        public Task<Province?> GetProvinceById(string id);
        public Task<(int count, List<ProvinceDetailDTO>)> GetAllProvinceDetails(string? nameSearch, string? zoneId, int pageIndex, int pageSize);
        public Task<string> CreateProvinceAsync(Province province);
        public Task UpdateProvinceAsync(Province newProvince);
        public Task UpdateProvinceImageAsync(string provinceId, IFormFile newImages);
        public Task DeleteProvinceAsync(string provinceId);
    }
}