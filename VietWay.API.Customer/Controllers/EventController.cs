using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.DataTransferObject;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Event API endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<EventPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActiveEvent(string? nameSearch, [FromQuery]List<string>? provinceId,
            [FromQuery]List<string>? eventCategoryIds, DateTime? startDateFrom, DateTime? startDateTo,
            int? pageSize, int? pageIndex)
        {
            return Ok(new DefaultResponseModel<List<EventPreviewDTO>>()
            {
                Message = "Success",

                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
