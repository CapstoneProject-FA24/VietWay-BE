using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.Service.Interface;
using VietWay.Service.DataTransferObject;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Implement;
using VietWay.Repository.EntityModel.Base;
using Microsoft.AspNetCore.Authorization;
using VietWay.API.Management.RequestModel;
using VietWay.Repository.EntityModel;
using VietWay.Util.TokenUtil;
using AutoMapper;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostService postService, ITokenHelper tokenHelper, IMapper mapper) : ControllerBase
    {
        private readonly IPostService _postService = postService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IMapper _mapper = mapper;

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
            PostStatus? status,
            int? pageSize, 
            int? pageIndex)
        {
            int checkedPageSize = pageSize ?? 10;
            int checkedPageIndex = pageIndex ?? 1;

            (int totalCount, List<PostPreviewDTO> items) = await _postService.GetAllPostAsync(
                nameSearch, postCategoryIds, provinceIds, status, checkedPageSize, checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<PostPreviewDTO>>()
            {
                Message = "Success",
                Data = new PaginatedList<PostPreviewDTO>
                {
                    Items = items,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Total = totalCount
                },
                StatusCode = StatusCodes.Status200OK
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
            string? staffId = _tokenHelper.GetAccountIdFromToken(HttpContext) ?? "1";
            if (string.IsNullOrWhiteSpace(staffId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            Post post = _mapper.Map<Post>(request);

            post.CreatedAt = DateTime.UtcNow;

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
            PostPreviewDTO? post = await _postService.GetPostByIdAsync(postId);
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

        /// <summary>
        /// ✅🔐[Staff] Get post by ID
        /// </summary>
        /// <returns>Post detail</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpGet("{postId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PostPreviewDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostById(string postId)
        {
            PostPreviewDTO? post = await _postService.GetPostByIdAsync(postId);
            if (null == post)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }

            return Ok(new DefaultResponseModel<PostPreviewDTO>
            {
                Message = "Get post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = post
            });
        }
    }
}
