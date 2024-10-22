using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface IProvinceService
    {
        public Task<List<ProvincePreviewDTO>> GetAllProvinces();
        public Task<Province?> GetProvinceById(string id);
        public Task<(int count, List<ProvinceDetailDTO>)> GetAllProvinceDetails(string? nameSearch, string? zoneId ,int pageIndex, int pageSize);
    }
}