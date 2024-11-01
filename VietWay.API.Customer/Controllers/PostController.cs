using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Post API endpoints
    /// </summary>
    [Route("api/post")]
    [ApiController]
    public class PostController(IPostService postService) : ControllerBase
    {
        private readonly IPostService _postService = postService;

        /// <summary>
        /// ❌[All] Get posts
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<PostPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostPreviewAsync(string? nameSearch, [FromQuery] List<string>? provinceIds,
            [FromQuery] List<string>? postCategoryIds, int? pageSize, int? pageIndex)
        {
            int checkedPageSize = (pageSize.HasValue || pageSize > 0) ? pageSize.Value : 10;
            int checkedPageIndex = (pageIndex.HasValue || pageIndex > 0) ? pageIndex.Value : 1;
            var (count, items) = await _postService.GetPostPreviewsAsync(nameSearch, provinceIds, postCategoryIds, checkedPageSize, checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<PostPreviewDTO>>()
            {
                Message = "Success",
                Data = new PaginatedList<PostPreviewDTO>()
                {
                    Items = items,
                    PageIndex = checkedPageIndex,
                    PageSize = checkedPageSize,
                    Total = count
                },
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ❌[All] Get post detail
        /// </summary>
        [HttpGet("{tourId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PostDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostDetailAsync(string tourId)
        {
            PostDetailDTO? postDetail = await _postService.GetPostDetailAsync(tourId);
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
    }
}
