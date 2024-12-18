using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.TokenUtil;
using VietWay.Service.Customer.Interface;
using VietWay.API.Customer.RequestModel;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Customer API endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// ✅🔐[Customer] Get current customer profile
        /// </summary>
        [HttpGet("profile")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<CustomerDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentCustomerProfileAsync()
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            CustomerDetailDTO? customerDetailDTO = await _customerService.GetCustomerDetailAsync(customerId);
            if (customerDetailDTO == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<CustomerDetailDTO>()
            {
                Data = customerDetailDTO,
                Message = "Get current customer profile successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Update customer profile
        /// </summary>
        [HttpPut("profile")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateCustomerProfile(UpdateCustomerProfileRequest request)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _customerService.UpdateCustomerInfoAsync(customerId, request.FullName,request.DateOfBirth,request.ProvinceId,request.Gender,request.Email);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Update customer profile successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPatch("customer-change-password")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CustomerChangePasswordAsync([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            await _customerService.CustomerChangePassword(accountId, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return Ok();
        }
    }
}
