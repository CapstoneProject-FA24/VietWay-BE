using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.Service.Customer.Interface;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Tour template API endpoints
    /// </summary>
    [Route("api/tour-templates")]
    [ApiController]
    public class TourTemplateController(ITourTemplateService tourTemplateService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ITourTemplateService _tourTemplateService = tourTemplateService;

        /// <summary>
        /// ⚠️[All] Get tour template by ID
        /// </summary>
        [HttpGet("{tourTemplateId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultResponseModel<TourTemplateDetailDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTourTemplateById(string tourTemplateId)
        {
            TourTemplateDetailDTO? tourTemplate = await _tourTemplateService
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
                DefaultResponseModel<TourTemplateDetailDTO> response = new()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Get tour template successfully",
                    Data = tourTemplate
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// ⚠️[All] Get all tour templates
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<TourTemplateWithTourInfoDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTourTemplateWithTourInfoAsync(
            string? nameSearch,
            [FromQuery] List<string>? templateCategoryIds,
            [FromQuery] List<string>? provinceIds,
            [FromQuery] List<int>? numberOfDay,
            DateTime? startDateFrom,
            DateTime? startDateTo,
            decimal? minPrice,
            decimal? maxPrice,
            int? pageSize,
            int? pageIndex)
        {
            int checkedPageSize = (null == pageSize || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (null == pageIndex || pageIndex <1)? 1 : (int)pageIndex;

            var (count,items) = await _tourTemplateService.GetTourTemplatesWithActiveToursAsync(
                nameSearch,templateCategoryIds,provinceIds,numberOfDay,startDateFrom,
                startDateTo,minPrice,maxPrice,checkedPageSize,checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<TourTemplateWithTourInfoDTO>>()
            {
                Message = "Get tour template successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = new()
                {
                    Items = items,
                    PageIndex = checkedPageIndex,
                    PageSize = checkedPageSize,
                    Total = count
                }
            });
        }
    }
}
