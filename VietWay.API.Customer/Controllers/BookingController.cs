using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.RequestModel;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Implement;
using VietWay.Service.Interface;
using VietWay.Service.ThirdParty;
using VietWay.Util.CustomExceptions;
using VietWay.Util.DateTimeUtil;
using VietWay.Util.IdUtil;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Booking API endpoints
    /// </summary>
    [Route("api/bookings")]
    [ApiController]
    public class BookingController(ITourBookingService tourBookingService, ITourService tourService, IIdGenerator idGenerator,
        IMapper mapper, IVnPayService vnPayService, ITimeZoneHelper timeZoneHelper, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly ITourBookingService _tourBookingService = tourBookingService;
        private readonly ITourService _tourService = tourService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IMapper _mapper = mapper;
        private readonly ITimeZoneHelper _timeZoneHelper = timeZoneHelper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

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
        public async Task<IActionResult> BookTour(BookTourRequest request)
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
            if (request.TourParticipants?.Count != request.NumberOfParticipants)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Missing participant information"
                };
                return BadRequest(response);
            }
            Tour? tour = await _tourService.GetTourById(request.TourId);
            if (tour == null)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Can not find Tour with id {request.TourId}"
                };
                return NotFound(response);
            }
            if (tour.CurrentParticipant + request.NumberOfParticipants > tour.MaxParticipant)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Tour is full"
                };
                return BadRequest(response);
            }
            tour.CurrentParticipant = tour.CurrentParticipant + request.NumberOfParticipants;
            if (tour.CurrentParticipant == tour.MaxParticipant)
            {
                tour.Status = TourStatus.Closed;
            }
            Booking tourBooking = _mapper.Map<Booking>(request);
            tourBooking.CustomerId = customerId;
            tourBooking.Tour = tour;
            tourBooking.BookingId = _idGenerator.GenerateId();
            tourBooking.Status = BookingStatus.Pending;
            tourBooking.TotalPrice = (decimal)tour.Price * request.NumberOfParticipants;
            tourBooking.CreatedAt = _timeZoneHelper.GetUTC7Now();
            tourBooking.BookingPayments = [];
            tourBooking.BookingTourParticipants = [];
            foreach (TourParticipant tourParticipant in request.TourParticipants)
            {
                BookingTourParticipant bookingTourParticipant = _mapper.Map<BookingTourParticipant>(tourParticipant);
                bookingTourParticipant.TourBookingId = tourBooking.BookingId;
                bookingTourParticipant.ParticipantId = _idGenerator.GenerateId();
                tourBooking.BookingTourParticipants.Add(bookingTourParticipant);
            }
            await _tourBookingService.CreateBookingAsync(tourBooking);
            //call service
            DefaultResponseModel<string> responseModel = new()
            {
                Message = "Booking created successfully",
                Data = tourBooking.BookingId,
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
        [ProducesResponseType<DefaultResponseModel<TourBookingInfoDTO>>(StatusCodes.Status200OK)]
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
            return Ok(new DefaultResponseModel<TourBookingInfoDTO>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = await _tourBookingService.GetTourBookingInfoAsync(bookingId, customerId)
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
        [ProducesResponseType<DefaultResponseModel<PaginatedList<TourBookingPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerBookings(int? pageCount, int? pageIndex)
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
            int checkedPageSize = (pageCount == null || pageCount < 1) ? 10 : (int)pageCount;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            var (totalCount, items) = await _tourBookingService.GetCustomerBookedToursAsync(customerId, checkedPageSize, checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<TourBookingPreviewDTO>>()
            {
                Data = new()
                {
                    Total = totalCount,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Items = items
                },
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
                await _tourBookingService.CustomerCancelBookingAsync(bookingId, customerId, cancelBookingRequest.Reason);
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
    }
}