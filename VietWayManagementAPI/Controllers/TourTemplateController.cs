using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
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
        public async Task<IActionResult> GetAllTemplatesAsync(int pageSize, int pageIndex)
        {
            var result = await _tourTemplateService.GetAllTemplatesAsync(pageSize, pageIndex);
            List<TourTemplatePreview> tourTemplatePreviews = _mapper.Map<List<TourTemplatePreview>>(result.items);
            DefaultPageResponse<TourTemplatePreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
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
        public async Task<IActionResult> GetTourTemplateById(long tourTemplateId)
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
