using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController(ITourService tourService, IMapper mapper) : ControllerBase
    {
        private readonly ITourService _tourService = tourService;
        private readonly IMapper _mapper = mapper;

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

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<TourPreview>>>(StatusCodes.Status200OK)]
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
