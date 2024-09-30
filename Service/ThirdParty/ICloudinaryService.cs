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
        public Task<string> UploadImageAsync(Stream stream, string fileName);
        public Task<string> DeleteImage(string publicId);
    }
}
