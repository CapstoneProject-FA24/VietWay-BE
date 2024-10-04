using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionController(IAttractionService attractionService, IMapper mapper) : ControllerBase
    {
        private readonly IAttractionService _attractionService = attractionService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<AttractionPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttractionsAsync(
            string? nameSearch,
            [FromQuery] List<string>? provinceIds,
            [FromQuery] List<string>? attractionTypeIds,
            AttractionStatus? status,
            int? pageSize,
            int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;

            var (totalCount, items) = await _attractionService.GetAllAttractions(nameSearch, provinceIds, attractionTypeIds, status, checkedPageSize, checkedPageIndex);
            List<AttractionPreview> attractionPreviews = _mapper.Map<List<AttractionPreview>>(items);
            DefaultPageResponse<AttractionPreview> pagedResponse = new()
            {
                Total = totalCount,
                PageSize = checkedPageSize,
                PageIndex = checkedPageIndex,
                Items = attractionPreviews
            };
            DefaultResponseModel<DefaultPageResponse<AttractionPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all attractions successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
        [HttpGet("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<AttractionDetail>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAttractionById(string attractionId)
        {
            Attraction? attraction = await _attractionService.GetAttractionById(attractionId);
            if (null == attraction)
            {
                DefaultResponseModel<object> response = new()
                {
                    Message = "Attraction not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(response);
            }
            else 
            {
                DefaultResponseModel<AttractionDetail> response = new()
                {
                    Message = "Get attraction successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<AttractionDetail>(attraction)
                };
                return Ok(response);
            }
        }
    }
}
