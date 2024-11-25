using Microsoft.AspNetCore.Http;

namespace VietWay.Util.TokenUtil
{
    public interface ITokenHelper
    {
        public string GenerateAuthenticationToken(string accountId, string role);
        public string GenerateResetPasswordToken(string accountId, DateTime tokenExpireTime);
        public string? GetAccountIdFromToken(HttpContext httpContext);
    }
}
