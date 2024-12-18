using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IManagerService
    {
        public Task<(int totalCount, List<ManagerPreviewDTO> items)> GetAllManagerInfos(string? nameSearch,
            int pageSize,
            int pageIndex);
        public Task<Manager?> GetManagerInfoById(string id);
        public Task ManagerChangePassword(string managerId, string oldPassword, string newPassword);
        public Task<Manager> AddManager(Manager managerInfo);
        public Task RegisterAccountAsync(Manager manager);
        public Task ChangeManagerStatusAsync(string managerId, bool isDeleted);
        public Task AdminResetManagerPassword(string managerId);
        public Task<ManagerDetailDTO?> GetManagerDetailAsync(string managerId);
    }
}
