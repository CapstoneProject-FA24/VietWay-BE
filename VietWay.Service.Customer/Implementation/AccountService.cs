using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.Interface;
using VietWay.Service.ThirdParty.Firebase;
using VietWay.Util.HashUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class AccountService(IUnitOfWork unitOfWork, IHashHelper hashHelper, IFirebaseService firebaseService) : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly IFirebaseService _firebaseService = firebaseService;


        public async Task<Account?> LoginWithGoogleAsync(string idToken)
        {
            string email = await _firebaseService.GetEmailFromIdToken(idToken);
            Account? account = await _unitOfWork.AccountRepository
                .Query()
                .SingleOrDefaultAsync(x => (x.Email.Equals(email)) && false == x.IsDeleted);

            return account;
        }

        public async Task<Account?> LoginAsync(string emailOrPhone, string password)
        {
            Account? account = await _unitOfWork.AccountRepository
                .Query()
                .SingleOrDefaultAsync(x => (x.PhoneNumber.Equals(emailOrPhone) || x.Email.Equals(emailOrPhone)) && false == x.IsDeleted);
            if (account == null || false == _hashHelper.Verify(password, account.Password))
            {
                return null;
            }

            return account;
        }
    }
}
