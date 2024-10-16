using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Attraction Type API endpoints
    /// </summary>
    [Route("api/attraction-types")]
    [ApiController]
    public class AttractionTypeController : ControllerBase
    {
        /// <summary>
        /// [All] {WIP} Get all attraction types
        /// </summary>
        /// <returns> List of attraction types</returns>
        /// <response code="200">Return list of attraction types</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        public Task<IActionResult> GetAllAttractionType()
        {
            throw new NotImplementedException();
        }
    }
}
