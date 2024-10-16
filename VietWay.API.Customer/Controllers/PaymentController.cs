using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Payment API endpoints
    /// </summary>
    [Route("api/payments")]
    [ApiController]
    public class PaymentController(IBookingPaymentService bookingPaymentService) : ControllerBase
    {
        /// <summary>
        /// [Customer] Generate a VNPay URL for booking payment
        /// </summary>
        /// <return>VNPay URL for booking payment</return>
        /// <response code="200">Get VNPay URL successfully</response>
        /// <response code="404">Booking ID not found</response>
#warning TODO: check if booking ID exists
        private readonly IBookingPaymentService _bookingPaymentService = bookingPaymentService;
        [HttpGet("{bookingId}/vnpay")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVnPayUrl(string bookingId)
        {
            string url = await _bookingPaymentService
                .GetVnPayBookingPaymentUrl(bookingId, HttpContext.Connection.RemoteIpAddress?.ToString()??"");
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = url,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
