using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/post-categories")]
    [ApiController]
    public class PostCategoryController(IPostCategoryService postCategoryService,
        IMapper mapper,
        ITokenHelper tokenHelper) : ControllerBase
    {
        private readonly IPostCategoryService _postCategoryService = postCategoryService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PostCategoryDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPostCategoryAsync(string? nameSearch)
        {
            var result = await _postCategoryService.GetPostCategoriesAsync(nameSearch);
            DefaultResponseModel<List<PostCategoryDTO>> response = new()
            {
                Data = result,
                Message = "Get all tour category successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpGet("{postCategoryId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PostDetailDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostCategoryById(string postCategoryId)
        {
            PostCategoryDTO? postCategory = await _postCategoryService.GetPostCategoryByIdAsync(postCategoryId);
            if (null == postCategory)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<PostCategoryDTO>
            {
                Message = "Get post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = postCategory
            });
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePostCategoryAsync(CreatePostCategoryRequest request)
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

            PostCategory postCategory = _mapper.Map<PostCategory>(request);
            string postCategoryId = await _postCategoryService.CreatePostCategoryAsync(postCategory);

            return Ok(new DefaultResponseModel<string>
            {
                Message = "Create post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = postCategoryId
            });
        }

        [HttpPut("{postCategoryId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePostAsync(string postCategoryId, CreatePostCategoryRequest request)
        {
            PostCategory postCategory = _mapper.Map<PostCategory>(request);

            await _postCategoryService.UpdatePostCategoryAsync(postCategoryId, postCategory);
            return Ok();
        }

        [HttpDelete("{postCategoryId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePostCategoryAsync(string postCategoryId)
        {
            PostCategoryDTO? postCategory = await _postCategoryService.GetPostCategoryByIdAsync(postCategoryId);
            if (postCategory == null)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Post Category not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(errorResponse);
            }
            await _postCategoryService.DeletePostCategoryAsync(postCategoryId);
            DefaultResponseModel<object> response = new()
            {
                Message = "Delete successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
