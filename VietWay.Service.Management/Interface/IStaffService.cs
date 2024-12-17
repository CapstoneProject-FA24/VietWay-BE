using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IStaffService
    {
        public Task<(int totalCount, List<StaffPreviewDTO> items)> GetAllStaffInfos(
            string? nameSearch,
            int pageSize,
            int pageIndex);
        public Task<Staff?> GetStaffInfoById(string id);
        public Task StaffChangePassword(string staffId, string oldPassword, string newPassword);
        public Task<Staff> AddStaff(Staff staffInfo);
        public Task RegisterAccountAsync(Staff staff);
        public Task ChangeStaffStatusAsync(string staffId, bool isDeleted);
        public Task<string> AdminResetStaffPassword(string staffId);
    }
}
