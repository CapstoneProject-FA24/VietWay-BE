using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.API.Customer.RequestModel;
using VietWay.Service.ThirdParty.GoogleGemini;
namespace VietWay.API.Customer.Controllers
{
    [Route("api/gemini-ai")]
    [ApiController]
    public class GeminiAIController(IGeminiService geminiService, IMapper mapper) : ControllerBase
    {
        private readonly IGeminiService _geminiServince = geminiService;
        private readonly IMapper _mapper = mapper;

        [HttpGet("query")]
        public async Task<IActionResult> QueryAsync(string content)
        {
            return Ok(await _geminiServince.QueryAsync(content));
        }
        [HttpPost("chat")]
        public async Task<IActionResult> ChatAsync(List<ChatRequest> request)
        {
            List<Content> contents = _mapper.Map<List<Content>>(request);
            return Ok(await _geminiServince.ChatAsync(contents));
        }
    }
}
