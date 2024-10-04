﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;

namespace VietWay.API.Management.ResponseModel
{
    public class AttractionDetail
    {
        public required string AttractionId { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required string ContactInfo { get; set; }
        public string? Website { get; set; }
        public required string Description { get; set; }
        public string? GooglePlaceId { get; set; }
        public required AttractionStatus Status { get; set; }

        public required ProvincePreview Province { get; set; }
        public required AttractionTypePreview AttractionType { get; set; }
        public virtual ICollection<ImageDetail>? Images { get; set; }
    }
}
