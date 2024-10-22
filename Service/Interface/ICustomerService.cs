using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.DataTransferObject;

namespace VietWay.Service.Interface
{
    public interface ICustomerService
    {
        public Task<Customer?> GetCustomerById(string id);
        public Task<(int totalCount, List<Customer> items)> GetAllCustomers(
            string? nameSearch,
            int pageSize,
            int pageIndex);
        public Task RegisterAccountAsync(Customer customer);
        public Task<CustomerInfoDTO?> GetCustomerProfileInfo(string customerId);
        public Task UpdateCustomerProfileAsync(string customerId, string? fullName, DateTime? 
            dateOfBirth, string? provinceId, Gender? gender, string? email);
    }
}
