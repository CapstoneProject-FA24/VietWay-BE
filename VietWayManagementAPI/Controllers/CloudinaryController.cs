using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VietWay.Service.ThirdParty;

namespace VietWay.API.Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudinaryController(ICloudinaryService cloudinaryService) : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        [HttpGet]
        public IActionResult GetImage(string publicId)
        {
            return Ok(_cloudinaryService.GetImage(publicId));
        }
        [HttpPost]
        public async Task<IActionResult> UploadImageAsync(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            return Ok(await _cloudinaryService.UploadImageAsync(stream, file.FileName));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteImage(string publicId)
        {
            return Ok(await _cloudinaryService.DeleteImage(publicId));
        }
    }
}
