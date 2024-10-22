using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Tour category API endpoints
    /// </summary>
    [Route("api/tour-categories")]
    [ApiController]
    public class TourCategoryController(ITourCategoryService tourCategoryService) : ControllerBase
    {
        private readonly ITourCategoryService _tourCategoryService = tourCategoryService;
        /// <summary>
        /// ✅[All] Get tour categories
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<object>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTourCategories()
        {
            return Ok(new DefaultResponseModel<List<TourCategoryDTO>>()
            {
                Data = await _tourCategoryService.GetAllTourCategory(),
                Message = "Success",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
