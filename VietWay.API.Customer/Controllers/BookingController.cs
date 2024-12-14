using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.RequestModel;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.CustomExceptions;
using VietWay.Util.TokenUtil;
using VietWay.Service.Customer.Interface;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Booking API endpoints
    /// </summary>
    [Route("api/bookings")]
    [ApiController]
    public class BookingController(IBookingService bookingService, ITourService tourService,
        IMapper mapper, ITokenHelper tokenHelper, IBookingPaymentService bookingPaymentService,
        ITourReviewService tourReviewService) : ControllerBase
    {
        private readonly IBookingService _bookingService = bookingService;
        private readonly ITourService _tourService = tourService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IBookingPaymentService _bookingPaymentService = bookingPaymentService;
        private readonly ITourReviewService _tourReviewService = tourReviewService;

        /// <summary>
        /// ⚠️🔐[Customer] Book a tour
        /// </summary>
        /// <returns>Status message</returns>
        /// <response code="200">Booking created successfully</response>
        /// <response code="400"> Missng info or tour is full </response>
        /// <response code="404">Can not find Tour with id</response>
        [HttpPost]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> BookTourAsync(BookTourRequest request)
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
            if (request.NumberOfParticipants < 1 || request.TourParticipants?.Count != request.NumberOfParticipants)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Missing participant information"
                };
                return BadRequest(response);
            }

            Booking booking = _mapper.Map<Booking>(request);
            booking.CustomerId = customerId;

            string bookingId = await _bookingService.BookTourAsync(booking);

            DefaultResponseModel<string> responseModel = new()
            {
                Message = "Booking created successfully",
                Data = bookingId,
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// ✅🔐[Customer] Get booking info by booking ID
        /// </summary>
        /// <returns> Booking detail</returns>
        /// <response code="200">Get booking successfully</response>
        /// <response code="404">Can not find booking with id</response>
        [HttpGet("{bookingId}")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<BookingDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTourBookingByIdAsync(string bookingId)
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
            BookingDetailDTO? bookingDetailDTO = await _bookingService.GetBookingDetailAsync(customerId, bookingId);
            if (bookingDetailDTO == null)
            {
                return NotFound(new DefaultResponseModel<object>()
                {
                    Message = "Not Found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<BookingDetailDTO>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = bookingDetailDTO
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Get customer bookings
        /// </summary>
        /// <returns> List of customer bookings </returns>
        /// <response code="200">Get customer bookings successfully</response>
        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<BookingPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerBookings(BookingStatus? bookingStatus, int? pageCount, int? pageIndex)
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
            int checkedPageSize = (pageCount.HasValue && pageCount.Value > 0) ? pageCount.Value : 10;
            int checkedPageIndex = (pageIndex.HasValue && pageIndex.Value > 0) ? pageIndex.Value : 1;

            return Ok(new DefaultResponseModel<PaginatedList<BookingPreviewDTO>>()
            {
                Data = await _bookingService.GetCustomerBookingsAsync(customerId, bookingStatus, checkedPageSize, checkedPageIndex),
                Message = "Get customer bookings successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Cancel booking
        /// </summary>
        /// <response code="200">Booking cancelled successfully</response>
        /// <response code="404">Can not find booking with id</response>
        /// <response code="400">Booking is not cancellable</response>
        [HttpPatch("{bookingId}")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelBooking(string bookingId, CancelBookingRequest cancelBookingRequest)
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
            try
            {
                await _bookingService.CancelBookingAsync(customerId, bookingId, cancelBookingRequest.Reason);
                return Ok(new DefaultResponseModel<object>()
                {
                    Message = "Booking cancelled successfully",
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new DefaultResponseModel<object>()
                {
                    Message = "Booking is not cancellable",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            catch (ResourceNotFoundException)
            {
                return NotFound(new DefaultResponseModel<object>()
                {
                    Message = "Can not find booking with id",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
        }

        [HttpGet("{bookingId}/payments")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<BookingPaymentDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookingPayments(string bookingId, int? pageCount, int? pageIndex)
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
            int checkedPageSize = (pageCount.HasValue && pageCount.Value > 0) ? pageCount.Value : 10;
            int checkedPageIndex = (pageIndex.HasValue && pageIndex.Value > 0) ? pageIndex.Value : 1;
            return Ok(new DefaultResponseModel<PaginatedList<BookingPaymentDTO>>()
            {
                Data = await _bookingPaymentService.GetBookingPaymentsAsync(customerId, bookingId, checkedPageSize, checkedPageIndex),
                Message = "Get booking payments successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Get tour info by booking id
        /// </summary>
        [HttpGet("{bookingId}/tour-info")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<TourDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTourInfo(string bookingId)
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
            TourDetailDTO? tourDetailDTO = await _tourService.GetTourDetailByBookingIdAsync(customerId, bookingId);
            if (tourDetailDTO == null)
            {
                return NotFound(new DefaultResponseModel<object>()
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<TourDetailDTO>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = tourDetailDTO
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Generate a payment URL for booking payment
        /// </summary>
        [HttpGet("{bookingId}/payment-url")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentUrl(string bookingId, PaymentMethod paymentMethod, bool? isFullPayment)
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
                .GetBookingPaymentUrl(paymentMethod, isFullPayment, bookingId, customerId, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "");
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = url,
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// ✅🔐[Customer] Review a tour by booking ID
        /// </summary>
        [HttpPost("{bookingId}/review")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReviewBookingAsync(string bookingId, ReviewTourRequest reviewTourRequest)
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
            TourReview tourReview = _mapper.Map<TourReview>(reviewTourRequest);
            tourReview.BookingId = bookingId;
            await _tourReviewService.CreateTourReviewAsync(customerId, tourReview);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Review successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// ✅🔐[Customer] Get review by booking ID
        /// </summary>
        [HttpGet("{bookingId}/review")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<TourReviewDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewByBookingId(string bookingId)
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
            TourReviewDTO? tourReviewDTO = await _tourReviewService.GetTourReviewByBookingIdAsync(customerId, bookingId);
            if (tourReviewDTO == null)
            {
                return NotFound(new DefaultResponseModel<object>()
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<TourReviewDTO>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = tourReviewDTO
            });
        }
        [HttpGet("{bookingId}/history")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<List<BookingHistoryDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBookingHistory(string bookingId)
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
            return Ok(new DefaultResponseModel<List<BookingHistoryDTO>>()
            {
                Data = await _bookingService.GetBookingHistoryAsync(customerId, bookingId),
                Message = "Get booking history successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpPatch("{bookingId}/confirm-tour-change")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmTourChangeAsync(string bookingId)
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
            await _bookingService.ConfirmTourChangeAsync(customerId, bookingId);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpPatch("{bookingId}/deny-tour-change")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DenyTourChangeAsync(string bookingId)
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
            await _bookingService.DenyTourChangeAsync(customerId, bookingId);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}