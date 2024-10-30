using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaffService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Staff> AddStaff(Staff staffInfo)
        {
            await _unitOfWork.StaffRepository
                .CreateAsync(staffInfo);
            return staffInfo;
        }

        public async Task<Staff> EditStaffInfo(Staff staffInfo)
        {
            await _unitOfWork.StaffRepository
                .UpdateAsync(staffInfo);
            return staffInfo;
        }

        public async Task<(int totalCount, List<Staff> items)> GetAllStaffInfos(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .StaffRepository
                .Query();
            int count = await query.CountAsync();
            List<Staff> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Account)
                .ToListAsync();
            return (count, items);
        }

        public async Task<Staff?> GetStaffInfoById(string id)
        {
            return await _unitOfWork.StaffRepository
                .Query()
                .Where(x => x.StaffId.Equals(id))
                .Include(x => x.Account)
                .FirstOrDefaultAsync();
        }
    }
}
