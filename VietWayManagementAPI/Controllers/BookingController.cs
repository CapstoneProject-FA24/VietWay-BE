using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;
using UserRole = VietWay.Repository.EntityModel.Base.UserRole;

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Account API endpoints
    /// </summary>
    [Route("api/booking")]
    [ApiController]
    public class BookingController(IBookingService bookingService, ITokenHelper tokenHelper, IMapper mapper) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
          
        [HttpPost("{bookingId}/create-refund-transaction")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRefundTransaction(string bookingId, [FromBody] RefundRequest request)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(managerId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            BookingPayment bookingPayment = _mapper.Map<BookingPayment>(request);
            await _bookingService.CreateRefundTransactionAsync(managerId, bookingId, bookingPayment);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Create refund transaction successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
