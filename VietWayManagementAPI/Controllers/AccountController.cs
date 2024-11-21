using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;
using UserRole = VietWay.Repository.EntityModel.Base.UserRole;

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Account API endpoints
    /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AccountController(IAccountService accountService, 
        IManagerService managerService,
        ITokenHelper tokenHelper, 
        IMapper mapper,
        IStaffService staffService) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IMapper _mapper = mapper;
        private readonly IStaffService _staffService = staffService;
        private readonly IManagerService _managerService = managerService;

        /// <summary>
        /// ✅ Login with email/phone and password
        /// </summary>
        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            CredentialDTO? credential = await _accountService.LoginAsync(request.EmailOrPhone, request.Password);
            if (credential == null || credential.Role == UserRole.Customer)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Email or password is incorrect"
                });
            }
            return Ok(new DefaultResponseModel<CredentialDTO>()
            {
                Message = "Login successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = credential
            });
        }
        /// <summary>
        /// ✅ Create new staff account
        /// </summary>
        [HttpPost("create-staff-account")]
        //[Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateStaffAccountAsync([FromBody] CreateAccountRequest request)
        {
            /*string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(managerId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }*/
            Staff account = _mapper.Map<Staff>(request);
            await _staffService.RegisterAccountAsync(account);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Create staff account successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅ Create new manager account
        /// </summary>
        [HttpPost("create-manager-account")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateManagerAccountAsync([FromBody] CreateManagerAccountRequest request)
        {
            string? adminId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            Manager account = _mapper.Map<Manager>(request);
            await _managerService.RegisterAccountAsync(account);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Create manager account successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
