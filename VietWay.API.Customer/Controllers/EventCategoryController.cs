using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Event category API endpoints
    /// </summary>
    [Route("api/event-category")]
    [ApiController]
    public class EventCategoryController(IEventCategoryService eventCategoryService) : ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService = eventCategoryService;

        /// <summary>
        /// ✅[All] Get all event categories
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<EventCategoryPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEventCategory()
        {
            return Ok(new DefaultResponseModel<List<EventCategoryPreviewDTO>>()
            {
                Message = "Success",
                Data = await _eventCategoryService.GetEventCategoriesAsync(),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
