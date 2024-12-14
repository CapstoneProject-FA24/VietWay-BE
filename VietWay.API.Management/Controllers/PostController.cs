using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using Microsoft.AspNetCore.Authorization;
using VietWay.API.Management.RequestModel;
using VietWay.Repository.EntityModel;
using VietWay.Util.TokenUtil;
using AutoMapper;
using VietWay.Service.Management.Interface;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.ThirdParty.Twitter;
using Tweetinvi.Core.Web;
using VietWay.Service.Management.Implement;

namespace VietWay.API.Management.Controllers
{
    [Route("api/posts")]
    [ApiController]
    public class PostController(
        IPostService postService, 
        ITokenHelper tokenHelper, 
        IMapper mapper,
        IPublishPostService publishPostService) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IMapper _mapper = mapper;
        private readonly IPublishPostService _publishPostService = publishPostService;

        /// <summary>
        /// ✅[All] Get all posts
        /// </summary>
        /// <returns> List of posts</returns>
        /// <response code="200">Return list of posts</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<PaginatedList<PostPreviewDTO>>))]
        public async Task<IActionResult> GetPosts(string? nameSearch, 
            [FromQuery] List<string>? postCategoryIds,
            [FromQuery] List<string>? provinceIds, 
            [FromQuery] List<PostStatus>? statuses,
            int? pageSize, 
            int? pageIndex)
        {
            int checkedPageSize = pageSize ?? 10;
            int checkedPageIndex = pageIndex ?? 1;

            return Ok(new DefaultResponseModel<PaginatedList<PostPreviewDTO>>()
            {
                Message = "Success",
                Data = await _postService.GetAllPostAsync(
                    nameSearch, postCategoryIds, provinceIds, statuses, checkedPageSize, checkedPageIndex),
                StatusCode = StatusCodes.Status200OK
            });
        }
        /// <summary>
        /// ✅🔐[Staff] Get post by ID
        /// </summary>
        /// <returns>Post detail</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{postId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PostDetailDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostById(string postId)
        {
            PostDetailDTO? post = await _postService.GetPostByIdAsync(postId);
            if (null == post)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<PostDetailDTO>
            {
                Message = "Get post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = post
            });
        }

        /// <summary>
        /// ✅🔐[Staff] Create new post
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Staff))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePostAsync(CreatePostRequest request)
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
            Post post = _mapper.Map<Post>(request);
            string postId = await _postService.CreatePostAsync(post);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Create post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = postId
            });
        }

        /// <summary>
        /// ✅🔐[Staff] Update post
        /// </summary>
        /// <returns>Update post message</returns>
        /// <response code="200">Return update post message</response>
        /// <response code="400">Bad request</response>
        [HttpPut("{postId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePostAsync(string postId, CreatePostRequest request)
        {
            Post post = _mapper.Map<Post>(request);
            post.PostId = postId;

            await _postService.UpdatePostAsync(post);
            return Ok();
        }

        /// <summary>
        /// [Staff] Delete draft post
        /// </summary>
        /// <returns>Delete post message</returns>
        /// <response code="200">Return delete post message</response>
        /// <response code="404">Post not found</response>
        [HttpDelete("{postId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePostAsync(string postId)
        {
            PostDetailDTO? post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Post not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(errorResponse);
            }
            await _postService.DeletePostAsync(postId);
            DefaultResponseModel<object> response = new()
            {
                Message = "Delete successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpPost("{postId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadPostTwitterAsync(string postId)
        {
            await _publishPostService.PostTweetWithXAsync(postId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Post tweet successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpPost("{postId}/facebook")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UploadPostFacebookAsync(string postId)
        {
            await _publishPostService.PublishPostToFacebookPageAsync(postId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Post facebook successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpGet("{postId}/facebook/metrics")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<FacebookMetricsDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFacebookReactionsAsync(string postId)
        {

            return Ok(new DefaultResponseModel<FacebookMetricsDTO>
            {
                Message = "Get facebook reaction count successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = await _publishPostService.GetFacebookPostMetricsAsync(postId)
            });
        }

        /// <summary>
        /// ✅🔐[Manager][Staff] Change post status
        /// </summary>
        /// <remarks>
        /// Change post status. 
        /// Staff can only change status of draft post to pending.
        /// Manager can change status of pending post to approved or rejected.
        /// </remarks>
        [HttpPatch("change-post-status/{postId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePostStatusAsync(string postId, ChangePostStatusRequest request)
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

            await _postService.ChangePostStatusAsync(postId, accountId, request.Status, request.Reason);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Status change successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }

        [HttpPatch("{postId}/images")]
        [Authorize(Roles = nameof(UserRole.Staff))]
        [Produces("application/json")]
        public async Task<IActionResult> UpdatePostImageAsync(string postId, IFormFile? newImage)
        {
            string? staffId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (staffId == null)
            {
                return Unauthorized(new DefaultResponseModel<string>()
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _postService.UpdatePostImageAsync(postId, newImage);
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{postId}/twitter/reactions-by-post-id")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTwitterPostByPostIdAsync(string postId)
        {
            TweetDTO result = await _publishPostService.GetPublishedTweetByIdAsync(postId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Get twitter post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = result
            });
        }

        [HttpDelete("{postId}/twitter")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTwitterPostsAsync(string postId)
        {
            await _publishPostService.DeleteTweetWithXAsync(postId);
            return Ok(new DefaultResponseModel<object>
            {
                Message = "Get twitter posts successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
