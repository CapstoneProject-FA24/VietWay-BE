using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VietWay.Service.Interface;
using VietWay.API.Customer.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet.Actions;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.TokenUtil;

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
        /// ✅[Customer] Get current customer profile
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentCustomerProfile()
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized();
            }
            return Ok();
        }
    }
}
