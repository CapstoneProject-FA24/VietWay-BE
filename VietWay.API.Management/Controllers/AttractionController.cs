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
    /// Attraction API Endpoints
    /// </summary>
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController(IAttractionService attractionService, IMapper mapper, ITokenHelper tokenHelper, IAttractionReviewService attractionReviewService) : ControllerBase
    {
        private readonly IAttractionService _attractionService = attractionService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IAttractionReviewService _attractionReviewService = attractionReviewService;

        /// <summary>
        /// ✅🔐[Manager][Staff] Get attraction list with filter and paging
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttractionsAsync(string? nameSearch, [FromQuery] List<string>? provinceIds, 
            [FromQuery] List<string>? attractionTypeIds, [FromQuery] List<AttractionStatus>? statuses, int? pageSize, int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            return Ok(new DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>
            {
                Data = await _attractionService.GetAllAttractionsWithCreatorAsync(nameSearch, provinceIds, attractionTypeIds, statuses, checkedPageSize, checkedPageIndex),
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Manager][Staff] Get attraction by ID
        /// </summary>
        /// <returns>Attraction detail</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<AttractionDetailDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAttractionById(string attractionId)
        {
            AttractionDetailDTO? attraction = await _attractionService.GetAttractionWithCreateDateByIdAsync(attractionId);
            if (null == attraction)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(new DefaultResponseModel<AttractionDetailDTO>
            {
                Message = "Get attraction successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = attraction
            });
        }

        /// <summary>
        /// ✅🔐[Staff] Create new attraction
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Staff))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAttractionAsync(CreateAttractionRequest request)
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
            Attraction attraction = _mapper.Map<Attraction>(request);
            string attractionId = await _attractionService.CreateAttractionAsync(attraction);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Create attraction successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = attractionId
            });
        }
        /// <summary>
        /// ✅🔐[Staff] Update attraction
        /// </summary>
        /// <returns>Update attraction message</returns>
        /// <response code="200">Return update attraction message</response>
        /// <response code="400">Bad request</response>
        [HttpPut("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAttractionAsync(string attractionId, CreateAttractionRequest request)
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
            Attraction attraction = _mapper.Map<Attraction>(request);
            attraction.AttractionId = attractionId;

            await _attractionService.UpdateAttractionAsync(attraction, accountId);
            return Ok();
        }

        /// <summary>
        /// ❌🔐[Staff] Delete draft attraction
        /// </summary>
        /// <returns>Delete attraction message</returns>
        /// <response code="200">Return delete attraction message</response>
        /// <response code="404">Attraction not found</response>
        [HttpDelete("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAttractionAsync(string attractionId)
        {
            await _attractionService.DeleteAttractionAsync(attractionId);
            DefaultResponseModel<object> response = new()
            {
                Message = "Delete successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        /// <summary>
        /// ✅🔐[Staff] Update attraction image
        /// </summary>
        [HttpPatch("{attractionId}/images")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(UserRole.Staff))]
        public async Task<IActionResult> UpdateAttractionImageAsync(string attractionId, [FromForm] UpdateImageRequest request)
        {
            if (0 == request.NewImages?.Count && 0 == request.DeletedImageIds?.Count)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Nothing to update",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorResponse);
            }
            await _attractionService.UpdateAttractionImageAsync(attractionId, request.NewImages, request.DeletedImageIds);
            DefaultResponseModel<object> response = new()
            {
                Message = "Update image successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        /// <summary>
        /// ✅🔐[Manager][Staff] Change attraction status
        /// </summary>
        /// <remarks>
        /// Change attraction status. 
        /// Staff can only change status of draft attraction to pending.
        /// Manager can change status of pending attraction to approved or rejected.
        /// </remarks>
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, ${nameof(UserRole.Staff)}")]
        [HttpPatch("{attractionId}/status")]
        public async Task<IActionResult> UpdateAttractionStatusAsync(string attractionId, UpdateAttractionStatusRequest request)
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
            await _attractionService.UpdateAttractionStatusAsync(attractionId, accountId, request.Status, request.Reason);
            return Ok();
        }

        [HttpGet("{attractionId}/reviews")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<PaginatedList<AttractionReviewDTO>>))]
        public async Task<IActionResult> GetAttractionReviewAsync(string attractionId, bool isOrderedByLikeNumber, [FromQuery] List<int> ratingValue,
            bool? hasReviewContent, int? pageSize, int? pageIndex, bool? isDeleted)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            var (totalCount, items) = await _attractionReviewService.GetAttractionReviewsAsync(attractionId, isOrderedByLikeNumber, ratingValue, hasReviewContent, checkedPageSize, checkedPageIndex, isDeleted);

            return Ok(new DefaultResponseModel<PaginatedList<AttractionReviewDTO>>
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
            await _attractionReviewService.ToggleAttractionReviewVisibilityAsync(accountId, reviewId, request.IsHided, request.Reason);
            return Ok();
        }

        [HttpGet("approved-attraction")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Staff)}")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllApproveAttractions(string? nameSearch, [FromQuery] List<string>? provinceIds,
            [FromQuery] List<string>? attractionTypeIds, [FromQuery] List<string>? attractionIds, int? pageSize, int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            return Ok(new DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>
            {
                Data = await _attractionService.GetAllApproveAttractionsAsync(nameSearch, provinceIds, attractionTypeIds, attractionIds, checkedPageSize, checkedPageIndex),
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPost("{attractionId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadTemplateTwitterAsync(string attractionId)
        {
            await _attractionService.PostAttractionWithXAsync(attractionId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Post attraction successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
