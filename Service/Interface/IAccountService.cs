using VietWay.Repository.EntityModel;

namespace VietWay.Service.Interface
{
    public interface IAccountService
    {
        public Task<Account?> LoginByPhone(string phone, string password);
        public Task<Account?> LoginByEmail(string email, string password);
    }
}
