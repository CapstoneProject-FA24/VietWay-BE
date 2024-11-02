using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Tour duration API endpoints
    /// </summary>
    [Route("api/tour-duration")]
    [ApiController]
    public class TourDurationController(ITourDurationService tourDurationService) : ControllerBase
    {
        private readonly ITourDurationService _tourDurationService = tourDurationService;
        /// <summary>
        /// ✅[All] Get all tour duration
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<TourDuration>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourDuration()
        {
            return Ok(new DefaultResponseModel<List<TourDurationDTO>>()
            {
                Message = "Get all tour duration successfully",
                Data = await _tourDurationService.GetTourDurationsAsync(),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
