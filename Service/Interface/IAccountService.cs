using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Interface
{
    public interface IAccountService
    {
        public Task<Account?> LoginByPhone(string phone, string password);
        public Task<Account?> LoginByEmailAsync(string email, string password);
        public Task<string> CreateAccountAsync(Account account);
    }
}
