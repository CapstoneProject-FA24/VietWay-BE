using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Implement;
using VietWay.Service.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/staff")]
    [ApiController]
    public class StaffController(IStaffService staffService,
        ITokenHelper tokenHelper,
        IMapper mapper) : ControllerBase
    {
        private readonly IStaffService _staffService = staffService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<StaffInfoPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStaffInfosAsync(int pageSize, int pageIndex)
        {
            var result = await _staffService.GetAllStaffInfos(pageSize, pageIndex);
            List<StaffInfoPreview> staffInfoPreviews = _mapper.Map<List<StaffInfoPreview>>(result.items);
            PaginatedList<StaffInfoPreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = staffInfoPreviews
            };
            DefaultResponseModel<PaginatedList<StaffInfoPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all staff info successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        /// <summary>
        /// ✅🔐[Manager] Change staff status
        /// </summary>
        /// <returns>Staff status changed</returns>
        /// <response code="200">Return staff account status changed</response>
        /// <response code="400">Bad request</response>
        [HttpPatch("change-staff-status/{staffId}")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStaffAccountStatusAsync(string staffId, bool isDeleted)
        {
            string? managerId = _tokenHelper.GetAccountIdFromToken(HttpContext) ?? "2";
            if (string.IsNullOrWhiteSpace(managerId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            await _staffService.ChangeStaffStatusAsync(staffId, isDeleted);
            return Ok();
        }
    }
}
