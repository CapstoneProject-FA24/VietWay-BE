﻿using System.ComponentModel.DataAnnotations;

namespace VietWay.API.Management.RequestModel
{
    public class CreateAttractionRequest
    {
        [StringLength(255)]
        public string? Name { get; set; }
        [StringLength(255)]
        public string? Address { get; set; }
        [StringLength(500)]
        public string? ContactInfo { get; set; }
        [StringLength(2048)]
        public string? Website { get; set; }
        public string? Description { get; set; }
        [StringLength(20)]
        [Required]
        public required string ProvinceId { get; set; }
        [StringLength(20)]
        [Required]
        public required string AttractionTypeId { get; set; }
        [StringLength(50)]
        public string? GooglePlaceId { get; set; }
        [Required]
        public bool IsDraft { get; set; }
    }
}
