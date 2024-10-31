using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Service.ThirdParty.Cloudinary;

namespace VietWay.Service.Management.Jobs
{
    public class CloudImageProcessingJob(ICloudinaryService cloudinaryService)
    {
        private readonly ICloudinaryService _cloudinaryService = cloudinaryService;
        public async Task UploadImageAsync(string publicId, string imagePath, string fileName)
        {
            await _cloudinaryService.UploadImageAsync(publicId, imagePath, fileName);
            File.Delete(imagePath);
        }
        public async Task DeleteImagesAsync(IEnumerable<string> publicIds)
        {
            await _cloudinaryService.DeleteImagesAsync(publicIds);
        }
    }
}
