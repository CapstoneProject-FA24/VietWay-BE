using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourCategoryController(ITourCategoryService tourCategoryService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ITourCategoryService _tourCategoryService = tourCategoryService;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<TourCategoryPreview>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourCategoryAsync()
        {
            var result = await _tourCategoryService.GetAllTourCategory();
            List<TourCategoryPreview> tourCategoryPreviews = _mapper.Map<List<TourCategoryPreview>>(result);
            DefaultResponseModel<List<TourCategoryPreview>> response = new()
            {
                Data = tourCategoryPreviews,
                Message = "Get all tour category successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
