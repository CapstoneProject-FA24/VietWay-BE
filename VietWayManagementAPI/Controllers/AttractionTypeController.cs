using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Attraction type API endpoints
    /// </summary>
    [Route("api/attraction-types")]
    [ApiController]
    public class AttractionTypeController(IAttractionTypeService attractionTypeService, IMapper mapper) : ControllerBase
    {
        private readonly IAttractionTypeService _attractionTypeService = attractionTypeService;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// [Manager][Staff] Get all attraction types
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<AttractionTypePreview>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttractionTypeAsync()
        {
            var result = await _attractionTypeService.GetAllAttractionType();
            List<AttractionTypePreview> attractionTypePreviews = _mapper.Map<List<AttractionTypePreview>>(result);
            DefaultResponseModel<List<AttractionTypePreview>> response = new()
            {
                Data = attractionTypePreviews,
                Message = "Get all attraction type successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        /// <summary>
        /// [Manager][Staff] {WIP} Get attraction type by id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("{attractionTypeId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<AttractionTypePreview>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttractionTypeByIdAsync(string attractionTypeId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Manager] {WIP} Create new attraction type
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAttractionTypeAsync([FromBody] CreateAttractionTypeRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Manager] {WIP} Update attraction type
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpPut("{attractionTypeId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAttractionTypeAsync(string attractionTypeId, [FromBody] CreateAttractionTypeRequest request)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// [Manager] {WIP} Delete attraction type
        /// </summary>
        /// <remarks>
        /// Soft delete if the attraction type already been assigned to at least 1 attraction.
        /// Otherwise, hard delete that attraction type.
        /// </remarks>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpDelete("{attractionTypeId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAttractionTypeAsync(string attractionTypeId)
        {
            throw new NotImplementedException();
        }
    }
}
