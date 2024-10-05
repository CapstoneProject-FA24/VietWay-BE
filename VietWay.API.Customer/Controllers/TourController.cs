using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Implement;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController(ITourService tourService, IMapper mapper) : ControllerBase
    {
        private readonly ITourService _tourService = tourService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<TourPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourAsync(int pageSize, int pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;

            var result = await _tourService.GetAllScheduledTour(checkedPageSize, checkedPageIndex);
            List<TourPreview> tourPreviews = _mapper.Map<List<TourPreview>>(result.items);
            DefaultPageResponse<TourPreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = tourPreviews
            };
            DefaultResponseModel<DefaultPageResponse<TourPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all tour successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpGet("by-id/{tourId}")]
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

        [HttpGet("by-template-ids")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<TourPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllToursByTemplateIdsAsync(
            [FromQuery] string tourTemplateIds,
            int pageSize,
            int pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;

            var result = await _tourService.GetAllToursByTemplateIdsAsync(tourTemplateIds, checkedPageSize, checkedPageIndex);
            List<TourPreview> tourPreviews = _mapper.Map<List<TourPreview>>(result.items);
            DefaultPageResponse<TourPreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = tourPreviews
            };
            DefaultResponseModel<DefaultPageResponse<TourPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all tour successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
