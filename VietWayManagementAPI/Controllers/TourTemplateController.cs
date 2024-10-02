using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourTemplateController(ITourTemplateService tourTemplateService, IMapper mapper) : ControllerBase
    {
        private readonly ITourTemplateService _tourTemplateService = tourTemplateService;
        private readonly IMapper _mapper = mapper;
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<TourTemplatePreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTemplatesAsync(
            string? nameSearch,
            [FromQuery]List<string>? templateCategoryIds,
            [FromQuery]List<string>? provinceIds,
            [FromQuery]List<string>? durationIds,
            TourTemplateStatus? status,
            int? pageSize, 
            int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1)? 1 : (int)pageIndex;

            var result = await _tourTemplateService.GetAllTemplatesAsync(nameSearch,templateCategoryIds,provinceIds,durationIds,status,checkedPageSize,checkedPageIndex);
            List<TourTemplatePreview> tourTemplatePreviews = _mapper.Map<List<TourTemplatePreview>>(result.items);
            DefaultPageResponse<TourTemplatePreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = checkedPageSize,
                PageIndex = checkedPageIndex,
                Items = tourTemplatePreviews
            };
            DefaultResponseModel<DefaultPageResponse<TourTemplatePreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all tour templates successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
        [HttpGet("{tourTemplateId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultResponseModel<TourTemplateDetail>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTourTemplateById(string tourTemplateId)
        {
            TourTemplate? tourTemplate = await _tourTemplateService
                .GetTemplateByIdAsync(tourTemplateId);
            if (tourTemplate == null)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Can not find Tour template with id {tourTemplateId}"
                };
                return NotFound(response);
            } 
            else
            {
                DefaultResponseModel<TourTemplateDetail> response = new()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Get tour template successfully",
                    Data = _mapper.Map<TourTemplateDetail>(tourTemplate)
                };
                return Ok(response);
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTourTemplate([FromForm]CreateTourTemplateRequest request)
        {
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour created successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
