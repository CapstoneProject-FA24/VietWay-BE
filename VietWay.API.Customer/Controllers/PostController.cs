using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.RequestModel;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Post API endpoints
    /// </summary>
    [Route("api/post")]
    [ApiController]
    public class PostController(IPostService postService, ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// ✅[🔐][All]/[Customer] Get all posts, and get if customer liked each post
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<PostPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostPreviewAsync(string? nameSearch, [FromQuery] List<string>? provinceIds,
            [FromQuery] List<string>? postCategoryIds, int? pageSize, int? pageIndex)
        {
            int checkedPageSize = (pageSize.HasValue || pageSize > 0) ? pageSize.Value : 10;
            int checkedPageIndex = (pageIndex.HasValue || pageIndex > 0) ? pageIndex.Value : 1;
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            return Ok(new DefaultResponseModel<PaginatedList<PostPreviewDTO>>()
            {
                Message = "Success",
                Data = await _postService.GetPostPreviewsAsync(nameSearch, provinceIds, postCategoryIds, customerId,
                    checkedPageSize, checkedPageIndex),
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[🔐][All]/[Customer] Get post detail, and get if customer liked this post
        /// </summary>
        [HttpGet("{postId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PostDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostDetailAsync(string postId)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            PostDetailDTO? postDetail = await _postService.GetPostDetailAsync(postId,customerId);
            if (postDetail == null)
            {
                return NotFound(new DefaultResponseModel<object>()
                {
                    Message = "Not found",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<PostDetailDTO>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK,
                Data = postDetail
            });
        }
        [HttpPatch("{postId}/like")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Customer))]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> LikePostAsync(string postId,ToggleLikeRequest request)
        {
            string? customerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            await _postService.TogglePostLikeAsync(postId, customerId, request.IsLike);
            return Ok(new DefaultResponseModel<object>()
            {
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
