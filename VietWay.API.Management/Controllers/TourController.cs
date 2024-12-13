using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Tour API endpoints
    /// </summary>
    [Route("api/tours")]
    [ApiController]
    public class TourController(ITourService tourService,
        ITokenHelper tokenHelper,
        IMapper mapper) : ControllerBase
    {
        private readonly ITourService _tourService = tourService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// [Manager][Staff] Get all tours
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<TourPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourAsync(
            string? nameSearch, string? codeSearch, [FromQuery] List<string>? provinceIds, [FromQuery] List<string>? tourCategoryIds,
            [FromQuery] List<string>? durationIds, TourStatus? status, int? pageSize, int? pageIndex,
            DateTime? startDateFrom, DateTime? startDateTo
            )
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            var (totalCount, items) = await _tourService.GetAllTour(
                nameSearch, codeSearch, provinceIds, tourCategoryIds, durationIds, status, 
                checkedPageSize, checkedPageIndex, startDateFrom, startDateTo);
            PaginatedList<TourPreviewDTO> pagedResponse = new()
            {
                Total = totalCount,
                PageSize = checkedPageSize,
                PageIndex = checkedPageIndex,
                Items = items
            };
            DefaultResponseModel<PaginatedList<TourPreviewDTO>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all tour successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        /// <summary>
        /// [Manager][Staff] Get tour by id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("{tourId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultResponseModel<TourDetailDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTourById(string tourId)
        {
            TourDetailDTO? tour = await _tourService
                .GetTourById(tourId);
            if (tour == null)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Can not find Tour with id {tourId}"
                };
                return NotFound(response);
            }
            else
            {
                DefaultResponseModel<TourDetailDTO> response = new()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Get tour template successfully",
                    Data = tour
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// [Staff] {WIP} Create a new tour
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateTourAsync(CreateTourRequest request)
        {
            string? staffId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(staffId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            Tour tour = _mapper.Map<Tour>(request);
            string tourId = await _tourService.CreateTour(tour);

            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Create tour successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = tourId
            });
        }

        /// <summary>
        /// ✅🔐[Manager] Change tour status
        /// </summary>
        /// <returns>Tour status changed</returns>
        /// <response code="200">Return tour status changed</response>
        /// <response code="400">Bad request</response>
        [HttpPatch("change-tour-status/{tourId}")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeTourStatusAsync(string tourId, ChangeTourStatusRequest request)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(managerId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            await _tourService.ChangeTourStatusAsync(tourId, managerId, request.Status, request.Reason);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Status change successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }

        /// <summary>
        /// [Manager][Staff] Get tour by tour template id
        /// </summary>
        /// <response code="200">Success</response>
        [HttpGet("get-by-template-id/{tourTemplateId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<TourDetailDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllToursByTemplateIdsAsync(string tourTemplateId)
        {
            List<TourDetailDTO> tours = await _tourService.GetAllToursByTemplateIdsAsync(tourTemplateId);

            DefaultResponseModel<List<TourDetailDTO>> response = new()
            {
                Data = tours,
                Message = "Get all tour successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpPut("edit-tour/{tourId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> EditTourAsync(string tourId, EditTourRequest request)
        {
            Tour tour = _mapper.Map<Tour>(request);

            await _tourService.EditTour(tourId, tour);
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Edit tour successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = ""
            });
        }

        /// <summary>
        /// [Manager] Cancel tour
        /// </summary>
        [HttpPatch("cancel-tour/{tourId}")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelTourAsync(string tourId, CancelRequest request)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(managerId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            await _tourService.CancelTourAsync(tourId, managerId, request.Reason);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Status change successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }

        [HttpDelete("{tourId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTourAsync(string tourId)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _tourService.DeleteTourAsync(tourId);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour template deleted successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
