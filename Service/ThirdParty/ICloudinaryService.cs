using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty
{
    public interface ICloudinaryService
    {
        public string GetImage(string publicId);
        public Task<(string publicId, string secureUrl)> UploadImageAsync(Stream stream, string fileName);
        public Task<bool> DeleteImage(string publicId);
    }
}
