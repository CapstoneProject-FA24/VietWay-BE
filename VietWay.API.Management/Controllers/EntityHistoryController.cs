using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Repository.EntityModel.Base;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/entity-histories")]
    [ApiController]
    public class EntityHistoryController(IEntityHistoryService entityHistoryService) : ControllerBase
    {
        private readonly IEntityHistoryService _entityHistoryService = entityHistoryService;
        [HttpGet("{entityId}")]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<List<EntityHistoryDTO>>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.Manager)}")]
        public async Task<IActionResult> GetEntityHistoryAsync(string entityId, EntityType entityType)
        {
            return Ok(new DefaultResponseModel<List<EntityHistoryDTO>>
            {
                Message = "Success",
                Data = await _entityHistoryService.GetEntityHistoryAsync(entityId, entityType),
                StatusCode = StatusCodes.Status200OK
            });
        }
    }
}
