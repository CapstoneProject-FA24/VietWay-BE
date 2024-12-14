using Azure.Core;
using IdGen;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.HashUtil;
using VietWay.Util.IdUtil;
using VietWay.Util.TokenUtil;
using System;

namespace VietWay.Service.Management.Implement
{
    public class AccountService(IUnitOfWork unitOfWork, ILogger<AccountService> logger, IHashHelper hashHelper, ITokenHelper tokenHelper) : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<AccountService> _logger = logger;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        public async Task<CredentialDTO?> LoginAsync(string emailOrPhone, string password)
        {
            CredentialDTO? credential = null;
            string fullName = string.Empty;
            Account? account = await _unitOfWork.AccountRepository
                .Query()
                .SingleOrDefaultAsync(x => (x.PhoneNumber.Equals(emailOrPhone) || x.Email.Equals(emailOrPhone)) && false == x.IsDeleted);

            if (emailOrPhone.Equals(Environment.GetEnvironmentVariable("EMAIL")) &&
                        password.Equals(Environment.GetEnvironmentVariable("PASSWORD")))
            {
                fullName = "Admin";
                credential = new()
                {
                    AvatarUrl = default!,
                    FullName = fullName,
                    Role = account.Role,
                    Token = _tokenHelper.GenerateAuthenticationToken("1", account.Role.ToString())
                };
            }

            else if (account == null || false == _hashHelper.Verify(password, account.Password))
            {
                return null;
            }
            switch (account.Role)
            {
                case UserRole.Staff:
                    Staff? staff = await _unitOfWork.StaffRepository
                        .Query()
                        .SingleOrDefaultAsync(x => x.StaffId == account.AccountId);
                    if (staff != null)
                    {
                        fullName = staff.FullName;
                    }
                    break;
                case UserRole.Manager:
                    Manager? manager = await _unitOfWork.ManagerRepository
                        .Query()
                        .SingleOrDefaultAsync(x => x.ManagerId == account.AccountId);
                    if (manager != null)
                    {
                        fullName = manager.FullName;
                    }
                    break;
            }
            
            credential = new()
            {
                AvatarUrl = default!,
                FullName = fullName,
                Role = account.Role,
                Token = _tokenHelper.GenerateAuthenticationToken(account.AccountId, account.Role.ToString())
            };

            return credential;
        }
    }
}
