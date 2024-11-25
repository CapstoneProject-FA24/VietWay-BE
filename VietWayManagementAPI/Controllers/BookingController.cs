using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
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

        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<BookingPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookings(
            BookingStatus? bookingStatus,
            int? pageCount, int? pageIndex,
            string? bookingIdSearch, string? contactNameSearch, string? contactPhoneSearch, string? tourIdSearch)
        {
            int checkedPageSize = (pageCount.HasValue && pageCount.Value > 0) ? pageCount.Value : 10;
            int checkedPageIndex = (pageIndex.HasValue && pageIndex.Value > 0) ? pageIndex.Value : 1;

            var (totalCount, items) = await _bookingService.GetBookingsAsync(bookingStatus, checkedPageSize, checkedPageIndex, bookingIdSearch, contactNameSearch, contactPhoneSearch, tourIdSearch);

            return Ok(new DefaultResponseModel<PaginatedList<BookingPreviewDTO>>()
            {
                Data = new()
                {
                    Total = totalCount,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Items = items
                },
                Message = "Get bookings successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }

        [HttpGet("{bookingId}")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [ProducesResponseType<DefaultResponseModel<BookingDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookingById(string bookingId)
        {
            BookingDetailDTO result = await _bookingService.GetBookingByIdAsync(bookingId);
            if (null == result)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<BookingDetailDTO>()
            {
                Data = result,
                Message = "Get bookings successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }

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
