using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class ProvinceDetailDTO
    {
        public required string ProvinceId { get; set; }
        public required string ProvinceName { get; set; }
        public required string ImageUrl { get; set; }
        public required int AttractionsCount { get; set; }
        public required int PostsCount { get; set; }
        public required int ToursCount { get; set; }
    }
}
