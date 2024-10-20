using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourDurationController(ITourDurationService tourDurationService, IMapper mapper) : ControllerBase
    {
        public readonly ITourDurationService _tourDurationService = tourDurationService;
        public readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DurationDetail>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourDurationAsync()
        {
            var result = await _tourDurationService.GetAllTourDuration();
            List<DurationDetail> durationDetails = _mapper.Map<List<DurationDetail>>(result);
            DefaultResponseModel<List<DurationDetail>> response = new()
            {
                Data = durationDetails,
                Message = "Get all tour category successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
