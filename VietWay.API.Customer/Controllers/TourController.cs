using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Service.Customer.DataTransferObject;
using VietWay.Service.Customer.Interface;

namespace VietWay.API.Customer.Controllers
{
    /// <summary>
    /// Tour API Endpoints
    /// </summary>
    [Route("api/tours")]
    [ApiController]
    public class TourController(ITourService tourService) : ControllerBase
    {
        private readonly ITourService _tourService = tourService;

        /// <summary>
        /// ⚠️[All] Get tour by tour ID
        /// </summary>
        [HttpGet("{tourId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultResponseModel<TourDetailDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTourById(string tourId)
        {
            TourDetailDTO? tour = await _tourService
                .GetTourByIdAsync(tourId);
            if (tour == null)
            {
                DefaultResponseModel<object> response = new()
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"Can not find Tour with id {tourId}"
                };
                return NotFound(response);
            }
            else
            {
                DefaultResponseModel<TourDetailDTO> response = new()
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Get tour successfully",
                    Data = tour
                };
                return Ok(response);
            }
        }

        /// <summary>
        /// ⚠️[All] Get tour by tour template ID
        /// </summary>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<TourPreviewDTO>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllToursByTemplateIdsAsync(
            string tourTemplateId)
        {
            List<TourPreviewDTO> tourPreviews = await _tourService
                .GetAllToursByTemplateIdsAsync(tourTemplateId);

            DefaultResponseModel<List<TourPreviewDTO>> response = new()
            {
                Data = tourPreviews,
                Message = "Get all tour successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
