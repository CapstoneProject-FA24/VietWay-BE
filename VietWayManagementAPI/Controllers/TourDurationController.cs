using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/tour-durations")]
    [ApiController]
    public class TourDurationController(ITourDurationService tourDurationService, IMapper mapper) : ControllerBase
    {
        private readonly ITourDurationService _tourDurationService = tourDurationService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<TourDurationDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourDurationAsync()
        {
            var result = await _tourDurationService.GetAllTourDuration();
            DefaultResponseModel<List<TourDurationDTO>> response = new()
            {
                Data = result,
                Message = "Get all tour category successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
