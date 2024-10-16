using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Province API endpoints
    /// </summary>
    [Route("api/provinces")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        /// <summary>
        /// [All] {WIP} Get province list
        /// </summary>
        /// <returns>List of provinces</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProvinces()
        {
            throw new NotImplementedException();
        }
    }
}
