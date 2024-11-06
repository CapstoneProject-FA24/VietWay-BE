using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class AttractionPreviewDTO
    {
        public required string AttractionId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Province { get; set; }
        public string? AttractionCategory { get; set; }
        public string? ImageUrl { get; set; }
        public double? AverageRating { get; set; }
        public int? LikeCount { get; set; }
    }
}
