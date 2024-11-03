using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel.Base;
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
        /// <summary>
        /// ✅🔐[Customer] Generate a VNPay URL for booking payment
        /// </summary>
        /// <return>VNPay URL for booking payment</return>
        /// <response code="200">Get VNPay URL successfully</response>
        /// <response code="404">Booking ID not found</response>
        [HttpGet("{bookingId}")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentUrl(string bookingId, PaymentMethod paymentMethod)
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
            string url = await _bookingPaymentService
                .GetBookingPaymentUrl(paymentMethod, bookingId, customerId, HttpContext.Connection.RemoteIpAddress?.ToString()??"");
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = url,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
