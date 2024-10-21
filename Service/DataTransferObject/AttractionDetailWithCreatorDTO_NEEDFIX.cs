using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietWay.Repository.EntityModel.Base;

namespace VietWay.Service.DataTransferObject
{
    public class AttractionDetailWithCreatorDTO_NEEDFIX
    {
        public required string AttractionId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string ContactInfo { get; set; }
        public string? Website { get; set; }
        public required string Description { get; set; }
        public string? GooglePlaceId { get; set; }
        public required AttractionStatus Status { get; set; }
        public required DateTime CreatedDate { get; set; }

        public required ProvincePreviewDTO Province { get; set; }
        public required AttractionTypePreviewDTO AttractionType { get; set; }
        public virtual ICollection<ImageDTO>? Images { get; set; }
    }
}
