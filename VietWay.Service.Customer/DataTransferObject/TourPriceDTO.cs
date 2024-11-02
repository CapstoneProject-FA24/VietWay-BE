using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class TourPriceDTO
    {
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required int AgeFrom { get; set; }
        public required int AgeTo { get; set; }

    }
}
