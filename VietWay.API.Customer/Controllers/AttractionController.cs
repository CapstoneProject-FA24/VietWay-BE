using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Attraction API endpoints
    /// </summary>
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController : ControllerBase
    {
        /// <summary>
        /// [All] {WIP} Get all attractions
        /// </summary>
        /// <returns> List of attractions</returns>
        /// <response code="200">Return list of attractions</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(DefaultResponseModel<object>))]
        public Task<IActionResult> GetAttractions(
            string? nameSearch, 
            [FromQuery] List<string>? provinceIds, 
            [FromQuery] List<string>? attractionTypeIds,
            int? pageSize,
            int? pageIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [All] {WIP} Get attraction by ID
        /// </summary>
        /// <param name="attractionId"></param>
        /// <returns> Attraction details</returns>
        /// <response code="200">Return attraction details</response>
        /// <response code="404">Attraction not found</response>
        [HttpGet("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultResponseModel<object>))]
        public Task<IActionResult> GetAttractionById(string attractionId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// [All] {WIP} Get tour templates related to attraction
        /// </summary>
        /// <returns> List of tour templates related to this attraction </returns>
        /// <response code="200">Return list of tour templates</response>
        /// <response code="404">Attraction not found</response>
        [HttpGet("{attractionId}/tour-templates")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<object>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRelatedTourTemplates(string attractionId)
        {
            throw new NotImplementedException();
        }
    }
}
