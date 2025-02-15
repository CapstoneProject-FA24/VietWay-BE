﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Implement;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/tour-templates")]
    [ApiController]
    public class TourTemplateController(ITourTemplateService tourTemplateService, IMapper mapper, ITokenHelper tokenHelper, ITourReviewService tourReviewService) : ControllerBase
    {
        private readonly ITourTemplateService _tourTemplateService = tourTemplateService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly ITourReviewService _tourReviewService = tourReviewService;

        [HttpGet]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<TourTemplatePreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTemplatesAsync(
            string? nameSearch,
            [FromQuery] List<string>? templateCategoryIds,
            [FromQuery] List<string>? provinceIds,
            [FromQuery] List<string>? durationIds,
            [FromQuery] List<TourTemplateStatus>? statuses,
            int? pageSize,
            int? pageIndex)
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

            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            DefaultResponseModel<PaginatedList<TourTemplatePreviewDTO>> response = new()
            {
                Data = await _tourTemplateService.GetAllTemplatesAsync(nameSearch, templateCategoryIds, provinceIds, durationIds, statuses, checkedPageSize, checkedPageIndex),
                Message = "Get all tour templates successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpGet("{tourTemplateId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultResponseModel<TourTemplateDetailDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTourTemplateById(string tourTemplateId)
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
            TourTemplateDetailDTO? tourTemplateDetailDTO = await _tourTemplateService
                .GetTemplateByIdAsync(tourTemplateId);
            if (tourTemplateDetailDTO == null)
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
                    Data = tourTemplateDetailDTO
                };
                return Ok(response);
            }
        }
        [HttpPost]
        [Authorize(Roles = $"{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTourTemplate([FromBody] CreateTourTemplateRequest request)
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

            TourTemplate tourTemplate = _mapper.Map<TourTemplate>(request);
            await _tourTemplateService.CreateTemplateAsync(tourTemplate);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour template created successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = tourTemplate.TourTemplateId
            });
        }
        [HttpPut("{tourTemplateId}")]
        [Authorize(Roles = $"{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTourTemplate(string tourTemplateId, CreateTourTemplateRequest request)
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
            TourTemplate tourTemplate = _mapper.Map<TourTemplate>(request);
            await _tourTemplateService.UpdateTemplateAsync(tourTemplateId, tourTemplate);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour template updated successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpDelete("{tourTemplateId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTourTemplate(string tourTemplateId)
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
            await _tourTemplateService.DeleteTemplateAsync(accountId, tourTemplateId);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Tour template deleted successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPatch("{tourTemplateId}/images")]
        [Authorize(Roles = $"{nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateTourTemplateImageAsync(string tourTemplateId, UpdateImageRequest request)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (accountId == null)
            {
                return Unauthorized(new DefaultResponseModel<string>()
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            if (0 == request.NewImages?.Count && 0 == request.DeletedImageIds?.Count)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Nothing to update",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorResponse);
            }
            await _tourTemplateService.UpdateTourTemplateImageAsync(tourTemplateId, accountId, request.NewImages, request.DeletedImageIds);
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Manager] Change tour template status
        /// </summary>
        /// <returns>Tour template status changed</returns>
        /// <response code="200">Return tour template status changed</response>
        /// <response code="400">Bad request</response>
        [HttpPatch("change-tour-template-status/{tourTemplateId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeTourTemplateStatusAsync(string tourTemplateId, ChangeTourTemplateStatusRequest request)
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

            await _tourTemplateService.ChangeTourTemplateStatusAsync(tourTemplateId, accountId, request.Status, request.Reason);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Status change successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }

        [HttpGet("{tourTemplateId}/reviews")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<PaginatedList<TourReviewDTO>>))]
        public async Task<IActionResult> GetTourTemplateReviewAsync(string tourTemplateId, [FromQuery] List<int> ratingValue,
            bool? hasReviewContent, int? pageSize, int? pageIndex, bool? isDeleted)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            var (totalCount, items) = await _tourReviewService.GetTourReviewsAsync(tourTemplateId, ratingValue, hasReviewContent, checkedPageSize, checkedPageIndex, isDeleted);

            return Ok(new DefaultResponseModel<PaginatedList<TourReviewDTO>>
            {
                Data = new()
                {
                    Total = totalCount,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Items = items
                },
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [Authorize(Roles = $"{nameof(UserRole.Manager)}")]
        [HttpPatch("reviews/{reviewId}/hide")]
        public async Task<IActionResult> ToggleReviewVisibilityAsync(string reviewId, [FromBody] HideReviewRequest request)
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
            await _tourReviewService.ToggleTourReviewVisibilityAsync(accountId, reviewId, request.IsHided, request.Reason);
            return Ok();
        }

        [HttpGet("with-tour-info")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
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
            int? pageIndex,
            string? tourId)
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

            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            DefaultResponseModel<PaginatedList<TourTemplateWithTourInfoDTO>> response = new()
            {
                Data = await _tourTemplateService.GetAllTemplateWithActiveToursAsync(
                nameSearch, templateCategoryIds, provinceIds, numberOfDay, startDateFrom,
                startDateTo, minPrice, maxPrice, checkedPageSize, checkedPageIndex, tourId),
                Message = "Get all tour templates successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
