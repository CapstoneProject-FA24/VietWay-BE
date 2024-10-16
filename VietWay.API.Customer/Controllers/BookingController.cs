using AutoMapper;
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
using VietWay.Util.IdHelper;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Booking API endpoints
    /// </summary>
    [Route("api/bookings")]
    [ApiController]
    public class BookingController(ITourBookingService tourBookingService, ITourService tourService, IIdGenerator idGenerator, IMapper mapper, IVnPayService vnPayService) : ControllerBase
    {
        private readonly ITourBookingService _tourBookingService = tourBookingService;
        private readonly ITourService _tourService = tourService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// [Customer] Book a tour
        /// </summary>
        /// <returns>Status message</returns>
        /// <response code="200">Booking created successfully</response>
        /// <response code="400"> Missng info or tour is full </response>
        /// <response code="404">Can not find Tour with id</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> BookTour(BookTourRequest request)
        {
            if (request.TourParticipants?.Count != request.NumberOfParticipants){
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
            TourBooking tourBooking = _mapper.Map<TourBooking>(request);
            tourBooking.Tour = tour;
            tourBooking.BookingId = _idGenerator.GenerateId();
            tourBooking.Status = BookingStatus.Pending;
            tourBooking.TotalPrice = tour.Price * request.NumberOfParticipants;
            tourBooking.CreatedOn = DateTime.UtcNow;
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
        /// [Customer] Get booking info by booking ID
        /// </summary>
        /// <returns> Booking detail</returns>
        /// <response code="200">Get booking successfully</response>
        /// <response code="404">Can not find booking with id</response>
        [HttpGet("{bookingId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<TourBookingInfoDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTourBookingByIdAsync(string bookingId)
        {
            return Ok(new DefaultResponseModel<TourBookingInfoDTO>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = await _tourBookingService.GetTourBookingInfoAsync(bookingId)
            });
        }

        /// <summary>
        /// [Customer] {WIP} Get customer bookings
        /// </summary>
        /// <returns> List of customer bookings </returns>
        /// <response code="200">Get customer bookings successfully</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<object>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCustomerBookings(int? pageCount, int? pageIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Customer] {WIP} Cancel booking
        /// </summary>
        /// <returns> Booking cancellation message </returns>
        /// <response code="200">Booking cancelled successfully</response>
        /// <response code="404">Can not find booking with id</response>
        /// <response code="400">Booking is not cancellable</response>
        [HttpPatch("{bookingId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelBooking(string bookingId, CancelBookingRequest cancelBookingRequest)
        {
            throw new NotImplementedException();
        }
    }
}
