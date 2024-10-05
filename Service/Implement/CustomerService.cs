using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
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

        public async Task<Customer?> GetCurrentCustomerProfile(string id)
        {
            return await _unitOfWork.CustomerInfoRepository
                .Query()
                .Where(x => x.CustomerId.Equals(id))
                .Include(x => x.Account)
                .FirstOrDefaultAsync();
        }
    }
}
