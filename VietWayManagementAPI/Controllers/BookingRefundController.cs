using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Util.TokenUtil;
using VietWay.Service.Management.Interface;
using AutoMapper;
using VietWay.Service.Management.DataTransferObject;


namespace VietWay.API.Management.Controllers
{
    [Route("api/booking-refunds")]
    [ApiController]
    public class BookingRefundController(ITokenHelper tokenHelper, IBookingRefundService bookingRefundService,
        IMapper mapper) : ControllerBase
    {
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IBookingRefundService _bookingRefundService = bookingRefundService;
        private readonly IMapper _mapper = mapper;
        [HttpPost("{bookingId}/refund/")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRefundTransaction(string bookingId, [FromBody] RefundRequest request)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            BookingRefund bookingPayment = _mapper.Map<BookingRefund>(request);
            await _bookingRefundService.UpdateBookingRefundInfo(accountId, bookingId, bookingPayment);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Create refund transaction successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<Service.Management.DataTransferObject.PaginatedList<BookingRefundDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRefundRequestAsync(
            string? bookingId, RefundStatus? refundStatus, int? pageSize, int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            Service.Management.DataTransferObject.PaginatedList<BookingRefundDTO> pagedResponse = await _bookingRefundService.GetBookingRefundAsync(bookingId, refundStatus, checkedPageSize, checkedPageIndex);
            DefaultResponseModel<Service.Management.DataTransferObject.PaginatedList<BookingRefundDTO>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all refund request successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
