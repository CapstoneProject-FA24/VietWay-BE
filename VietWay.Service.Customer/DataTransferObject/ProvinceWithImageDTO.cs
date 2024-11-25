using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class ProvinceWithImageDTO
    {
        public required string ProvinceId { get; set; }
        public required string Name { get; set; }
        public required List<ImageDTO> Images { get; set; }
    }
}
