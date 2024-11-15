using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Province API endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController(IMapper mapper,
        ITokenHelper tokenHelper, 
        IProvinceService provinceService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProvinceService _provinceService = provinceService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// [Manager][Staff] Get all provinces
        /// </summary>
        /// <returns>List of provinces</returns>
        /// <response code="200">Return list of provinces</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<ProvincePreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProvinces(
            string? nameSearch,
            int? pageSize,
            int? pageIndex)
        {
            int checkedPageSize = pageSize ?? 10;
            int checkedPageIndex = pageIndex ?? 1;

            (int totalCount, List<ProvincePreviewDTO> items) = await _provinceService.GetAllProvinces(
                nameSearch, checkedPageSize, checkedPageIndex);

            return Ok(new DefaultResponseModel<PaginatedList<ProvincePreviewDTO>>() 
            { 
                Message= "Get all province successfully",
                Data = new PaginatedList<ProvincePreviewDTO>
                {
                    Items = items,
                    PageSize = checkedPageSize,
                    PageIndex = checkedPageIndex,
                    Total = totalCount
                },
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// [Manager][Staff] Get province by ID
        /// </summary>
        /// <returns>Province details</returns>
        /// <response code="200">Return province details</response>
        /// <response code="404">Province not found</response>
        [HttpGet("{provinceId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<ProvinceBriefPreviewDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProvinceById(string provinceId)
        {
            Province? province = await _provinceService
                .GetProvinceById(provinceId);
            if (province == null)
            {
                DefaultResponseModel<object> response = new()
                {
                    Message = "Province not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(response);
            } else
            {
                DefaultResponseModel<ProvinceBriefPreviewDTO> response = new()
                {
                    Message = "Get province successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<ProvinceBriefPreviewDTO>(province)
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// [Manager] Create new province
        /// </summary>
        /// <returns>Created province ID</returns>
        /// <response code="200">Return created province ID</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProvince(CreateProvinceRequest request)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(managerId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            Province province = _mapper.Map<Province>(request);
            string provinceId = await _provinceService.CreateProvinceAsync(province);
            return Ok(new DefaultResponseModel<string>

            {
                Message = "Create province successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = provinceId
            });
        }
        /// <summary>
        /// [Manager]Update current province
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns> Province update message</returns>
        /// <response code="200">Return province update message</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Province not found</response>
        [HttpPut("{provinceId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProvince(string provinceId, CreateProvinceRequest request)
        {
            Province province = _mapper.Map<Province>(request);
            province.ProvinceId = provinceId;

            await _provinceService.UpdateProvinceAsync(province);
            return Ok();
        }

        /// <summary>
        /// [Manager] {WIP} Delete province
        /// </summary>
        /// <returns> Province delete message </returns>
        /// <response code="200">Return province delete message</response>
        /// <response code="404">Province not found</response>
        [HttpDelete("{provinceId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteProvince(string provinceId)
        {
            throw new NotImplementedException();
        }

        [HttpPatch("{provinceId}/images")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateProvinceImageAsync(string provinceId, IFormFile? newImage)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (managerId == null)
            {
                return Unauthorized(new DefaultResponseModel<string>()
                {
                    Message = "Unauthorized",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            await _provinceService.UpdateProvinceImageAsync(provinceId, managerId, newImage);
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
