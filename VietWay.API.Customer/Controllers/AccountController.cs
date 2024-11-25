using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.RequestModel;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Customer.Controllers
{/// <summary>
 /// Account API endpoints
 /// </summary>
    [Route("api/account")]
    [ApiController]
    public class AccountController(IAccountService accountService, ITokenHelper tokenHelper,
        ICustomerService customerService, IMapper mapper) : ControllerBase
    {
        private readonly IAccountService _accountService = accountService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly ICustomerService _customerService = customerService;
        private readonly IMapper _mapper = mapper;

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
            if (account == null || account.Role != UserRole.Customer)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Unauthorized"
                });
            }
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = _tokenHelper.GenerateAuthenticationToken(account.AccountId, account.Role.ToString())
            });
        }
        /// <summary>
        /// ✅ Register new customer account
        /// </summary>
        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromBody] CreateAccountRequest request)
        {
            Repository.EntityModel.Customer account = _mapper.Map<Repository.EntityModel.Customer>(request);
            await _customerService.RegisterAccountAsync(account);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("register-with-google")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterWithGoogleAsync([FromBody] CreateAccountWithGoogleRequest request)
        {
            Repository.EntityModel.Customer account = _mapper.Map<Repository.EntityModel.Customer>(request);
            await _customerService.RegisterAccountWithGoogleAsync(account, request.IdToken);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("login-with-google")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string idToken)
        {
            Account? account = await _accountService.LoginWithGoogleAsync(idToken);
            if (account == null || account.Role != UserRole.Customer)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Unauthorized"
                });
            }
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = _tokenHelper.GenerateAuthenticationToken(account.AccountId, account.Role.ToString())
            });
        }

        /// <summary>
        /// ✅ [All] Request OTP to reset password
        /// </summary>
        [HttpPost("request-password-reset")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RequestPasswordReset(ResetPasswordOtpRequest request)
        {
            await _accountService.SendResetPasswordOtpAsync(request.PhoneNumber);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// ✅ [All] Confirm OTP to reset password
        /// </summary>
        [HttpPost("confirm-reset-password-otp")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmResetPasswordOtp(ConfirmOtpRequest request)
        {
            string token = await _accountService.ConfirmResetPasswordOtpAsync(request.PhoneNumber, request.Otp);
            if (token == null)
            {
                return BadRequest(new DefaultResponseModel<object>()
                {
                    Message = "Invalid OTP",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = token
            });
        }

        /// <summary>
        /// ✅🔐[ResettingPasswordCustomer] Reset password
        /// </summary>
        [HttpPost("reset-password")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(CustomRole.ResettingPasswordCustomer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (accountId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Message = "Unauthorized"
                });
            }
            await _accountService.ResetPasswordAsync(accountId, request.PhoneNumber, request.NewPassword);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
