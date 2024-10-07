using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using VietWay.Repository.EntityModel;

namespace VietWay.Util.TokenHelper
{
    public class TokenHelper : ITokenHelper
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _secret;
        public TokenHelper(IConfiguration configuration)
        {
            string? environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            _issuer = configuration["Jwt:Issuer"]
                ?? throw new Exception("Can not load jwt issuer");
            _audience = configuration["Jwt:Audience"]
                ?? throw new Exception("Can not load jwt audience");
            if (environment == "Development")
            {
                _secret = configuration["Jwt:Key"]
                    ?? throw new Exception("Can not find jwt secret key");
            } else
            {
                _secret = Environment.GetEnvironmentVariable("PROD_JWT_KEY")
                    ?? throw new Exception("Can not find jwt secret key");
            }
        }

        public string GenerateAuthenticationToken(Account account)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha384);
            Dictionary<string, object> claims = new()
            {
                { CustomClaimType.AccountId, account.AccountId.ToString() },
                { ClaimTypes.Role, account.Role.ToString() }
            };
            SecurityTokenDescriptor descriptor = new()
            {
                Issuer = _issuer,
                Audience = _audience,
                Claims = claims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credentials
            };
            return new JsonWebTokenHandler().CreateToken(descriptor);
        }

        public long? GetAccountIdFromToken(HttpContext httpContext)
        {
            string? authorizationString = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authorizationString) || false == authorizationString.StartsWith("Bearer "))
                return null;
            string jwtTokenString = authorizationString["Bearer ".Length..];
            JsonWebTokenHandler tokenHandler = new();
            JsonWebToken jwtToken = tokenHandler.ReadJsonWebToken(jwtTokenString);
            Claim? idClaim = jwtToken.Claims.SingleOrDefault(c => c.Type == CustomClaimType.AccountId);
            return long.TryParse(idClaim?.Value, out long accountId) ? accountId : null;
        }
    }
    public static class CustomClaimType
    {
        public static string AccountId { get; } = "AccountId";
    }
}
