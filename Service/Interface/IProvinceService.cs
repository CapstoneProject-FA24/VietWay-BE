using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IProvinceService
    {
        public Task<List<Province>> GetAllProvince();
        public Task<Province?> GetProvinceById(string id);
    }
}
