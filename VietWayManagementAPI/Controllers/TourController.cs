using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Tour API endpoints
    /// </summary>
    [Route("api/tours")]
    [ApiController]
    public class TourController(ITourService tourService, IMapper mapper) : ControllerBase
    {
        private readonly ITourService _tourService = tourService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// [Manager][Staff] Get all tours
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<TourPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourAsync(int pageSize, int pageIndex)
        {
            var result = await _tourService.GetAllTour(pageSize, pageIndex);
            List<TourPreview> tourPreviews = _mapper.Map<List<TourPreview>>(result.items);
            PaginatedList<TourPreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = tourPreviews
            };
            DefaultResponseModel<PaginatedList<TourPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all tour successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        /// <summary>
        /// [Manager][Staff] Get tour by id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
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
                    Message = "Get tour template successfully",
                    Data = _mapper.Map<TourDetail>(tour)
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// [Staff] {WIP} Create a new tour
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTourAsync([FromBody] CreateTourRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
