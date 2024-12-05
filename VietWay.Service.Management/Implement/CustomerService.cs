using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Management.Implement
{
    public class CustomerService(IUnitOfWork unitOfWork, IHashHelper hashHelper,
        IIdGenerator idGenerator, ITimeZoneHelper timeZoneHelper) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        public async Task<Customer?> GetCustomerById(string id)
        {
            return await _unitOfWork.CustomerRepository
                .Query()
                .Where(x => x.CustomerId.Equals(id))
                .Include(x => x.Account)
                .Include(x => x.Province)
                .FirstOrDefaultAsync();
        }

        public async Task<(int totalCount, List<CustomerPreviewDTO> items)> GetAllCustomers(
            string? nameSearch,
            int pageSize,
            int pageIndex)
        {
            var query = _unitOfWork
                .CustomerRepository
                .Query();
            if (!string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.FullName.Contains(nameSearch));
            }

            int count = await query.CountAsync();

            List<CustomerPreviewDTO> items = await query
                .Include(x => x.Account)
                .Include(x => x.Province)
                .OrderByDescending(x => x.Account.CreatedAt)
                .Select(x => new CustomerPreviewDTO
                {
                    CustomerId = x.CustomerId,
                    PhoneNumber = x.Account.PhoneNumber,
                    Email = x.Account.Email,
                    FullName = x.FullName,
                    DateOfBirth = x.DateOfBirth,
                    Province = x.Province.Name,
                    Gender = x.Gender,
                    CreatedAt = x.Account.CreatedAt,
                    IsDeleted = x.IsDeleted
                })
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();
            return (count, items);
        }

        public async Task<CustomerInfoDTO?> GetCustomerProfileInfo(string customerId)
        {
            return await _unitOfWork
                .CustomerRepository
                .Query()
                .Where(x => x.CustomerId.Equals(customerId) && false == x.IsDeleted)
                .Include(x => x.Account)
                .Include(x => x.Province)
                .Select(x => new CustomerInfoDTO
                {
                    PhoneNumber = x.Account.PhoneNumber,
                    Email = x.Account.Email,
                    FullName = x.FullName,
                    DateOfBirth = x.DateOfBirth,
                    ProvinceId = x.ProvinceId,
                    ProvinceName = x.Province.Name,
                    Gender = x.Gender
                })
                .SingleOrDefaultAsync();
        }

        public async Task RegisterAccountAsync(Customer customer)
        {
            Province? province = await _unitOfWork.ProvinceRepository.Query()
                .Where(x => x.ProvinceId.Equals(customer.ProvinceId) && false == x.IsDeleted)
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

        public async Task UpdateCustomerProfileAsync(string customerId, string? fullName, DateTime? dateOfBirth,
            string? provinceId, Gender? gender, string? email)
        {
            Customer? customer = await _unitOfWork.CustomerRepository.Query()
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

        public async Task ChangeCustomerStatus(string customerId, bool isDeleted)
        {
            Customer? customer = await _unitOfWork.CustomerRepository.Query()
                    .Include(x => x.Account)
                    .SingleOrDefaultAsync(x => x.CustomerId.Equals(customerId))
                    ?? throw new ResourceNotFoundException("Customer not found");
            customer.IsDeleted = isDeleted;
            customer.Account.IsDeleted = isDeleted;
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                await _unitOfWork.CommitTransactionAsync();
            } catch {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
