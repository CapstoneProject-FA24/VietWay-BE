﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Implement;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/managers")]
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
        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<ManagerPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllManagerInfosAsync(string? nameSearch,
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

            (int totalCount, List<ManagerPreviewDTO> items) = await _managerService.GetAllManagerInfos(
                nameSearch, checkedPageSize, checkedPageIndex);
            return Ok(new DefaultResponseModel<PaginatedList<ManagerPreviewDTO>>()
            {
                Message = "Success",
                Data = new PaginatedList<ManagerPreviewDTO>
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

        [HttpPatch("change-manager-password")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeManagerPasswordAsync([FromBody] ChangePasswordRequest changePasswordRequest)
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

            await _managerService.ManagerChangePassword(accountId, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);
            return Ok();
        }

        [HttpPatch("admin-reset-manager-password")]
        [Authorize(Roles = nameof(UserRole.Admin))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> AdminResetManagerPasswordAsync(string managerId)
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

            await _managerService.AdminResetManagerPassword(managerId);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Password successfully",
                StatusCode = StatusCodes.Status200OK,
            });
        }

        [HttpGet("profile")]
        [Produces("application/json")]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [ProducesResponseType<DefaultResponseModel<ManagerDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentManagerProfileAsync()
        {
            string? accountId = _tokenHelper.GetAccountIdFromToken(HttpContext);
            if (accountId == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            ManagerDetailDTO? managerDetailDTO = await _managerService.GetManagerDetailAsync(accountId);
            if (managerDetailDTO == null)
            {
                return Unauthorized(new DefaultResponseModel<object>()
                {
                    Message = "Unauthorized",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<ManagerDetailDTO>()
            {
                Data = managerDetailDTO,
                Message = "Get current manager profile successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
