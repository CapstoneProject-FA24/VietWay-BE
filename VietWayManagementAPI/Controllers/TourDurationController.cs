using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/tour-durations")]
    [ApiController]
    public class TourDurationController(ITourDurationService tourDurationService,
        ITokenHelper tokenHelper,
        IMapper mapper) : ControllerBase
    {
        private readonly ITourDurationService _tourDurationService = tourDurationService;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<TourDurationDTO>))]
        public async Task<IActionResult> GetAllTourDurationAsync()
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

            List<TourDurationDTO> items = await _tourDurationService.GetAllTourDuration(nameSearch);

            return Ok(new DefaultResponseModel<List<TourDurationDTO>>()
            {
                Message = "Success",
                Data = items,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{tourDurationId}")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [ProducesResponseType<DefaultResponseModel<PostDetailDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTourDurationById(string tourDurationId)
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

            TourDurationDTO? tourDuration = await _tourDurationService.GetTourCategoryByIdAsync(tourDurationId);
            if (null == tourDuration)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<TourDurationDTO>
            {
                Message = "Get tour category successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = tourDuration
            });
        }
    }
}
