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
        public async Task<IActionResult> GetAllPostCategoryAsync()
        {
            var result = await _postCategoryService.GetPostCategoriesAsync();
            DefaultResponseModel<List<PostCategoryDTO>> response = new()
            {
                Data = result,
                Message = "Get all tour category successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpPost]
        //[Authorize(Roles = nameof(UserRole.Staff))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreatePostCategoryAsync(CreatePostCategoryRequest request)
        {
            /*string? staffId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(staffId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }*/

            PostCategory postCategory = _mapper.Map<PostCategory>(request);
            string postCategoryId = await _postCategoryService.CreatePostCategoryAsync(postCategory);

            return Ok(new DefaultResponseModel<string>
            {
                Message = "Create post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = postCategoryId
            });
        }
    }
}
