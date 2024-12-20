using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;
using VietWay.Service.Management.Interface;
using VietWay.Service.Management.DataTransferObject;
using System.Security.Cryptography;
using VietWay.Job.Interface;
using Hangfire;

namespace VietWay.Service.Management.Implement
{
    public class ManagerService(IUnitOfWork unitOfWork, IHashHelper hashHelper,
        IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper, IBackgroundJobClient backgroundJobClient) : IManagerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IBackgroundJobClient _backgroundJobClient = backgroundJobClient;

        public async Task<(int totalCount, List<ManagerPreviewDTO> items)> GetAllManagerInfos(string? nameSearch,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .ManagerRepository
                .Query();
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.FullName.Contains(nameSearch));
            }
            int count = await query.CountAsync();
            List<ManagerPreviewDTO> items = await query
                .Include(x => x.Account)
                .Select(x => new ManagerPreviewDTO
                {
                    ManagerId = x.ManagerId,
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
        public async Task<Manager?> GetManagerInfoById(string id)
        {
            return await _unitOfWork
                .ManagerRepository
                .Query()
                .Include(x => x.Account)
                .SingleOrDefaultAsync(x => x.ManagerId.Equals(id));
        }
        public async Task ManagerChangePassword(string managerId, string oldPassword, string newPassword)
        {
            Manager? manager = await _unitOfWork.ManagerRepository.Query()
                .Where(x => x.ManagerId.Equals(managerId))
                .Include(x => x.Account)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXIST_MANAGER");

            bool checkPassword = _hashHelper.Verify(oldPassword, manager.Account.Password);

            if (!checkPassword)
            {
                throw new InvalidInfoException("INVALID_INFO_PASSWORD");
            }
            else if (oldPassword.Equals(newPassword))
            {
                throw new InvalidInfoException("INVALID_INFO_SAME_PASSWORD");
            }

            manager.Account.Password = _hashHelper.Hash(newPassword);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.ManagerRepository.UpdateAsync(manager);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<Manager> AddManager(Manager managerInfo)
        {
            await _unitOfWork.ManagerRepository
                .CreateAsync(managerInfo);
            return managerInfo;
        }

        public async Task RegisterAccountAsync(Manager manager)
        {
            Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.PhoneNumber.Equals(manager.Account.PhoneNumber) || x.Email.Equals(manager.Account.Email));
            if (account != null)
            {
                throw new InvalidActionException("EXISTED_PHONE_OR_EMAIL");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                string accountId = _idGenerator.GenerateId();
                string newPassword = GeneratePassword();
                manager.ManagerId = accountId;
                manager.Account.AccountId = accountId;
                manager.Account.Password = _hashHelper.Hash(newPassword);
                manager.Account.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.ManagerRepository.CreateAsync(manager);
                await _unitOfWork.CommitTransactionAsync();
                _backgroundJobClient.Enqueue<IEmailJob>(x => x.SendNewPasswordEmail(manager.Account.Email, manager.FullName, newPassword));
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task ChangeManagerStatusAsync(string managerId, bool isDeleted)
        {
            Manager? manager = await _unitOfWork.ManagerRepository.Query()
                .SingleOrDefaultAsync(x => x.ManagerId.Equals(managerId)) ??
                throw new ResourceNotFoundException("NOT_EXIST_MANAGER");

            manager.IsDeleted = isDeleted;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.ManagerRepository.UpdateAsync(manager);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        public async Task AdminResetManagerPassword(string managerId)
        {
            Manager? manager = await _unitOfWork.ManagerRepository.Query()
                .Where(x => x.ManagerId.Equals(managerId))
                .Include(x => x.Account)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("Manager not found");

            string newPassword = GeneratePassword();
            manager.Account.Password = _hashHelper.Hash(newPassword);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.ManagerRepository.UpdateAsync(manager);
                await _unitOfWork.CommitTransactionAsync();
                _backgroundJobClient.Enqueue<IEmailJob>(x => x.SendNewPasswordEmail(manager.Account.Email, manager.FullName, newPassword));
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
        public async Task<ManagerDetailDTO?> GetManagerDetailAsync(string managerId)
        {
            return await _unitOfWork.ManagerRepository
                .Query()
                .Where(x => x.ManagerId.Equals(managerId))
                .Select(x => new ManagerDetailDTO
                {
                    FullName = x.FullName,
                    PhoneNumber = x.Account.PhoneNumber,
                    Email = x.Account.Email,
                }).SingleOrDefaultAsync();
        }
    }
}
