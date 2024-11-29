using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Payment API endpoints
    /// </summary>
    [Route("api/payments")]
    [ApiController]
    public class PaymentController(IBookingPaymentService bookingPaymentService, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IBookingPaymentService _bookingPaymentService = bookingPaymentService;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<BookingPaymentDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = nameof(UserRole.Customer))]
        public async Task<IActionResult> GetAllCustomerPaymentsAsync()
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized();
            }
            int pageSize = 10;
            int pageIndex = 1;
            return Ok(new DefaultResponseModel<PaginatedList<BookingPaymentDTO>>
            {
                Message = "Success",
                Data = await _bookingPaymentService.GetAllCustomerBookingPaymentsAsync(customerId, pageSize, pageIndex),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
