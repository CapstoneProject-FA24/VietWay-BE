using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;

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

        public async Task StaffChangePassword(string staffId, string oldPassword, string newPassword)
        {
            Staff? staff = await _unitOfWork.StaffRepository.Query()
                .Where(x => x.StaffId.Equals(staffId))
                .Include(x => x.Account)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("Staff not found");

            bool checkPassword = _hashHelper.Verify(oldPassword, staff.Account.Password);

            if (!checkPassword)
            {
                throw new InvalidActionException("Incorrect password");
            }
            else if (oldPassword.Equals(newPassword))
            {
                throw new InvalidActionException("Your new password cannot be the same as your current password.");
            }

            staff.Account.Password = _hashHelper.Hash(newPassword);

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

        public async Task<(int totalCount, List<StaffPreviewDTO> items)> GetAllStaffInfos(
            string? nameSearch,
            int pageSize, 
            int pageIndex)
        {
            var query = _unitOfWork
                .StaffRepository
                .Query();
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.FullName.Contains(nameSearch));
            }
            int count = await query.CountAsync();
            List<StaffPreviewDTO> items = await query
                .Include(x => x.Account)
                .Select(x => new StaffPreviewDTO
                {
                    StaffId = x.StaffId,
                    PhoneNumber = x.Account.PhoneNumber,
                    Email = x.Account.Email,
                    FullName = x.FullName,
                    CreatedAt = x.Account.CreatedAt,
                    IsDeleted = x.IsDeleted
                })
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
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
                staff.Account.Password = _hashHelper.Hash("VietWay@12345");
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

        public async Task ChangeStaffStatusAsync(string staffId, bool isDeleted)
        {
            Staff? staff = await _unitOfWork.StaffRepository.Query()
                .SingleOrDefaultAsync(x => x.StaffId.Equals(staffId)) ??
                throw new ResourceNotFoundException("Staff not found");

            staff.IsDeleted = isDeleted;
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
