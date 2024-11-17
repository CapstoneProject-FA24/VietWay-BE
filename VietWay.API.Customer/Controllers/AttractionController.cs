using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.RequestModel;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Attraction API endpoints
    /// </summary>
    [Route("api/attractions")]
    [ApiController]
    public class AttractionController(IAttractionService attractionService, ITourTemplateService tourTemplateService, 
        IAttractionReviewService attractionReviewService, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly ITourTemplateService _tourTemplateService = tourTemplateService;
        private readonly IAttractionService _attractionService = attractionService;
        private readonly IAttractionReviewService _attractionReviewService = attractionReviewService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        /// <summary>
        /// ✅[🔐][All]/[Customer] Get all attractions, and get if customer liked each attraction
        /// </summary>
        /// <returns> List of attractions</returns>
        /// <response code="200">Return list of attractions</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>))]
        public async Task<IActionResult> GetAttractionsAsync(string? nameSearch, [FromQuery] List<string>? provinceIds, 
            [FromQuery] List<string>? attractionTypeIds,int? pageSize,int? pageIndex)
        {
            int checkedPageSize = pageSize ?? 10;
            int checkedPageIndex = pageIndex ?? 1;
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            return Ok(new DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>()
            {
                Message = "Success",
                Data = await _attractionService.GetAttractionsPreviewAsync(
                    nameSearch, provinceIds, attractionTypeIds, customerId, checkedPageSize, checkedPageIndex),
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[🔐][All]/[Customer] Get attraction by ID, and get if customer liked this attraction
        /// </summary>
        [HttpGet("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultResponseModel<object>))]
        public async Task<IActionResult> GetAttractionById(string attractionId)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            AttractionDetailDTO? attractionDetailDTO = await _attractionService.GetAttractionDetailByIdAsync(attractionId,customerId);
            if (attractionDetailDTO == null)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Attraction not found",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<AttractionDetailDTO>
            {
                Message = "Success",
                Data = attractionDetailDTO,
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// ✅[All] Get tour templates related to attraction
        /// </summary>
        /// <returns> List of tour templates related to this attraction </returns>
        /// <response code="200">Return list of tour templates</response>
        /// <response code="404">Attraction not found</response>
        [HttpGet("{attractionId}/tour-templates")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<TourTemplatePreviewDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRelatedTourTemplates(string attractionId, int previewCount)
        {
            return Ok(new DefaultResponseModel<List<TourTemplatePreviewDTO>>
            {
                Data = await _tourTemplateService.GetTourTemplatePreviewsByAttractionId(attractionId, previewCount),
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[🔐] [All]/[Customer] Get attraction reviews
        /// </summary>
        /// <remarks>
        /// Get reviews for current attraction. If customer is logged in, that customer's review is not included in the result.
        /// </remarks>
        [HttpGet("{attractionId}/reviews")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<PaginatedList<AttractionReviewDTO>>))]
        public async Task<IActionResult> GetAttractionReviewsAsync(string attractionId, bool isOrderedByLikeNumber, [FromQuery] List<int> ratingValue, 
            bool? hasReviewContent, int? pageSize, int? pageIndex)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            int checkedPageSize = (pageSize.HasValue && pageSize.Value > 0) ? pageSize.Value : 10;
            int checkedPageIndex = (pageIndex.HasValue && pageIndex.Value > 0) ? pageIndex.Value : 1;
            return Ok(new DefaultResponseModel<PaginatedList<AttractionReviewDTO>> {
                Message = "Success",
                Data = await _attractionReviewService.GetOtherAttractionReviewsAsync(
                    attractionId, customerId, isOrderedByLikeNumber, ratingValue, hasReviewContent, checkedPageSize, checkedPageIndex),
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐 [Customer] Get logged in customer's review
        /// </summary>
        /// <remarks>
        /// Get customer's review for current attraction.
        /// </remarks>
        [HttpGet("{attractionId}/customer-reviews")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<AttractionReviewDTO>))]
        public async Task<IActionResult> GetCustomerAttractionReviewAsync(string attractionId)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            AttractionReviewDTO? review = await _attractionReviewService.GetUserAttractionReviewAsync(attractionId, customerId);
            return Ok(new DefaultResponseModel<AttractionReviewDTO>
            {
                Message = "Success",
                Data = review,
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Add attraction review
        /// </summary>
        /// <param name="attractionId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{attractionId}/customer-reviews")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        public async Task<IActionResult> AddAttractionReviewAsync(string attractionId, [FromBody] ReviewAttractionRequest request)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            AttractionReview review = new()
            {
                AttractionId = attractionId,
                CustomerId = customerId,
                Rating = request.Rating,
                CreatedAt = DateTime.MinValue,
                ReviewId = "",
                Review = request.Review
            };
            await _attractionReviewService.AddAttractionReviewAsync(review);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }


        /// <summary>
        /// ✅🔐[Customer] Update attraction review
        /// </summary>
        [HttpPut("{attractionId}/customer-reviews")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        public async Task<IActionResult> UpdateAttractionReviewAsync(string attractionId, [FromBody] ReviewAttractionRequest request)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            AttractionReview review = new()
            {
                AttractionId = attractionId,
                CustomerId = customerId,
                Rating = request.Rating,
                CreatedAt = DateTime.MinValue,
                ReviewId = "",
                Review = request.Review
            };
            await _attractionReviewService.UpdateAttractionReviewAsync(review);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Like/Dislike an attraction review
        /// </summary>
        [HttpPatch("reviews/{reviewId}/like")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        public async Task<IActionResult> ToggleAttractionReviewLikeAsync(string reviewId, ToggleLikeRequest toggleLikeRequest)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _attractionReviewService.ToggleAttractionReviewLikeAsync(reviewId, customerId, toggleLikeRequest.IsLike);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅🔐[Customer] Like/Dislike an attraction
        /// </summary>
        [HttpPatch("{attractionId}/likes")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<object>))]
        public async Task<IActionResult> UpdateAttractionAsync(string attractionId, [FromBody] ToggleLikeRequest request)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _attractionService.ToggleAttractionLikeAsync(attractionId, customerId, request.IsLike);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// ✅🔐[Customer] Get liked attractions
        /// </summary>
        [HttpGet("liked")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>))]
        public async Task<IActionResult> GetCustomerLikedAttractionAsync(int? pageSize, int? pageIndex)
        {
            int checkedPageSize = (pageSize.HasValue && pageSize.Value > 0) ? pageSize.Value : 10;
            int checkedPageIndex = (pageIndex.HasValue && pageIndex.Value > 0) ? pageIndex.Value : 1;
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (customerId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            return Ok(new DefaultResponseModel<PaginatedList<AttractionPreviewDTO>>
            {
                Message = "Success",
                Data = await _attractionService.GetCustomerLikedAttractionsAsync(customerId, checkedPageSize, checkedPageIndex),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
