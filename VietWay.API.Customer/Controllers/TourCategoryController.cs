using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Tour category API endpoints
    /// </summary>
    [Route("api/tour-categories")]
    [ApiController]
    public class TourCategoryController : ControllerBase
    {
        /// <summary>
        /// [All] {WIP} Get tour categories
        /// </summary>
        /// <returns> List of tour category</returns>
        /// <response code="200">Get tour category list successfully</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<object>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTourCategories()
        {
            throw new NotImplementedException();
        }
    }
}
