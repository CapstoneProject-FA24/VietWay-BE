using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/post-categories")]
    [ApiController]
    public class PostCategoryController(IPostCategoryService postCategoryService) : ControllerBase
    {
        private readonly IPostCategoryService _postCategoryService = postCategoryService;

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
    }
}
