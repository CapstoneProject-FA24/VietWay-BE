using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.RequestModel;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttractionController(IAttractionService attractionService, IMapper mapper) : ControllerBase
    {
        private readonly IAttractionService _attractionService = attractionService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<DefaultPageResponse<AttractionPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAttractionsAsync(
            string? nameSearch,
            [FromQuery] List<string>? provinceIds,
            [FromQuery] List<string>? attractionTypeIds,
            AttractionStatus? status,
            int? pageSize,
            int? pageIndex)
        {
            int checkedPageSize = (pageSize == null || pageSize < 1) ? 10 : (int)pageSize;
            int checkedPageIndex = (pageIndex == null || pageIndex < 1) ? 1 : (int)pageIndex;

            var (totalCount, items) = await _attractionService.GetAllAttractions(nameSearch, provinceIds, attractionTypeIds, status, checkedPageSize, checkedPageIndex);
            List<AttractionPreview> attractionPreviews = _mapper.Map<List<AttractionPreview>>(items);
            DefaultPageResponse<AttractionPreview> pagedResponse = new()
            {
                Total = totalCount,
                PageSize = checkedPageSize,
                PageIndex = checkedPageIndex,
                Items = attractionPreviews
            };
            DefaultResponseModel<DefaultPageResponse<AttractionPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all attractions successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
        [HttpGet("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<AttractionDetail>>(StatusCodes.Status200OK)]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAttractionById(string attractionId)
        {
            Attraction? attraction = await _attractionService.GetAttractionById(attractionId);
            if (null == attraction)
            {
                DefaultResponseModel<object> response = new()
                {
                    Message = "Attraction not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(response);
            }
            else 
            {
                DefaultResponseModel<AttractionDetail> response = new()
                {
                    Message = "Get attraction successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<AttractionDetail>(attraction)
                };
                return Ok(response);
            }
        }
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateAttractionAsync(CreateAttractionRequest request)
        {
            Attraction attraction = _mapper.Map<Attraction>(request);
            if (false == request.IsDraft &&
                (string.IsNullOrWhiteSpace(request.Name)||
                string.IsNullOrWhiteSpace(request.Address)||
                string.IsNullOrWhiteSpace(request.ContactInfo)||
                string.IsNullOrWhiteSpace(request.Description)))
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Incomplete attraction information",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorResponse);
            } 
            attraction.CreatedBy = "1"; 
            #warning Need to be replaced by staffid from jwt
            attraction.CreatedDate = DateTime.UtcNow;
            await _attractionService.CreateAttraction(attraction);
            DefaultResponseModel<object> response = new()
            {
                Message = "Create attraction successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpPut("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAttractionAsync(string attractionId,CreateAttractionRequest request)
        {
            Attraction? attraction = await _attractionService.GetAttractionById(attractionId);
            if (null == attraction)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Attraction not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(errorResponse);
            }
            if (false == request.IsDraft &&
                (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.Address) ||
                string.IsNullOrWhiteSpace(request.ContactInfo) ||
                string.IsNullOrWhiteSpace(request.Description)))
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Incomplete attraction information",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorResponse);
            }

            attraction.Address = request.Address ??"";
            attraction.ContactInfo = request.ContactInfo ?? "";
            attraction.Description = request.Description ?? "";
            attraction.GooglePlaceId = request.GooglePlaceId;
            attraction.Name = request.Name ?? "";
            attraction.ProvinceId = request.ProvinceId;
            attraction.Website = request.Website;
            attraction.AttractionTypeId = request.AttractionTypeId;
            attraction.Status = request.IsDraft ? AttractionStatus.Draft : AttractionStatus.Pending;
            await _attractionService.UpdateAttraction(attraction);
            DefaultResponseModel<object> response = new()
            {
                Message = "Update successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }

        [HttpDelete("{attractionId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteAttractionAsync(string attractionId)
        {
            Attraction? attraction = await _attractionService.GetAttractionById(attractionId);
            if (null == attraction)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Attraction not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(errorResponse);
            }
            await _attractionService.DeleteAttraction(attraction);
            DefaultResponseModel<object> response = new()
            {
                Message = "Delete successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
        [HttpPatch("{attractionId}/images")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<object>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateAttractionImageAsync(string attractionId, [FromForm] UpdateImageRequest request)
        {
            if (0 == request.NewImages?.Count && 0 == request.DeletedImageIds?.Count)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Nothing to update",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorResponse);
            }
            Attraction? attraction = await _attractionService.GetAttractionById(attractionId);
            if (null == attraction)
            {
                DefaultResponseModel<object> errorResponse = new()
                {
                    Message = "Attraction not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(errorResponse);
            }
            await _attractionService.UpdateAttractionImage(attraction, request.NewImages, request.DeletedImageIds);
            DefaultResponseModel<object> response = new()
            {
                Message = "Update image successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
