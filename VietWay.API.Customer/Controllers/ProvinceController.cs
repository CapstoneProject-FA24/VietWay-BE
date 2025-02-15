﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Province API endpoints
    /// </summary>
    [Route("api/provinces")]
    [ApiController]
    public class ProvinceController(IProvinceService provinceService) : ControllerBase
    {
        private readonly IProvinceService _provinceService = provinceService;
        /// <summary>
        /// ✅[All] Get province list
        /// </summary>
        /// <returns>List of provinces</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<ProvincePreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProvinces()
        {
            return Ok(new DefaultResponseModel<List<ProvincePreviewDTO>>()
            {
                Message = "Get all province successfully",
                Data = await _provinceService.GetProvinces(),
                StatusCode = StatusCodes.Status200OK
            });
        }

        /// <summary>
        /// ✅[All] Get province list with detail
        /// </summary>
        [HttpGet("province-detail")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<ProvinceDetailDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProvincesDetails(
            string? nameSearch,
            string? zoneId,
            int? pageSize,
            int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;
            return Ok(new DefaultResponseModel<PaginatedList<ProvinceDetailDTO>>()
            {
                Data = await _provinceService.GetProvincesDetails(nameSearch, zoneId, checkedPageIndex, checkedPageSize),
                Message = "Get all province successfully",
                StatusCode = StatusCodes.Status200OK
            });
        }
        [HttpGet("{provinceId}/images")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<ProvinceWithImageDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProvinceImagesAsync(string provinceId,[Required] int imageCount)
        {
            return Ok(new DefaultResponseModel<ProvinceWithImageDTO>()
            {
                Message = "Success",
                Data = await _provinceService.GetProvinceImagesAsync(provinceId, imageCount),
                StatusCode = StatusCodes.Status200OK,
            });
        }
    }
}