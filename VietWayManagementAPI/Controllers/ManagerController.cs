using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Implement;
using VietWay.Service.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
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
        /// ✅🔐[Manager] Activate staff account
        /// </summary>
        /// <returns>Staff account activated</returns>
        /// <response code="200">Return staff account activated</response>
        /// <response code="400">Bad request</response>
        [HttpPut("activate-staff-account/{staffId}")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivateStaffAccountAsync(string staffId, ActivateStaffAccountRequest request)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext) ?? "2";
            if (string.IsNullOrWhiteSpace(staffId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            Staff staff = _mapper.Map<Staff>(request);
            staff.StaffId = staffId;

            await _managerService.ActivateStaffAccountAsync(staff);
            return Ok();
        }

        /// <summary>
        /// ✅🔐[Manager] Deactivate staff account
        /// </summary>
        /// <returns>Staff account deactivated</returns>
        /// <response code="200">Return staff account deactivated</response>
        /// <response code="400">Bad request</response>
        [HttpPut("deactivate-staff-account/{staffId}")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeactivateStaffAccountAsync(string staffId, DeactivateStaffAccountRequest request)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext) ?? "2";
            if (string.IsNullOrWhiteSpace(staffId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            Staff staff = _mapper.Map<Staff>(request);
            staff.StaffId = staffId;

            await _managerService.ActivateStaffAccountAsync(staff);
            return Ok();
        }
    }
}
