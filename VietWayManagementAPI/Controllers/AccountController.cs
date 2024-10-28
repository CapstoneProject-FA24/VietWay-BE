using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;
using VietWay.Util.TokenUtil;
using UserRole = VietWay.Repository.EntityModel.Base.UserRole;  

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Account API endpoints
    /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AccountController(IAccountService accountService, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// ✅ Login with email/phone and password
        /// </summary>
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            Account? account = await _accountService.LoginAsync(request.EmailOrPhone, request.Password);
            if (account == null || account.Role == UserRole.Customer)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Email or password is incorrect"
                });
            }
            ManagementAccountLoginDTO result = new ManagementAccountLoginDTO
            {   
                Token = _tokenHelper.GenerateAuthenticationToken(account.AccountId, account.Role.ToString()),
                Role = account.Role
            };
            return Ok(new DefaultResponseModel<ManagementAccountLoginDTO>()
            {
                Message = "Login successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }
    }
}
