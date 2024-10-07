using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionScheduleController(IAttractionScheduleService attractionScheduleService, IMapper mapper) : ControllerBase
    {
        private readonly IAttractionScheduleService _attractionScheduleService = attractionScheduleService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<AttractionSchedulePreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttractionScheduleAsync(int pageSize, int pageIndex)
        {
            var result = await _attractionScheduleService.GetAllAttractionSchedules(pageSize, pageIndex);
            List<AttractionSchedulePreview> attractionSchedulePreviews = _mapper.Map<List<AttractionSchedulePreview>>(result.items);
            DefaultPageResponse<AttractionSchedulePreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = attractionSchedulePreviews
            };
            DefaultResponseModel<DefaultPageResponse<AttractionSchedulePreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all attraction schedule successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
