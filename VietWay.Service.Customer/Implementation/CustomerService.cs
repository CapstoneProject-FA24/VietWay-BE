using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Service.ThirdParty.Firebase;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class CustomerService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, IHashHelper hashHelper, ITimeZoneHelper timeZoneHelper, IFirebaseService firebaseService) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly IFirebaseService _firebaseService = firebaseService;

        public async Task<CustomerDetailDTO?> GetCustomerDetailAsync(string customerId)
        {
            return await _unitOfWork.CustomerRepository
                .Query()
                .Where(x => x.CustomerId.Equals(customerId))
                .Select(x => new CustomerDetailDTO
                {
                    FullName = x.FullName,
                    PhoneNumber = x.Account.PhoneNumber,
                    ProvinceId = x.ProvinceId,
                    ProvinceName = x.Province.Name,
                    DateOfBirth = x.DateOfBirth,
                    Email = x.Account.Email,
                    Gender = x.Gender,
                    LoginWithGoogle = x.Account.Password == string.Empty
                }).SingleOrDefaultAsync();
        }

        public async Task RegisterAccountAsync(Repository.EntityModel.Customer customer)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Province? province = await _unitOfWork.ProvinceRepository.Query()
                    .SingleOrDefaultAsync(x => x.ProvinceId.Equals(customer.ProvinceId) && false == x.IsDeleted) ??
                    throw new ResourceNotFoundException("NOT_EXIST_PROVINCE");

                Account? account = await _unitOfWork.AccountRepository.Query()
                    .SingleOrDefaultAsync(x => x.PhoneNumber.Equals(customer.Account.PhoneNumber) || x.Email.Equals(customer.Account.Email));
                if (account != null)
                {
                    throw new InvalidActionException("EXISTED_PHONE_OR_EMAIL");
                }
                string accountId = _idGenerator.GenerateId();
                customer.CustomerId = accountId;
                customer.Account.AccountId = accountId;
                customer.Account.Password = _hashHelper.Hash(customer.Account.Password);
                customer.Account.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.CustomerRepository.CreateAsync(customer);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task RegisterAccountWithGoogleAsync(Repository.EntityModel.Customer customer, string idToken)
        {
            string email = await _firebaseService.GetEmailFromIdToken(idToken);
            Province? province = await _unitOfWork.ProvinceRepository.Query()
                .SingleOrDefaultAsync(x => x.ProvinceId.Equals(customer.ProvinceId) && false == x.IsDeleted) ??
                throw new ResourceNotFoundException("NOT_EXIST_PROVINCE");

            Account? account = await _unitOfWork.AccountRepository.Query()
                .SingleOrDefaultAsync(x => x.PhoneNumber.Equals(customer.Account.PhoneNumber) || x.Email.Equals(email));
            if (account != null)
            {
                throw new InvalidActionException("EXISTED_PHONE_OR_EMAIL");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                string accountId = _idGenerator.GenerateId();
                customer.Account.Email = email;
                customer.CustomerId = accountId;
                customer.Account.AccountId = accountId;
                customer.Account.Password = string.Empty;
                customer.Account.CreatedAt = _timeZoneHelper.GetUTC7Now();
                await _unitOfWork.CustomerRepository.CreateAsync(customer);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task UpdateCustomerInfoAsync(string customerId,string? fullName, DateTime? dateOfBirth, string? provinceId, Gender? gender, string? email)
        {
            VietWay.Repository.EntityModel.Customer? customer = await _unitOfWork.CustomerRepository.Query()
                .Where(x => x.CustomerId.Equals(customerId) && false == x.IsDeleted)
                .Include(x => x.Account)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("NOT_EXIST_CUSTOMER");
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (false == string.IsNullOrEmpty(fullName))
                {
                    customer.FullName = fullName;
                }
                if (dateOfBirth.HasValue)
                {
                    customer.DateOfBirth = dateOfBirth.Value;
                }
                if (false == string.IsNullOrEmpty(provinceId))
                {
                    customer.ProvinceId = provinceId;
                }
                if (gender.HasValue)
                {
                    customer.Gender = gender.Value;
                }
                if (false == string.IsNullOrEmpty(email))
                {
                    customer.Account.Email = email;
                }
                await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CustomerChangePassword(string customerId, string oldPassword, string newPassword)
        {
            VietWay.Repository.EntityModel.Customer? customer = await _unitOfWork.CustomerRepository.Query()
                .Where(x => x.CustomerId.Equals(customerId))
                .Include(x => x.Account)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("Manager not found");

            bool checkPassword = _hashHelper.Verify(oldPassword, customer.Account.Password);

            if (!checkPassword)
            {
                throw new InvalidActionException("Incorrect password");
            }
            else if (oldPassword.Equals(newPassword))
            {
                throw new InvalidActionException("Your new password cannot be the same as your current password.");
            }

            customer.Account.Password = _hashHelper.Hash(newPassword);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.CustomerRepository.UpdateAsync(customer);
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
