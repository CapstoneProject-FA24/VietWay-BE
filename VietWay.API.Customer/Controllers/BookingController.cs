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
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController(ITourBookingService tourBookingService, ITourService tourService, IIdGenerator idGenerator, IMapper mapper, IVnPayService vnPayService) : ControllerBase
    {
        private readonly ITourBookingService _tourBookingService = tourBookingService;
        private readonly ITourService _tourService = tourService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        private readonly IMapper _mapper = mapper;
        private readonly IVnPayService _vnPayService = vnPayService;
        [HttpPost("BookTour")]
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
            TourBooking tourBooking = _mapper.Map<TourBooking>(request);
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
    }
}
