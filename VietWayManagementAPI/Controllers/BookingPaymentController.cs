using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using VietWay.API.Management.RequestModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Service.ThirdParty.VnPay;
using VietWay.Service.ThirdParty.ZaloPay;

namespace VietWay.API.Management.Controllers
{
    [Route("api/booking-payments")]
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

        [HttpGet("ZaloPayCallback/local")]
        public async Task<IActionResult> ZaloPayCallbackLocal(ZaloPayCallbackRequest request)
        {
            ZaloPayCallback zaloPayCallback = _mapper.Map<ZaloPayCallback>(request);
            await _bookingPaymentService.HandleZaloPayCallbackLocal(zaloPayCallback);
            return Ok(new
            {
                RspCode = "00",
                Message = "Success"
            });
        }

        [HttpPost("ZaloPayCallback")]
        public async Task<IActionResult> HandleZaloPayCallback([FromBody] ZaloPayCallbackData callbackData)
        {
            CallbackData data = _mapper.Map<CallbackData>(callbackData);
            return Ok(await _bookingPaymentService.HandleZaloPayCallback(data));
        }
    }
}
