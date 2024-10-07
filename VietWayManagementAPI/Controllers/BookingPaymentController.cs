using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingPaymentController(IBookingPaymentService bookingPaymentService, IMapper mapper) : ControllerBase
    {
        private readonly IBookingPaymentService _bookingPaymentService = bookingPaymentService;
        private readonly IMapper _mapper = mapper;
        [HttpGet("VnPayIPN")]
        public async Task<IActionResult> HandleVnPayIPN(VnPayIPNRequest request)
        {
            VnPayIPN vnPayIPN = _mapper.Map<VnPayIPN>(request);
            await _bookingPaymentService.HandleVnPayIPN(vnPayIPN);
            return Ok(new
            {
                RspCode = "00",
                Message = "Success"
            });
        }
    }
}
