using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.ModelEntity;
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
        public async Task<IActionResult> GetAllTemplatesAsync(int pageSize, int pageIndex)
        {
            var result = await _tourTemplateService.GetAllTemplatesAsync(pageSize, pageIndex);
            List<TourTemplatePreviewResponse> tourTemplatePreviews = _mapper.Map<List<TourTemplatePreviewResponse>>(result.items);
            DefaultPageResponse<TourTemplatePreviewResponse> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = tourTemplatePreviews
            };
            DefaultResponseModel<DefaultPageResponse<TourTemplatePreviewResponse>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all tour templates successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
