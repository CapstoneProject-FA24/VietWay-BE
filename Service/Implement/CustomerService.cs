using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;

namespace VietWay.Service.Implement
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Customer?> GetCustomerById(string id)
        {
            return await _unitOfWork.CustomerInfoRepository
                .Query()
                .Where(x => x.CustomerId.Equals(id))
                .Include(x => x.Account)
                .Include(x => x.Province)
                .FirstOrDefaultAsync();
        }

        public async Task<(int totalCount, List<Customer> items)> GetAllCustomers(
            string? nameSearch,
            int pageSize,
            int pageIndex)
        {
            IQueryable<Customer> query = _unitOfWork.CustomerInfoRepository.Query();
            if (false == string.IsNullOrEmpty(nameSearch))
            {
                query = query.Where(x => x.FullName.Contains(nameSearch));
            }
            int count = await query.CountAsync();
            List<Customer> customers = await query
                .Include(x => x.Account)
                .Include(x => x.Province)
                .Skip(pageSize * (pageIndex - 1))
                .Take(pageSize)
                .ToListAsync();
            return (count, customers);
        }
    }
}
