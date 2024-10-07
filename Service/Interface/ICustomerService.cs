using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface ICustomerService
    {
        public Task<Customer?> GetCustomerById(string id);
        public Task<(int totalCount, List<Customer> items)> GetAllCustomers(
            string? nameSearch,
            int pageSize,
            int pageIndex);
    }
}
