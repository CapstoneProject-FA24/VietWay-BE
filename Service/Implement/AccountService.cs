using Azure.Core;
using IdGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Interface;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;

namespace VietWay.Service.Implement
{
    public class AccountService(IUnitOfWork unitOfWork, ILogger<AccountService> logger, IHashHelper hashHelper) : IAccountService
    {
        public readonly IUnitOfWork _unitOfWork = unitOfWork;
        public readonly ILogger<AccountService> _logger = logger;
        public readonly IHashHelper _hashHelper = hashHelper;
        public async Task<Account?> LoginAsync(string emailOrPhone, string password)
        {
            Account? account = await _unitOfWork.AccountRepository
                .Query()
                .SingleOrDefaultAsync(x => (x.PhoneNumber.Equals(emailOrPhone) || x.Email.Equals(emailOrPhone)) && false == x.IsDeleted);
            if (account == null || false == _hashHelper.Verify(password,account.Password))
            {
                return null;
            }

            return account;
        }
    }
}
