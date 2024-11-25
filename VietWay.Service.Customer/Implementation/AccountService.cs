using Microsoft.EntityFrameworkCore;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.UnitOfWork;
using VietWay.Service.Customer.Interface;
using VietWay.Service.ThirdParty.Firebase;
using VietWay.Service.ThirdParty.Redis;
using VietWay.Service.ThirdParty.Sms;
using VietWay.Util.CustomExceptions;
using VietWay.Util.HashUtil;
using VietWay.Util.OtpUtil;
using VietWay.Util.TokenUtil;

namespace VietWay.Service.Customer.Implementation
{
    public class AccountService(IUnitOfWork unitOfWork, IHashHelper hashHelper, IOtpGenerator otpGenerator, ISmsService smsService,
        IFirebaseService firebaseService, IRedisCacheService redisCacheService, ITokenHelper tokenHelper) : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IHashHelper _hashHelper = hashHelper;
        private readonly IFirebaseService _firebaseService = firebaseService;
        private readonly IRedisCacheService _redisCacheService = redisCacheService;
        private readonly IOtpGenerator _otpGenerator = otpGenerator;
        private readonly ISmsService _smsService = smsService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

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

        public async Task<string> ConfirmResetPasswordOtpAsync(string phoneNumber, string otp)
        {
            string? otpInCache = await _redisCacheService.GetAsync<string>(phoneNumber);
            if (otpInCache == null || false == _hashHelper.Verify(otp,otpInCache))
            {
                throw new InvalidInfoException("Invalid OTP");
            }
            Account? account = await _unitOfWork.AccountRepository
                .Query()
                .SingleOrDefaultAsync(x => x.PhoneNumber!.Equals(phoneNumber) && false == x.IsDeleted) ?? throw new InvalidInfoException("Invalid phone number");
            return _tokenHelper.GenerateResetPasswordToken(account.AccountId!, DateTime.Now.Add(_otpGenerator.GetOtpTimespan()));
        }

        public async Task ResetPasswordAsync(string accountId, string phoneNumber, string newPassword)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                Account? account = await _unitOfWork.AccountRepository
                    .Query()
                    .SingleOrDefaultAsync(x => x.AccountId!.Equals(accountId) && x.PhoneNumber!.Equals(phoneNumber) && false == x.IsDeleted) ??
                        throw new InvalidInfoException("Invalid phone number");
                bool phoneNumberHasResetPasswordRequest = await _redisCacheService.GetAsync<string>(phoneNumber) != null;
                if (phoneNumberHasResetPasswordRequest == false)
                {
                    throw new InvalidInfoException("Invalid phone number");
                }
                account.Password = _hashHelper.Hash(newPassword);
                await _unitOfWork.AccountRepository.UpdateAsync(account);
                await _unitOfWork.CommitTransactionAsync();
                await _redisCacheService.RemoveAsync(phoneNumber);
            } 
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task SendResetPasswordOtpAsync(string phoneNumber)
        {
            bool isCustomerAccountValid = await _unitOfWork.AccountRepository.Query()
                .AnyAsync(x=>x.PhoneNumber.Equals(phoneNumber) && false == x.IsDeleted && x.Role == UserRole.Customer);
            if (isCustomerAccountValid == false)
            {
                throw new InvalidInfoException("Invalid phone number");
            }
            string otp = _otpGenerator.GenerateOtp();
            bool result = await _smsService.SendOTP(otp, phoneNumber);
            if (result == false)
            {
                throw new ServerErrorException("Failed to send OTP");
            }

            await _redisCacheService.SetAsync(phoneNumber, _hashHelper.Hash(otp), _otpGenerator.GetOtpTimespan());
        }
    }
}
