using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ICustomerService
    {
        public Task<Customer?> GetCustomerById(string id);
        public Task<(int totalCount, List<CustomerPreviewDTO> items)> GetAllCustomers(
            string? nameSearch,
            int pageSize,
            int pageIndex);
        public Task RegisterAccountAsync(Customer customer);
        public Task<CustomerInfoDTO?> GetCustomerProfileInfo(string customerId);
        public Task UpdateCustomerProfileAsync(string customerId, string? fullName, DateTime?
            dateOfBirth, string? provinceId, Gender? gender, string? email);
        public Task ChangeCustomerStatus(string customerId, string managerId, bool isDeleted);
    }
}
