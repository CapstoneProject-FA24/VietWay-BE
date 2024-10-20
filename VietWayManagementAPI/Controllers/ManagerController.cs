using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerController (IManagerService managerService, IMapper mapper) : ControllerBase
    {
        public readonly IManagerService _managerService = managerService;
        public readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<ManagerInfoPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllManagerInfosAsync(int pageSize, int pageIndex)
        {
            var result = await _managerService.GetAllManagerInfos(pageSize, pageIndex);
            List<ManagerInfoPreview> managerInfoPreviews = _mapper.Map<List<ManagerInfoPreview>>(result.items);
            PaginatedList<ManagerInfoPreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = managerInfoPreviews
            };
            DefaultResponseModel<PaginatedList<ManagerInfoPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all manager info successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
