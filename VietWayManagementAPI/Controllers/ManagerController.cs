using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/manager")]
    [ApiController]
    public class ManagerController (IManagerService managerService,
        ITokenHelper tokenHelper,
        IMapper mapper) : ControllerBase
    {
        private readonly IManagerService _managerService = managerService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

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
