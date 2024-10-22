using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VietWay.Service.Interface;
using VietWay.API.Customer.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet.Actions;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.TokenUtil;
using VietWay.Service.DataTransferObject;
using VietWay.API.Customer.RequestModel;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Customer API endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService, ITokenHelper tokenHelper) : ControllerBase
    {
        public readonly ICustomerService _customerService = customerService;
        public readonly ITokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// ✅🔐[Customer] Get current customer profile
        /// </summary>
        [HttpGet("profile")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<CustomerInfoDTO>>(StatusCodes.Status200OK)]
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
            CustomerInfoDTO? customerInfoDTO = await _customerService.GetCustomerProfileInfo(customerId);
            if (customerInfoDTO == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<CustomerInfoDTO>()
            {
                Data = customerInfoDTO,
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
            await _customerService.UpdateCustomerProfileAsync(customerId, request.FullName, request.DateOfBirth,
                request.ProvinceId, request.Gender, request.Email);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Update customer profile successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
