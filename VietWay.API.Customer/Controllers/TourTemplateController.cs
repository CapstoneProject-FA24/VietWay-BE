using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.Service.Interface;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Implement;
using VietWay.Service.DataTransferObject;

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
        /// [All] Get tour template by ID
        /// </summary>
        /// <param name="tourTemplateId"></param>
        /// <returns> Tour template details </returns>
        /// <response code="200"> Tour template details</response>
        /// <response code="404"> Tour template not found</response>
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

        /// <summary>
        /// [All] Get all tour templates
        /// </summary>
        /// <param name="nameSearch"></param>
        /// <param name="templateCategoryIds"></param>
        /// <param name="provinceIds"></param>
        /// <param name="numberOfDay"></param>
        /// <param name="startDateFrom"></param>
        /// <param name="startDateTo"></param>
        /// <param name="minPrice"></param>
        /// <param name="maxPrice"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>List of tour templates</returns>
        /// <response code="200">Return list of tour templates</response>
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

            var (count,items) = await _tourTemplateService.GetAllTemplateWithActiveToursAsync(
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
