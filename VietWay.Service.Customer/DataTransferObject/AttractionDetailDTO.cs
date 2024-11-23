using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.Customer.DataTransferObject
{
    public class AttractionDetailDTO
    {
        public required string AttractionId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? ContactInfo { get; set; }
        public string? Website { get; set; }
        public string? Description { get; set; }
        public string? GooglePlaceId { get; set; }
        public int? LikeCount { get; set; }

        public bool IsLiked { get; set; }
        public required ProvincePreviewDTO Province { get; set; }
        public required AttractionCategoryPreviewDTO AttractionCategory { get; set; }
        public double? AverageRating { get; set; }
        public required List<RatingDTO> RatingCount { get; set; }
        public required List<ImageDTO> Images { get; set; }
    }
}
