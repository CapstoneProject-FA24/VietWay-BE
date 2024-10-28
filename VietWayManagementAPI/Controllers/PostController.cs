using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.Service.Interface;
using VietWay.Service.DataTransferObject;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Implement;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

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
    }
}
