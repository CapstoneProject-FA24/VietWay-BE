using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Management.Interface;
using VietWay.Service.Management.DataTransferObject;

namespace VietWay.API.Management.Controllers
{
    [Route("api/popular")]
    [ApiController]
    public class PopularController : ControllerBase
    {
        private readonly IPopularService _popularService;

        public PopularController(IPopularService popularService)
        {
            _popularService = popularService;
        }

        /// <summary>
        /// ✅[All] Get popular provinces
        /// </summary>
        /// <returns>List of popular provinces</returns>
        [HttpGet("provinces")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<PopularProvinceDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPopularProvinces()
        {
            var provinces = await _popularService.GetPopularProvincesAsync();
            
            return Ok(new DefaultResponseModel<List<PopularProvinceDTO>>
            {
                Message = "Get popular provinces successfully",
                Data = provinces,
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[All] Get popular attraction categories
        /// </summary>
        /// <returns>List of popular attraction categories</returns>
        [HttpGet("attraction-categories")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<PopularAttractionCategoryDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPopularAttractionCategories()
        {
            var categories = await _popularService.GetPopularAttractionCategoriesAsync();
            
            return Ok(new DefaultResponseModel<List<PopularAttractionCategoryDTO>>
            {
                Message = "Get popular attraction categories successfully",
                Data = categories,
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[All] Get popular post categories
        /// </summary>
        /// <returns>List of popular post categories</returns>
        [HttpGet("post-categories")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<PopularPostCategoryDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPopularPostCategories()
        {
            var categories = await _popularService.GetPopularPostCategoriesAsync();
            
            return Ok(new DefaultResponseModel<List<PopularPostCategoryDTO>>
            {
                Message = "Get popular post categories successfully",
                Data = categories,
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[All] Get popular tour categories
        /// </summary>
        /// <returns>List of popular tour categories</returns>
        [HttpGet("tour-categories")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<PopularTourCategoryDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPopularTourCategories()
        {
            var categories = await _popularService.GetPopularTourCategoriesAsync();
            
            return Ok(new DefaultResponseModel<List<PopularTourCategoryDTO>>
            {
                Message = "Get popular tour categories successfully",
                Data = categories,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
