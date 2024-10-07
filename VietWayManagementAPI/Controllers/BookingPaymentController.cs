using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using VietWay.API.Management.RequestModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingPaymentController(IBookingPaymentService bookingPaymentService, IMapper mapper, ILogger<BookingPaymentController> logger) : ControllerBase
    {
        private readonly IBookingPaymentService _bookingPaymentService = bookingPaymentService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BookingPaymentController> _logger = logger;
        [HttpGet("VnPayIPN")]
        public async Task<IActionResult> HandleVnPayIPN(VnPayIPNRequest request)
        {
            VnPayIPN vnPayIPN = _mapper.Map<VnPayIPN>(request);
            await _bookingPaymentService.HandleVnPayIPN(vnPayIPN);
            _logger.LogInformation("HandleVnPayIPN: {0}", JsonSerializer.Serialize(vnPayIPN));
            return Ok(new
            {
                RspCode = "00",
                Message = "Success"
            });
        }
    }
}
