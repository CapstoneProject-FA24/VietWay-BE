using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class CustomerService(IUnitOfWork unitOfWork, IIdGenerator idGenerator, IHashHelper hashHelper, ITimeZoneHelper timeZoneHelper) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
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
                    Email = x.Account.PhoneNumber,
                    Gender = x.Gender,
                }).SingleOrDefaultAsync();
        }

        public async Task RegisterAccountAsync(Repository.EntityModel.Customer customer)
        {
            Province? province = await _unitOfWork.ProvinceRepository.Query()
                .Where(x => x.ProvinceId.Equals(customer.ProvinceId) && false == x.IsDeleted)
                .SingleOrDefaultAsync();
            Account? account = await _unitOfWork.AccountRepository.Query()
                .Where(x => x.PhoneNumber.Equals(customer.Account.PhoneNumber) || x.Email.Equals(customer.Account.Email))
                .SingleOrDefaultAsync();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
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

        public async Task UpdateCustomerInfoAsync(string customerId,string? fullName, DateTime? dateOfBirth, string? provinceId, Gender? gender, string? email)
        {
            VietWay.Repository.EntityModel.Customer? customer = await _unitOfWork.CustomerRepository.Query()
                .Where(x => x.CustomerId.Equals(customerId) && false == x.IsDeleted)
                .Include(x => x.Account)
                .SingleOrDefaultAsync() ?? throw new ResourceNotFoundException("Customer not found");
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
    }
}
