using Microsoft.AspNetCore.Mvc;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        [HttpPatch("{customerId}")]
        public async Task<IActionResult> ChangeCustomerStatus(string customerId, bool isDeleted)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (managerId == null)
            {
                return Unauthorized();
            }
            await _customerService.ChangeCustomerStatus(customerId, managerId, isDeleted);
            return Ok();
        }
    }
}
