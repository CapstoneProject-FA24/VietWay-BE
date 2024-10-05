using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using VietWay.Repository.EntityModel;
using VietWay.Service.Interface;
using VietWay.API.Customer.ResponseModel;

namespace VietWay.API.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController(ICustomerService customerService, IMapper mapper) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<CustomerProfile>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCurrentCustomerProfile()
        {
            var result = await _customerService.GetCustomerById("4");
            if (result == null)
            {
                DefaultResponseModel<object> response = new()
                {
                    Message = "Customer not found",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return NotFound(response);
            }
            else
            {
                DefaultResponseModel<CustomerProfile> response = new()
                {
                    Message = "Get customer successfully",
                    StatusCode = StatusCodes.Status200OK,
                    Data = _mapper.Map<CustomerProfile>(result)
                };
                return Ok(response);
            }
        }
    }
}
