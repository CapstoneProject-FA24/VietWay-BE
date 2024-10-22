using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

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
            return Ok(new DefaultResponseModel<List<TourDurationPreviewDTO>>()
            {
                Message = "Get all tour duration successfully",
                Data = await _tourDurationService.GetTourDurationPreviews(),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
