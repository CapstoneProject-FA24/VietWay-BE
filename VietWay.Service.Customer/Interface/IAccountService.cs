using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.Service.Customer.Interface
{
    public interface IAccountService
    {
        public Task<string> ConfirmResetPasswordOtpAsync(string phoneNumber, string otp);
        public Task<CredentialDTO?> LoginAsync(string emailOrPhone, string password);
        public Task<CredentialDTO?> LoginWithGoogleAsync(string idToken);
        public Task ResetPasswordAsync(string accountId, string phoneNumber, string newPassword);
        public Task SendResetPasswordOtpAsync(string phoneNumber);
    }
}
