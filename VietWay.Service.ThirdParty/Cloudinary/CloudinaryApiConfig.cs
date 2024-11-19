using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Cloudinary
{
    public class CloudinaryApiConfig
    {
        public required string ApiKey { get; set; }
        public required string ApiSecret { get; set; }
        public required string CloudName { get; set; }
    }
}
