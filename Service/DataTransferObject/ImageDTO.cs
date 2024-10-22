using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.DataTransferObject
{
    public class ImageDTO
    {
        public required string ImageId { get; set; }
        public required string Url { get; set; }
    }
}
