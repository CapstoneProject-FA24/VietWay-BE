using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
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
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXIST_STAFF");

            bool checkPassword = _hashHelper.Verify(oldPassword, staff.Account.Password);

            if (!checkPassword)
            {
                throw new InvalidActionException("INVALID_INFO_PASSWORD");
            }
            else if (oldPassword.Equals(newPassword))
            {
                throw new InvalidActionException("INVALID_INFO_SAME_PASSWORD");
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
                throw new ResourceNotFoundException("NOT_EXIST_STAFF");

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
        public async Task AdminResetStaffPassword(string staffId)
        {
            Staff? staff = await _unitOfWork.StaffRepository.Query()
                .Where(x => x.StaffId.Equals(staffId))
                .Include(x => x.Account)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("Staff not found");

            string newPassword = GeneratePassword();
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

        private static string GeneratePassword()
        {
            int length = GenerateRandomLength(8, 16);

            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()-_=+[]{}|;:,.<>?/`~";

            char[] password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                password[0] = GetRandomCharacter(rng, uppercase);
                password[1] = GetRandomCharacter(rng, lowercase);
                password[2] = GetRandomCharacter(rng, digits);
                password[3] = GetRandomCharacter(rng, special);

                string allCharacters = uppercase + lowercase + digits + special;
                for (int i = 4; i < length; i++)
                {
                    password[i] = GetRandomCharacter(rng, allCharacters);
                }

                ShuffleArray(rng, password);
            }

            return new string(password);
        }

        private static int GenerateRandomLength(int min, int max)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[1];
                rng.GetBytes(buffer);
                return min + (buffer[0] % (max - min + 1));
            }
        }

        private static char GetRandomCharacter(RandomNumberGenerator rng, string characterPool)
        {
            byte[] buffer = new byte[1];
            rng.GetBytes(buffer);
            int index = buffer[0] % characterPool.Length;
            return characterPool[index];
        }

        private static void ShuffleArray(RandomNumberGenerator rng, char[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                byte[] buffer = new byte[1];
                rng.GetBytes(buffer);
                int j = buffer[0] % (i + 1);

                char temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }
        public async Task<StaffDetailDTO?> GetStaffDetailAsync(string staffId)
        {
            return await _unitOfWork.StaffRepository
                .Query()
                .Where(x => x.StaffId.Equals(staffId))
                .Select(x => new StaffDetailDTO
                {
                    FullName = x.FullName,
                    PhoneNumber = x.Account.PhoneNumber,
                    Email = x.Account.Email,
                }).SingleOrDefaultAsync();
        }
    }
}
