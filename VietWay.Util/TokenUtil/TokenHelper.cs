using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace VietWay.Util.TokenUtil
{
    public class TokenHelper : ITokenHelper
    {
        public readonly string _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? throw new Exception("JWT_ISSUER is not set in environment variables");
        public readonly string _audience = Environment.GetEnvironmentVariable("JWT_ISSUER")
            ?? throw new Exception("JWT_ISSUER is not set in environment variables");
        public readonly string _secret = Environment.GetEnvironmentVariable("JWT_KEY")
            ?? throw new Exception("JWT_KEY is not set in environment variables");

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
                Expires = DateTime.Now.AddDays(30),
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
