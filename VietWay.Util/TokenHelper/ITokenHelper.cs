using Microsoft.AspNetCore.Http;
using VietWay.Repository.EntityModel;

namespace VietWay.Util.TokenHelper
{
    public interface ITokenHelper
    {
        public string GenerateAuthenticationToken(Account account);

        public long? GetAccountIdFromToken(HttpContext httpContext);
    }
}
