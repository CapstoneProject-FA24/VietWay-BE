using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.RequestModel;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
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
            tourBooking.BookingPayments = [];
            tourBooking.BookingTourParticipants = [];
            foreach (TourParticipant tourParticipant in request.TourParticipants)
            {
                BookingTourParticipant bookingTourParticipant = _mapper.Map<BookingTourParticipant>(tourParticipant);
                bookingTourParticipant.TourBookingId = tourBooking.BookingId;
                bookingTourParticipant.ParticipantId = _idGenerator.GenerateId();
                tourBooking.BookingTourParticipants.Add(bookingTourParticipant);
            }
            string? url = null;
            if (false == request.IsPayLater)
            {
                BookingPayment bookingPayment = new()
                {
                    BookingId = tourBooking.BookingId,
                    Amount = tourBooking.TotalPrice,
                    CreateOn = DateTime.UtcNow,
                    PaymentId = _idGenerator.GenerateId(),
                    Status = PaymentStatus.Pending
                };
                tourBooking.BookingPayments.Add(bookingPayment);
                url = _vnPayService.GetPaymentUrl(bookingPayment, HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0");
            }
            await _tourBookingService.CreateBookingAsync(tourBooking);
            //call service
            DefaultResponseModel<string> responseModel = new()
            {
                Message = "Booking created successfully",
                Data = url,
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(responseModel);
        }
    }
}
