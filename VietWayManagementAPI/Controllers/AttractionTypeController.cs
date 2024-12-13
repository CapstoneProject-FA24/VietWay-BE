using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

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
        [ProducesResponseType<DefaultResponseModel<AttractionCategoryDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttractionTypeAsync(string? nameSearch)
        {
            var result = await _attractionTypeService.GetAllAttractionType(nameSearch);
            DefaultResponseModel<List<AttractionCategoryDTO>> response = new()
            {
                Data = result,
                Message = "Get all attraction type successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        /// <summary>
        /// [Manager][Staff] Get attraction type by id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("{attractionTypeId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<AttractionCategoryDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAttractionTypeByIdAsync(string attractionTypeId)
        {
            AttractionCategoryDTO? attractionCategory = await _attractionTypeService.GetAttractionCategoryByIdAsync(attractionTypeId);
            if (null == attractionCategory)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<AttractionCategoryDTO>
            {
                Message = "Get post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = attractionCategory
            });
        }

        /// <summary>
        /// [Manager] Create new attraction type
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAttractionTypeAsync(CreateAttractionTypeRequest request)
        {
            AttractionCategory attractionCategory = _mapper.Map<AttractionCategory>(request);
            string attractionCategoryId = await _attractionTypeService.CreateAttractionCategoryAsync(attractionCategory);

            return Ok(new DefaultResponseModel<string>
            {
                Message = "Create post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = attractionCategoryId
            });
        }

        /// <summary>
        /// [Manager] Update attraction type
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpPut("{attractionTypeId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAttractionTypeAsync(string attractionTypeId, CreateAttractionTypeRequest request)
        {
            AttractionCategory attractionCategory = _mapper.Map<AttractionCategory>(request);

            await _attractionTypeService.UpdateAttractionCategoryAsync(attractionTypeId, attractionCategory);
            return Ok();
        }

        /// <summary>
        /// [Manager] Delete attraction type
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
            AttractionCategoryDTO? attractionCategory = await _attractionTypeService.GetAttractionCategoryByIdAsync(attractionTypeId);
            if (attractionCategory == null)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Attraction Category not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(errorResponse);
            }
            await _attractionTypeService.DeleteAttractionCategoryAsync(attractionTypeId);
            DefaultResponseModel<object> response = new()
            {
                Message = "Delete successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
