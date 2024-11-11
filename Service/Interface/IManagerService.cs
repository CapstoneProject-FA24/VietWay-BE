using VietWay.Repository.EntityModel;

namespace VietWay.Service.Management.Interface
{
    public interface IManagerService
    {
        public Task<(int totalCount, List<Manager> items)> GetAllManagerInfos(int pageSize, int pageIndex);
        public Task<Manager?> GetManagerInfoById(string id);
        public Task<Manager> EditManagerInfo(Manager managerInfo);
        public Task<Manager> AddManager(Manager managerInfo);
        public Task RegisterAccountAsync(Manager manager);
        public Task ChangeManagerStatusAsync(string managerId, bool isDeleted);
    }
}
