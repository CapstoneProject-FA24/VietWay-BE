using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IBookingPaymentService bookingPaymentService) : ControllerBase
    {
        private readonly IBookingPaymentService _bookingPaymentService = bookingPaymentService;
        [HttpGet("VnPay/{bookingId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVnPayUrl(string bookingId)
        {
            IPAddress? ipAddress = HttpContext.Connection.RemoteIpAddress;
            if (ipAddress?.IsIPv4MappedToIPv6 ?? false)
            {
                ipAddress = ipAddress.MapToIPv4();
            }
            string url = await _bookingPaymentService
                .GetVnPayBookingPaymentUrl(bookingId, ipAddress?.ToString()??"");
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = url,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
