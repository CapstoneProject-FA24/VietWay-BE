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
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginByEmailRequest login)
        {
            if (login == null || string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Invalid client request");
            }

            var user = await _accountService.LoginByEmailAsync(login.Email, login.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            if (!user.Role.Equals(UserRole.Customer))
            {
                return Forbid("User does not have the appropriate permissions");
            }

            var tokenString = GenerateJSONWebToken(user);
            return Ok(new { token = tokenString });
        }

        private string GenerateJSONWebToken(Account user)
        {
            var secretKey = _configuration["Jwt:Key"]; 
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
            new Claim("role", user.Role.ToString()),
            new Claim("accountId", user.AccountId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], 
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost("CreateCustomerAccount")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequest request)
        {
            Account account = _mapper.Map<Account>(request);
            Repository.EntityModel.Customer customer = _mapper.Map<Repository.EntityModel.Customer>(request);

            await _accountService.CreateCustomerAccountAsync(account, customer);

            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour created successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
