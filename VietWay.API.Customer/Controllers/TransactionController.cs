using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.Repository.EntityModel;
using VietWay.Service.ThirdParty;
using VietWay.Util.IdHelper;

namespace VietWay.API.Customer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController(IVnPayService vnPayService, IIdGenerator idGenerator) : ControllerBase
    {
        private readonly IVnPayService _vnPayService = vnPayService;
        private readonly IIdGenerator _idGenerator = idGenerator;
        [HttpGet]
        public async Task<IActionResult> GetUrl() 
        {
            Transaction transaction = new Transaction
            {
                Amount = 100000,
                TransactionId = _idGenerator.GenerateId(),
                CreateOn = DateTime.UtcNow,
                Note = ""
            };
            string paymentUrl = _vnPayService.GetPaymentUrl(transaction, HttpContext.Connection.RemoteIpAddress.ToString());
            return Ok(paymentUrl);
        }
    }
}
