using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

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
