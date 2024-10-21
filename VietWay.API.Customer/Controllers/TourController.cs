using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Tour API Endpoints
    /// </summary>
    [Route("api/tours")]
    [ApiController]
    public class TourController(ITourService tourService, IMapper mapper) : ControllerBase
    {
        public readonly ITourService _tourService = tourService;
        public readonly IMapper _mapper = mapper;

        /// <summary>
        /// [All] Get tour by tour ID
        /// </summary>
        /// <returns> Tour details </returns>
        /// <response code="200">Get tour successfully</response>
        /// <response code="404">Tour ID not found</response>
        [HttpGet("{tourId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultResponseModel<TourDetail>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTourById(string tourId)
        {
            Tour? tour = await _tourService
                .GetTourById(tourId);
            if (tour == null)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Can not find Tour with id {tourId}"
                };
                return NotFound(response);
            }
            else
            {
                DefaultResponseModel<TourDetail> response = new()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Get tour successfully",
                    Data = _mapper.Map<TourDetail>(tour)
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// [All] Get tour by tour template ID
        /// </summary>
        /// <returns> List of tours </returns>
        /// <response code="200">Get tour list successfully</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<TourPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllToursByTemplateIdsAsync(
            string tourTemplateId)
        {

            var result = await _tourService.GetAllToursByTemplateIdsAsync(tourTemplateId);
            List<TourPreview> tourPreviews = _mapper.Map<List<TourPreview>>(result);
            
            DefaultResponseModel<List<TourPreview>> response = new()
            {
                Data = tourPreviews,
                Message = "Get all tour successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
