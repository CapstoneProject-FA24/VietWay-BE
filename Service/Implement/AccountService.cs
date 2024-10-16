using Azure.Core;
using IdGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;
using VietWay.Util.IdHelper;

namespace VietWay.Service.Implement
{
    public class AccountService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, ILogger<AccountService> logger) : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<AccountService> _logger = logger;
        private readonly IIdGenerator _idGenerator = idGenerator;
        public async Task<Account?> LoginByPhone(string phone, string password)
        {
            Account? account = await _unitOfWork.AccountRepository
                .Query()
                .SingleOrDefaultAsync(x => x.PhoneNumber.Equals(phone) && x.Password.Equals(password));
            return account;
        }
        
        public async Task<Account?> LoginByEmailAsync(string email, string password)
        {
            Account? account = await _unitOfWork.AccountRepository
                .Query()
                .SingleOrDefaultAsync(x => x.Email.Equals(email) || x.PhoneNumber.Equals(email));

            if (account == null)
            {
                return null;
            }

            bool isPasswordValid = VerifyPasswordHash(password, account.Password);

            return isPasswordValid ? account : null;
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        public async Task<string> CreateCustomerAccountAsync(Account account, Customer customer)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }

            Account newAccount = new Account
            {
                AccountId = _idGenerator.GenerateId(),
                Email = account.Email,
                Password = HashPassword(account.Password),
                PhoneNumber = account.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                Role = UserRole.Customer
            };

            Customer newCustomer = new Customer
            {
                CustomerId = newAccount.AccountId,
                FullName = customer.FullName,
                DateOfBirth = customer.DateOfBirth,
                ProvinceId = customer.ProvinceId,
                Gender = customer.Gender,
                IsDeleted = false,
            };
            await _unitOfWork.AccountRepository.Create(newAccount);
            await _unitOfWork.CustomerInfoRepository.Create(newCustomer);
            return account.AccountId;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
