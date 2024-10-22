using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.DataTransferObject;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    /// <summary>
    /// Province API endpoints
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController(IMapper mapper, IProvinceService provinceService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProvinceService _provinceService = provinceService;

        /// <summary>
        /// [Manager][Staff] Get all provinces
        /// </summary>
        /// <returns>List of provinces</returns>
        /// <response code="200">Return list of provinces</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<ProvincePreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProvinces()
        {
            return Ok(new DefaultResponseModel<List<ProvincePreviewDTO>>() 
            { 
                Message= "Get all province successfully",
                Data = await _provinceService.GetAllProvinces(),
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
        /// [Manager] {WIP} Create new province
        /// </summary>
        /// <returns>Created province ID</returns>
        /// <response code="200">Return created province ID</response>
        /// <response code="400">Bad request</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateProvince()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// [Manager] {WIP} Update current province
        /// </summary>
        /// <param name="provinceId"></param>
        /// <returns> Province update message</returns>
        /// <response code="200">Return province update message</response>
        /// <response code="400">Bad request</response>
        /// <response code="404">Province not found</response>
        [HttpPut("{provinceId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateProvince(string provinceId)
        {
            throw new NotImplementedException();
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
    }
}
