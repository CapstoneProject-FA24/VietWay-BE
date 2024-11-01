using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface ICustomerService
    {
        Task<CustomerDetailDTO?> GetCustomerDetailAsync(string customerId);
        public Task RegisterAccountAsync(Repository.EntityModel.Customer account);
        Task UpdateCustomerInfoAsync(string customerId, string? fullName, DateTime? dateOfBirth, string? provinceId, Gender? gender, string? email);
    }
}
