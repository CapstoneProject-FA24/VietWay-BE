﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Util.TokenUtil;
using VietWay.Service.Management.Interface;
using VietWay.Service.Management.DataTransferObject;
using VietWay.API.Management.RequestModel;
using VietWay.Service.Management.Implement;

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
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<StaffPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStaffInfosAsync(string? nameSearch,
            int? pageSize,
            int? pageIndex)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            int checkedPageSize = pageSize ?? 10;
            int checkedPageIndex = pageIndex ?? 1;

            (int totalCount, List<StaffPreviewDTO> items) = await _staffService.GetAllStaffInfos(
                nameSearch, checkedPageSize, checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<StaffPreviewDTO>>()
            {
                Message = "Success",
                Data = new PaginatedList<StaffPreviewDTO>
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
        /// ✅🔐[Manager] Change staff status
        /// </summary>
        /// <returns>Staff status changed</returns>
        /// <response code="200">Return staff account status changed</response>
        /// <response code="400">Bad request</response>
        [HttpPatch("change-staff-status/{staffId}")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Admin)}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStaffAccountStatusAsync(string staffId, bool isDeleted)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(accountId))
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

        [HttpPatch("change-staff-password")]
        [Authorize(Roles = nameof(UserRole.Staff))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeStaffPasswordAsync([FromBody] ChangePasswordRequest changePasswordRequest)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            await _staffService.StaffChangePassword(accountId, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return Ok();
        }

        [HttpPatch("admin-reset-staff-password")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AdminResetStaffPasswordAsync(string staffId)
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (string.IsNullOrWhiteSpace(accountId))
            {
                return Unauthorized(new DefaultResponseModel<object>
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            string password = await _staffService.AdminResetStaffPassword(staffId);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Status change successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = password,
            });
        }
    }
}
