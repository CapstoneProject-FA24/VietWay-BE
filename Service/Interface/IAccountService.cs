using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Interface
{
    public interface IAccountService
    {
        public Task<Account?> LoginAsync(string emailOrPhone, string password);
    }
}
