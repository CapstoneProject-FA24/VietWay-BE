using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
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

        /// <summary>
        /// ✅🔐[Admin] Change manager account status
        /// </summary>
        /// <returns>Manager account status changed</returns>
        /// <response code="200">Return manager account status changed</response>
        /// <response code="400">Bad request</response>
        [HttpPatch("change-manager-account-status/{managerId}")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeManagerStatusAsync(string managerId, bool isDeleted)
        {
            string? adminId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(adminId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            await _managerService.ChangeManagerStatusAsync(managerId, isDeleted);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Status change successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }
    }
}
