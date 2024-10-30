using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.Management.DataTransferObject
{
    public class TourCategoryDTO
    {
        public required string TourCategoryId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
