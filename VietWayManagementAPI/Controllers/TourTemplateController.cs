using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.Interface;

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
        [ProducesResponseType<DefaultResponseModel<PaginatedList<TourTemplatePreview>>>(StatusCodes.Status200OK)]
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
            PaginatedList<TourTemplatePreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = checkedPageSize,
                PageIndex = checkedPageIndex,
                Items = tourTemplatePreviews
            };
            DefaultResponseModel<PaginatedList<TourTemplatePreview>> response = new()
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
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
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
        public async Task<IActionResult> CreateTourTemplate([FromBody]CreateTourTemplateRequest request)
        {
            TourTemplate tourTemplate = _mapper.Map<TourTemplate>(request);
#warning replace createdby with current user id in token
            await _tourTemplateService.CreateTemplateAsync(tourTemplate);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour created successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpPut("{tourTemplateId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTourTemplate(string tourTemplateId,CreateTourTemplateRequest request)
        {
            TourTemplate? tourTemplate = await _tourTemplateService.GetTemplateByIdAsync(tourTemplateId);
            if (tourTemplate == null)
            {
                return NotFound(new DefaultResponseModel<object>()
                {
                    Message = $"Can not find Tour template with id {tourTemplateId}",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            bool isInfoMissing = string.IsNullOrWhiteSpace(request.Code) ||
                                string.IsNullOrWhiteSpace(request.TourName) ||
                                string.IsNullOrWhiteSpace(request.Description) ||
                                string.IsNullOrWhiteSpace(request.Policy) ||
                                string.IsNullOrWhiteSpace(request.Note) ||
                                request.ProvinceIds?.Count == 0 ||
                                request.Schedules?.Count == 0 ||
                                request.Schedules.Any(s => string.IsNullOrWhiteSpace(s.Title) || string.IsNullOrWhiteSpace(s.Description));
            if (false == request.IsDraft && isInfoMissing)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Incomplete attraction information",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorResponse);
            }
            tourTemplate.Code = request.Code?? "";
            tourTemplate.TourName = request.TourName ?? "";
            tourTemplate.Description = request.Description ?? "";
            tourTemplate.DurationId = request.DurationId;
            tourTemplate.TourCategoryId = request.TourCategoryId;
            tourTemplate.Note = request.Note ?? "";
            tourTemplate.TourTemplateProvinces?.Clear();
            foreach (string provinceId in request.ProvinceIds)
            {
                tourTemplate.TourTemplateProvinces?
                    .Add(new TourTemplateProvince()
                    {
                        ProvinceId = provinceId,
                        TourTemplateId = tourTemplateId
                    });
            }
            List<TourTemplateSchedule> newSchedule = [];
            foreach(var schedule in request.Schedules)
            {
                newSchedule.Add(new()
                {
                    TourTemplateId = tourTemplateId,
                    DayNumber = schedule.DayNumber,
                    Description = schedule.Description ?? "",
                    Title = schedule.Title ?? "",
                    AttractionSchedules = schedule.AttractionIds.Select(x => new AttractionSchedule()
                    {
                        AttractionId = x,
                        DayNumber = schedule.DayNumber,
                        TourTemplateId = tourTemplateId
                    }).ToList()
                });
            }

            await _tourTemplateService.UpdateTemplateAsync(tourTemplate, newSchedule);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour template updated successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpDelete("{tourTemplateId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTourTemplate(string tourTemplateId)
        {
            TourTemplate? tourTemplate = await _tourTemplateService.GetTemplateByIdAsync(tourTemplateId);
            if (tourTemplate == null)
            {
                return NotFound(new DefaultResponseModel<object>()
                {
                    Message = $"Can not find Tour template with id {tourTemplateId}",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            await _tourTemplateService.DeleteTemplateAsync(tourTemplate);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour template deleted successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
