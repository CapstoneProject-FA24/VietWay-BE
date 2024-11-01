using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Implement
{
    public class ManagerService(IUnitOfWork unitOfWork, IHashHelper hashHelper,
        IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper) : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;

        public async Task<(int totalCount, List<Manager> items)> GetAllManagerInfos(int pageSize, int pageIndex)
        {
            var query = _unitOfWork
                .ManagerRepository
                .Query();
            int count = await query.CountAsync();
            List<Manager> items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Include(x => x.Account)
                .ToListAsync();
            return (count, items);
        }
        public async Task<Manager?> GetManagerInfoById(string id)
        {
            return await _unitOfWork
                .ManagerRepository
                .Query()
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.ManagerId.Equals(id));
        }
        public async Task<Manager> EditManagerInfo(Manager managerInfo)
        {
            await _unitOfWork.ManagerRepository
                .UpdateAsync(managerInfo);
            return managerInfo;
        }

        public async Task<Manager> AddManager(Manager managerInfo)
        {
            await _unitOfWork.ManagerRepository
                .CreateAsync(managerInfo);
            return managerInfo;
        }

        public async Task RegisterAccountAsync(Manager manager)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                string accountId = _idGenerator.GenerateId();
                manager.ManagerId = accountId;
                manager.Account.AccountId = accountId;
                manager.Account.Password = _hashHelper.Hash(manager.Account.Password);
                manager.Account.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.ManagerRepository.CreateAsync(manager);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeactivateStaffAccountAsync(Staff newStaff)
        {
            Staff? staff = await _unitOfWork.StaffRepository.Query()
                .SingleOrDefaultAsync(x => x.StaffId.Equals(newStaff.StaffId)) ??
                throw new ResourceNotFoundException("Staff not found");

            staff.IsDeleted = newStaff.IsDeleted;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.StaffRepository.UpdateAsync(staff);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task ActivateStaffAccountAsync(Staff newStaff)
        {
            Staff? staff = await _unitOfWork.StaffRepository.Query()
                .SingleOrDefaultAsync(x => x.StaffId.Equals(newStaff.StaffId)) ??
                throw new ResourceNotFoundException("Staff not found");

            staff.IsDeleted = newStaff.IsDeleted;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.StaffRepository.UpdateAsync(staff);
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
