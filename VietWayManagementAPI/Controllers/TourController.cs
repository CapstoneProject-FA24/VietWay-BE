using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Implement;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
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
            var result = await _tourService.GetAllTour(pageSize, pageIndex);
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
