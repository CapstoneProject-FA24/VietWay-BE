using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VietWay.API.Customer.RequestModel;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Implement;
using VietWay.Service.Interface;
using VietWay.Util.IdHelper;

namespace VietWay.API.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAccountService accountService, 
        IMapper mapper,
        IConfiguration configuration,
        ILogger<AccountController> logger,
        IIdGenerator idGenerator) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<AccountController> _logger = logger;
        private readonly IIdGenerator _idGenerator = idGenerator;

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginByEmailRequest login)
        {
            // Input validation
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Invalid client request");
            }

            // Attempt to authenticate user
            var user = await _accountService.LoginByEmailAsync(login.Email, login.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            // Check if the user has the appropriate role
            if (!user.Role.Equals(UserRole.Customer))
            {
                return Forbid("User does not have the appropriate permissions");
            }

            // Generate JWT token for the user
            var tokenString = GenerateJSONWebToken(user);
            return Ok(new { token = tokenString });
        }

        private string GenerateJSONWebToken(Account user)
        {
            // Replace "YourSecretKey" with a value retrieved from a secure configuration
            var secretKey = _configuration["Jwt:Key"]; // Assuming you're using IConfiguration to access configuration values
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Claims to include in the JWT token
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim("role", user.Role.ToString()),
            new Claim("accountId", user.AccountId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Replace with actual issuer, possibly from configuration
                audience: _configuration["Jwt:Audience"], // Replace with actual audience, possibly from configuration
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            Account account = _mapper.Map<Account>(request);

            await _accountService.CreateAccountAsync(account);

            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour created successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
