using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IStaffService
    {
        public Task<(int totalCount, List<Staff> items)> GetAllStaffInfos(int pageSize, int pageIndex);
        public Task<Staff?> GetStaffInfoById(string id);
        public Task<Staff> EditStaffInfo(Staff staffInfo);
        public Task<Staff> AddStaff(Staff staffInfo);
        public Task RegisterAccountAsync(Staff staff);
    }
}
