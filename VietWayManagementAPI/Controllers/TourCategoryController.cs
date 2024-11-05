using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/tour-categories")]
    [ApiController]
    public class TourCategoryController(ITourCategoryService tourCategoryService, IMapper mapper) : ControllerBase
    {
        private readonly IMapper _mapper = mapper;
        private readonly ITourCategoryService _tourCategoryService = tourCategoryService;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<TourCategoryDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTourCategoryAsync()
        {
            var result = await _tourCategoryService.GetAllTourCategory();
            DefaultResponseModel<List<TourCategoryDTO>> response = new()
            {
                Data = result,
                Message = "Get all tour category successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
