using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;
using VietWay.Service.Management.Interface;

namespace VietWay.Service.Management.Implement
{
    public class StaffService(IUnitOfWork unitOfWork, IHashHelper hashHelper,
        IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper) : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;

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

        public async Task RegisterAccountAsync(Staff staff)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                string accountId = _idGenerator.GenerateId();
                staff.StaffId = accountId;
                staff.Account.AccountId = accountId;
                staff.Account.Password = _hashHelper.Hash(staff.Account.Password);
                staff.Account.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.StaffRepository.CreateAsync(staff);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
