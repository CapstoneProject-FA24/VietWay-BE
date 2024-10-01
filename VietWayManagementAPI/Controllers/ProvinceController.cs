using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController(IMapper mapper, IProvinceService provinceService) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly IProvinceService _provinceService = provinceService;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<ProvincePreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProvinces()
        {
            List<Province> provinces = await _provinceService.GetAllProvince();
            List<ProvincePreview> response = _mapper.Map<List<ProvincePreview>>(provinces);
            return Ok(new DefaultResponseModel<List<ProvincePreview>>() 
            { 
                Message= "Get all province successfully",
                Data = response,
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpGet("{provinceId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<ProvincePreview>>(StatusCodes.Status200OK)]
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
                DefaultResponseModel<ProvincePreview> response = new()
                {
                    Message = "Get province successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<ProvincePreview>(province)
                };
                return Ok(response);
            }
        }
    }
}
