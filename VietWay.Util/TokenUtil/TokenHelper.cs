using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace VietWay.Util.TokenUtil
{
    public class TokenHelper(TokenConfig config) : ITokenHelper
    {
        private readonly string _issuer = config.Issuer;
        private readonly string _audience = config.Audience;
        private readonly string _secret = config.Secret;
        private readonly int _tokenExpireAfterMinutes = config.TokenExpireAfterMinutes;

        public string GenerateAuthenticationToken(string accountId, string role)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha384);
            Dictionary<string, object> claims = new()
            {
                { CustomClaimType.AccountId, accountId},
                { ClaimTypes.Role, role}
            };
            SecurityTokenDescriptor descriptor = new()
            {
                Issuer = _issuer,
                Audience = _audience,
                Claims = claims,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(_tokenExpireAfterMinutes),
                SigningCredentials = credentials
            };
            return new JsonWebTokenHandler().CreateToken(descriptor);
        }

        public string GenerateResetPasswordToken(string accountId, DateTime tokenExpireTime)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_secret));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha384);
            Dictionary<string, object> claims = new()
            {
                { CustomClaimType.AccountId, accountId},
                {ClaimTypes.Role, nameof(CustomRole.ResettingPasswordCustomer) }
            };
            SecurityTokenDescriptor descriptor = new()
            {
                Issuer = _issuer,
                Audience = _audience,
                Claims = claims,
                NotBefore = DateTime.Now,
                Expires = tokenExpireTime,
                SigningCredentials = credentials
            };
            return new JsonWebTokenHandler().CreateToken(descriptor);
        }

        public string? GetAccountIdFromToken(HttpContext httpContext)
        {
            string? authorizationString = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(authorizationString) || false == authorizationString.StartsWith("Bearer "))
                return null;
            string jwtTokenString = authorizationString["Bearer ".Length..];
            JsonWebTokenHandler tokenHandler = new();
            JsonWebToken jwtToken = tokenHandler.ReadJsonWebToken(jwtTokenString);
            Claim? idClaim = jwtToken.Claims.SingleOrDefault(c => c.Type == CustomClaimType.AccountId);
            return idClaim?.Value;
        }
    }
    public static class CustomClaimType
    {
        public static string AccountId { get; } = "AccountId";
    }
}
