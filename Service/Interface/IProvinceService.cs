using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IProvinceService
    {
        public Task<List<ProvincePreviewDTO>> GetAllProvinces();
        public Task<Province?> GetProvinceById(string id);
        public Task<(int count, List<ProvinceDetailDTO>)> GetAllProvinceDetails(string? nameSearch, string? zoneId, int pageIndex, int pageSize);
        public Task<string> CreateProvinceAsync(Province province);
    }
}