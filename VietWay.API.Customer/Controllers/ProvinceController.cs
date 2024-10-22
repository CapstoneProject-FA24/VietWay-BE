using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Implement;
using VietWay.Service.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Province API endpoints
    /// </summary>
    [Route("api/provinces")]
    [ApiController]
    public class ProvinceController(IProvinceService provinceService) : ControllerBase
    {
        private readonly IProvinceService _provinceService = provinceService;
        /// <summary>
        /// [All] {WIP} Get province list
        /// </summary>
        /// <returns>List of provinces</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<ProvincePreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProvinces()
        {
            return Ok(new DefaultResponseModel<List<ProvincePreviewDTO>>()
            {
                Message = "Get all province successfully",
                Data = await _provinceService.GetAllProvinces(),
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("province-detail")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<ProvinceDetailDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProvincesWithDetail(
            string? nameSearch,
            string? zoneId,
            int? pageSize,
            int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            var (totalCount, items) = await _provinceService.GetAllProvinceDetails(nameSearch,zoneId, checkedPageIndex, checkedPageSize);
            return Ok(new DefaultResponseModel<PaginatedList<ProvinceDetailDTO>>()
            {
                Data = new()
                {
                    Total = totalCount,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Items = items
                },
                Message = "Get all province successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
