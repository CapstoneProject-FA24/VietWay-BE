using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Attraction Type API endpoints
    /// </summary>
    [Route("api/attraction-types")]
    [ApiController]
    public class AttractionTypeController(IAttractionTypeService attractionTypeService) : ControllerBase
    {
        private readonly IAttractionTypeService _attractionTypeService = attractionTypeService;
        /// <summary>
        /// ✅[All] Get all attraction types
        /// </summary>
        /// <returns> List of attraction types</returns>
        /// <response code="200">Return list of attraction types</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        public async Task<IActionResult> GetAllAttractionType()
        {
            return Ok(new DefaultResponseModel<List<AttractionCategoryPreviewDTO>>
            {
                Message = "Success",
                Data = await _attractionTypeService.GetAllAttractionType(),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
