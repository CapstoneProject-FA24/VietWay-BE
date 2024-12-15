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
using VietWay.Util.CustomExceptions;
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
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
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
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
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

        [HttpPatch("{bookingId}")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelBooking(string bookingId, CancelRequest cancelBookingRequest)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (accountId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _bookingService.CancelBookingAsync(bookingId, accountId, cancelBookingRequest.Reason);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Booking cancelled successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPut("{bookingId}/change-booking-tour")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeBookingTour(string bookingId, ChangeTourRequest request)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (accountId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _bookingService.ChangeBookingTourAsync(accountId, bookingId, request.NewTourId, request.Reason);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Booking tour change successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{bookingId}/history")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [ProducesResponseType<DefaultResponseModel<List<BookingHistoryDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookingHistory(string bookingId)
        {
            return Ok(new DefaultResponseModel<List<BookingHistoryDTO>>()
            {
                Data = await _bookingService.GetBookingHistoryAsync(bookingId),
                Message = "Get booking history successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
