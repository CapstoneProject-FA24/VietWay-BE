﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Management.ResponseModel;
using VietWay.Service.Management.DataTransferObject;
using VietWay.Service.Management.Interface;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerFeedbackController(ICustomerFeedbackService customerFeedbackService, IMapper mapper) : ControllerBase
    {
        private readonly ICustomerFeedbackService _customerFeedbackService = customerFeedbackService;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType<DefaultResponseModel<PaginatedList<CustomerFeedbackPreview>>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCustomerFeedback(int pageSize, int pageIndex)
        {
            var result = await _customerFeedbackService.GetAllCustomerFeedback(pageSize, pageIndex);
            List<CustomerFeedbackPreview> customerFeedbackPreviews = _mapper.Map<List<CustomerFeedbackPreview>>(result.items);
            PaginatedList<CustomerFeedbackPreview> pagedResponse = new()
            {
                Total = result.totalCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                Items = customerFeedbackPreviews
            };
            DefaultResponseModel<PaginatedList<CustomerFeedbackPreview>> response = new()
            {
                Data = pagedResponse,
                Message = "Get all customer feedback successfully",
                StatusCode = StatusCodes.Status200OK
            };
            return Ok(response);
        }
    }
}
