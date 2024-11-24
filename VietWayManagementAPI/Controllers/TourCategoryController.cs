using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;
using VietWay.Util.TokenUtil;

namespace VietWay.API.Management.Controllers
{
    [Route("api/tour-categories")]
    [ApiController]
    public class TourCategoryController(ITourCategoryService tourCategoryService,
        ITokenHelper tokenHelper,
        IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ITourCategoryService _tourCategoryService = tourCategoryService;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DefaultResponseModel<TourCategoryDTO>))]
        public async Task<IActionResult> GetAllTourCategoryAsync(
            string? nameSearch)
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

            List<TourCategoryDTO> items = await _tourCategoryService.GetAllTourCategoryAsync(nameSearch);

            return Ok(new DefaultResponseModel<List<TourCategoryDTO>>()
            {
                Message = "Success",
                Data = items,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("{tourCategoryId}")]
        [Produces("application/json")]
        [Authorize(Roles = $"{nameof(UserRole.Manager)}, {nameof(UserRole.Staff)}")]
        [ProducesResponseType<DefaultResponseModel<PostDetailDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTourCategoryById(string tourCategoryId)
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

            TourCategoryDTO? tourCategory = await _tourCategoryService.GetTourCategoryByIdAsync(tourCategoryId);
            if (null == tourCategory)
            {
                return NotFound(new DefaultResponseModel<object>
                {
                    Message = "Not found",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new DefaultResponseModel<TourCategoryDTO>
            {
                Message = "Get tour category successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = tourCategory
            });
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRole.Manager))]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<string>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateTourCategoryAsync(CreateTourCategoryRequest request)
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

            TourCategory tourCategory = _mapper.Map<TourCategory>(request);
            string tourCategoryId = await _tourCategoryService.CreateTourCategoryAsync(tourCategory);
            return Ok(new DefaultResponseModel<string>
            {
                Message = "Create post successfully",
                StatusCode = StatusCodes.Status200OK,
                Data = tourCategoryId
            });
        }

        [HttpPut("{tourCategoryId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTourCategory(string tourCategoryId, CreateTourCategoryRequest request)
        {
            TourCategory tourCategory = _mapper.Map<TourCategory>(request);
            tourCategory.TourCategoryId = tourCategoryId;

            await _tourCategoryService.UpdateTourCategoryAsync(tourCategory);
            return Ok();
        }

        [HttpDelete("{tourCategoryId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTourCategory(string tourCategoryId)
        {
            await _tourCategoryService.DeleteTourCategory(tourCategoryId);
            return Ok(new DefaultResponseModel<string>()
            {
                Message = "Success",
                Data = null,
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
