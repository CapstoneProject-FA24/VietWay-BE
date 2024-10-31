using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Event API endpoints
    /// </summary>
    [Route("api/event")]
    [ApiController]
    public class EventController(IEventService eventService) : ControllerBase
    {
        private readonly IEventService _eventService = eventService;

        /// <summary>
        /// ✅[All] Get all events
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<EventPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActiveEvent(string? nameSearch, [FromQuery]List<string>? provinceIds,
            [FromQuery]List<string>? eventCategoryIds, DateTime? startDateFrom, DateTime? startDateTo,
            int? pageSize, int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize <= 0) ? 10 : pageSize.Value;
            int checkedPageIndex = (pageIndex == null || pageIndex <= 0) ? 1 : pageIndex.Value;
            return Ok(new DefaultResponseModel<List<EventPreviewDTO>>()
            {
                Message = "Success",
                Data = await _eventService.GetEventsAsync(nameSearch, provinceIds, eventCategoryIds, 
                    startDateFrom, startDateTo, checkedPageSize, checkedPageIndex),
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{eventId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<EventDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetEventDetail(string eventId)
        {
            return Ok(new DefaultResponseModel<EventDetailDTO>()
            {
                Message = "Success",
                Data = await _eventService.GetEventDetailAsync(eventId),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
