using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.Service.Management.Interface
{
    public interface IAccountService
    {
        public Task<CredentialDTO?> LoginAsync(string emailOrPhone, string password);
    }
}
