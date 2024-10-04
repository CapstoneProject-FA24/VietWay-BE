using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Implement;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionTypeController(IAttractionTypeService attractionTypeService, IMapper mapper) : ControllerBase
    {
        private readonly IAttractionTypeService _attractionTypeService = attractionTypeService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<AttractionTypePreview>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttractionTypeAsync()
        {
            var result = await _attractionTypeService.GetAllAttractionType();
            List<AttractionTypePreview> attractionTypePreviews = _mapper.Map<List<AttractionTypePreview>>(result);
            DefaultResponseModel<List<AttractionTypePreview>> response = new()
            {
                Data = attractionTypePreviews,
                Message = "Get all attraction type successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
