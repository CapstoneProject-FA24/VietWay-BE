using AutoMapper;
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
    }
}
