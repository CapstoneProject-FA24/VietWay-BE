using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController(IStaffService staffService, IMapper mapper) : ControllerBase
    {
        private readonly IStaffService _staffService = staffService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<StaffInfoPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStaffInfosAsync(int pageSize, int pageIndex)
        {
            var result = await _staffService.GetAllStaffInfos(pageSize, pageIndex);
            List<StaffInfoPreview> staffInfoPreviews = _mapper.Map<List<StaffInfoPreview>>(result.items);
            DefaultPageResponse<StaffInfoPreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = staffInfoPreviews
            };
            DefaultResponseModel<DefaultPageResponse<StaffInfoPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all staff info successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
