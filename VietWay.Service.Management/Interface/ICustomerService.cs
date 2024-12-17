using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface ICustomerService
    {
        public Task<(int totalCount, List<CustomerPreviewDTO> items)> GetAllCustomers(
            string? nameSearch,
            int pageSize,
            int pageIndex);
        public Task ChangeCustomerStatus(string customerId, bool isDeleted);
    }
}
