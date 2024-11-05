using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/event-categories")]
    [ApiController]
    public class EventCategoryController(IEventCategoryService eventCategoryService) : ControllerBase
    {
        private readonly IEventCategoryService _eventCategoryService = eventCategoryService;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<EventCategoryDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllEventCategoryAsync()
        {
            var result = await _eventCategoryService.GetAllEventCategoryAsync();
            DefaultResponseModel<List<EventCategoryDTO>> response = new()
            {
                Data = result,
                Message = "Get all tour category successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
