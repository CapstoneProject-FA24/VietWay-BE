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
    public class CustomerService(IUnitOfWork unitOfWork) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
        public async Task ChangeCustomerStatus(string customerId, bool isDeleted)
        {
            Customer? customer = await _unitOfWork.CustomerRepository.Query()
                    .Include(x => x.Account)
                    .SingleOrDefaultAsync(x => x.CustomerId.Equals(customerId))
                    ?? throw new ResourceNotFoundException("NOT_EXIST_CUSTOMER");
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
