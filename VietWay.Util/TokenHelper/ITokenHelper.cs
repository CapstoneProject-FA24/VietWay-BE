using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel;

namespace VietWay.Util.TokenHelper
{
    public interface ITokenHelper
    {
        public string GenerateAuthenticationToken(Account account);

        public long? GetAccountIdFromToken(HttpContext httpContext);
    }
}
