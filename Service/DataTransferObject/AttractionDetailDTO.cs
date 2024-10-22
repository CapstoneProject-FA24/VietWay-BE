using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.DataTransferObject
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

        public required ProvinceBriefPreviewDTO Province { get; set; }
        public required AttractionCategoryPreviewDTO AttractionType { get; set; }
        public virtual ICollection<ImageDTO>? Images { get; set; }
    }
}
