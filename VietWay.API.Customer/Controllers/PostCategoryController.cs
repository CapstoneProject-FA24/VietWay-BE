using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    [Route("api/post-category")]
    [ApiController]
    public class PostCategoryController(IPostCategoryService postCategoryService) : ControllerBase
    {
        private readonly IPostCategoryService _postCategoryService = postCategoryService;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<PostCategoryDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPostCategoriesAsync()
        {
            return Ok(new DefaultResponseModel<List<PostCategoryDTO>>
            {
                Message = "Success",
                Data = await _postCategoryService.GetPostCategoriesAsync(),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
