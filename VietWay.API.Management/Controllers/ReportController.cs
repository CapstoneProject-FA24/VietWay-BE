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
    }
}
