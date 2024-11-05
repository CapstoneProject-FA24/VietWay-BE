using System.ComponentModel.DataAnnotations;
using VietWay.API.Management.RequestModel.CustomValidation;

namespace VietWay.API.Management.RequestModel
{
    public class CreateAttractionRequest
    {
        [StringLength(255)]
        [RequiredIf(nameof(IsDraft), false)]
        public string? Name { get; set; }
        [StringLength(255)]
        [RequiredIf(nameof(IsDraft), false)]
        public string? Address { get; set; }
        [StringLength(500)]
        [RequiredIf(nameof(IsDraft), false)]
        public string? ContactInfo { get; set; }
        [StringLength(2048)]
        public string? Website { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public string? Description { get; set; }
        [StringLength(20)]
        [Required]
        public required string ProvinceId { get; set; }
        [StringLength(20)]
        [Required]
        public required string AttractionCategoryId { get; set; }
        [StringLength(50)]
        public string? GooglePlaceId { get; set; }
        [Required]
        public bool IsDraft { get; set; }
    }
}
