using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Attraction API endpoints
    /// </summary>
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController(IAttractionService attractionService, ITourTemplateService tourTemplateService) : ControllerBase
    {
        private readonly ITourTemplateService _tourTemplateService = tourTemplateService;
        private readonly IAttractionService _attractionService = attractionService;
        /// <summary>
        /// ✅[All] Get all attractions
        /// </summary>
        /// <returns> List of attractions</returns>
        /// <response code="200">Return list of attractions</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>))]
        public async Task<IActionResult> GetAttractions(string? nameSearch, [FromQuery] List<string>? provinceIds, 
            [FromQuery] List<string>? attractionTypeIds,int? pageSize,int? pageIndex)
        {
            int checkedPageSize = pageSize ?? 10;
            int checkedPageIndex = pageIndex ?? 1;

            (int totalCount, List<AttractionPreviewDTO> items) = await _attractionService.GetAttractionsPreviewAsync(
                nameSearch, provinceIds, attractionTypeIds, checkedPageSize, checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>()
            {
                Message = "Success",
                Data = new PaginatedList<AttractionPreviewDTO>
                {
                    Items = items,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Total = totalCount
                },
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[All] Get attraction by ID
        /// </summary>
        /// <param name="attractionId"></param>
        /// <returns> Attraction details</returns>
        /// <response code="200">Return attraction details</response>
        /// <response code="404">Attraction not found</response>
        [HttpGet("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultResponseModel<object>))]
        public async Task<IActionResult> GetAttractionById(string attractionId)
        {
            AttractionDetailDTO? attractionDetailDTO = await _attractionService.GetAttractionDetailByIdAsync(attractionId);
            if (attractionDetailDTO == null)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Attraction not found",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<AttractionDetailDTO>
            {
                Message = "Success",
                Data = attractionDetailDTO,
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// ✅[All] Get tour templates related to attraction
        /// </summary>
        /// <returns> List of tour templates related to this attraction </returns>
        /// <response code="200">Return list of tour templates</response>
        /// <response code="404">Attraction not found</response>
        [HttpGet("{attractionId}/tour-templates")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<TourTemplatePreviewDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRelatedTourTemplates(string attractionId, int previewCount)
        {
            return Ok(new DefaultResponseModel<List<TourTemplatePreviewDTO>>
            {
                Data = await _tourTemplateService.GetTourTemplatePreviewsByAttractionId(attractionId, previewCount),
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
