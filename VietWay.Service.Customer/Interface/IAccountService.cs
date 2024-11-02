using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Service.Customer.Interface
{
    public interface IAccountService
    {
        public Task<Account?> LoginAsync(string emailOrPhone, string password);
    }
}
