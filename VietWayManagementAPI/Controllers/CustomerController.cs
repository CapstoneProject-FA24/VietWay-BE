using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Implement;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomerController(ICustomerService customerService, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<PaginatedList<CustomerPreviewDTO>>))]
        public async Task<IActionResult> GetAllCustomerInfos(string? nameSearch,
            int? pageSize,
            int? pageIndex)
        {
            int checkedPageSize = pageSize ?? 10;
            int checkedPageIndex = pageIndex ?? 1;

            (int totalCount, List<CustomerPreviewDTO> items) = await _customerService.GetAllCustomers(
                nameSearch, checkedPageSize, checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<CustomerPreviewDTO>>()
            {
                Message = "Success",
                Data = new PaginatedList<CustomerPreviewDTO>
                {
                    Items = items,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Total = totalCount
                },
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPatch("{customerId}")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeCustomerStatus(string customerId, bool isDeleted)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (accountId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _customerService.ChangeCustomerStatus(customerId, isDeleted);
            DefaultResponseModel<object> response = new()
            {
                Message = "Change status successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
