using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController(IReportService reportService) : ControllerBase
    {
        private readonly IReportService _reportService = reportService;

        [HttpGet("summary")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> GetSummaryReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            return Ok(new DefaultResponseModel<ReportSummaryDTO>
            {
                Data = await _reportService.GetReportSummaryAsync(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("booking")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> GetBookingReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            return Ok(new DefaultResponseModel<ReportBookingDTO>
            {
                Data = await _reportService.GetReportBookingAsync(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("rating")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> GetRatingReportAsync(DateTime startDate, DateTime endDate, bool? isAscending)
        {
            isAscending ??= false;
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            return Ok(new DefaultResponseModel<ReportRatingDTO>
            {
                Data = await _reportService.GetReportRatingAsync(startDate, endDate, isAscending.Value),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("revenue")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        public async Task<IActionResult> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);

            return Ok(new DefaultResponseModel<ReportRevenueDTO>
            {
                Data = await _reportService.GetReportRevenueAsync(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }

        [HttpGet("promotion-summary")]
        [ProducesResponseType<DefaultResponseModel<ReportPromotionSummaryDTO>>(StatusCodes.Status200OK)]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]

        public async Task<IActionResult> GetPromotionSummary(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<ReportPromotionSummaryDTO>
            {
                Data = await _reportService.GetPromotionSummaryAsync(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-summary")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<ReportSocialMediaSummaryDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaSummary(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<ReportSocialMediaSummaryDTO>
            {
                Data = await _reportService.GetSocialMediaSummaryAsync(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-province")]
        [Produces("application/json")]
        //[Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<List<ReportSocialMediaProvinceDTO>>>(StatusCodes.Status200OK)] 
        public async Task<IActionResult> GetSocialMediaProvince(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<List<ReportSocialMediaProvinceDTO>>
            {
                Data = await _reportService.GetSocialMediaProvinceReport(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-post-category")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<List<ReportSocialMediaPostCategoryDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaPostCategory(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<List<ReportSocialMediaPostCategoryDTO>>
            {
                Data = await _reportService.GetSocialMediaPostCategoryReport(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-attraction-category")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<List<ReportSocialMediaAttractionCategoryDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaAttractionCategory(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<List<ReportSocialMediaAttractionCategoryDTO>>
            {
                Data = await _reportService.GetSocialMediaAttractionCategoryReport(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-tour-category")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<List<ReportSocialMediaAttractionCategoryDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaTourCategoryAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<List<ReportSocialMediaTourCategoryDTO>>
            {
                Data = await _reportService.GetSocialMediaTourTemplateCategoryReport(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-hashtag")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<List<ReportSocialMediaHashtagDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaHashtagAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<List<ReportSocialMediaHashtagDTO>>
            {
                Data = await _reportService.GetSocialMediaHashtagReport(startDate, endDate),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-province-detail/{provinceId}")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<ReportSocialMediaProvinceDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaProvinceDetailAsync(DateTime startDate, DateTime endDate, string provinceId)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<ReportSocialMediaProvinceDetailDTO>
            {
                Data = await _reportService.GetSocialMediaProvinceDetailReport(startDate, endDate, provinceId),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-attraction-category-detail/{attractionCategoryId}")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<ReportSocialMediaAttractionCategoryDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaAttractionCategoryDetailAsync(DateTime startDate, DateTime endDate, string attractionCategoryId)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<ReportSocialMediaAttractionCategoryDetailDTO>
            {
                Data = await _reportService.GetSocialMediaAttractionCategoryDetailReport(startDate, endDate, attractionCategoryId),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-post-category-detail/{postCategoryId}")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<ReportSocialMediaPostCategoryDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaPostCategoryDetailAsync(DateTime startDate, DateTime endDate, string postCategoryId)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<ReportSocialMediaPostCategoryDetailDTO>
            {
                Data = await _reportService.GetSocialMediaPostCategoryDetailReport(startDate, endDate, postCategoryId),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-tour-category-detail/{tourCategoryId}")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<ReportSocialMediaTourCategoryDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaTourCategoryDetailAsync(DateTime startDate, DateTime endDate, string tourCategoryId)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<ReportSocialMediaTourCategoryDetailDTO>
            {
                Data = await _reportService.GetSocialMediaTourCategoryDetailReport(startDate, endDate, tourCategoryId),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
        [HttpGet("social-media-hashtag-detail/{hashtagId}")]
        [Produces("application/json")]
        //[Authorize(Roles = $"{nameof(UserRole.Manager)},{nameof(UserRole.Admin)}")]
        [ProducesResponseType<DefaultResponseModel<ReportSocialMediaHashtagDetailDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSocialMediaHashtagDetailAsync(DateTime startDate, DateTime endDate, string hashtagId)
        {
            if (startDate > endDate)
            {
                return BadRequest(new DefaultResponseModel<string>
                {
                    Message = "START_DATE_MUST_BE_BEFORE_END_DATE",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            startDate = startDate.Date;
            endDate = endDate.Date.AddDays(1).AddSeconds(-1);
            return Ok(new DefaultResponseModel<ReportSocialMediaHashtagDetailDTO>
            {
                Data = await _reportService.GetSocialMediaHashtagDetailReport(startDate, endDate, hashtagId),
                StatusCode = StatusCodes.Status200OK,
                Message = "Success"
            });
        }
    }
}
