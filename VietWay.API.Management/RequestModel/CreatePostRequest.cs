using System.ComponentModel.DataAnnotations.Schema;
using VietWay.Repository.EntityModel.Base;
using VietWay.Repository.EntityModel;
using System.ComponentModel.DataAnnotations;
using VietWay.API.Management.RequestModel.CustomValidation;

namespace VietWay.API.Management.RequestModel
{
    public class CreatePostRequest
    {
        [StringLength(255)]
        [RequiredIf(nameof(IsDraft), false)]
        public string? Title { get; set; }

        [RequiredIf(nameof(IsDraft), false)]
        public string? Content { get; set; }

        [RequiredIf(nameof(IsDraft), false)]
        public string? PostCategoryId { get; set; }

        [RequiredIf(nameof(IsDraft), false)]
        public string? ProvinceId { get; set; }

        [RequiredIf(nameof(IsDraft), false)]
        public string? Description { get; set; }

        [Required]
        public bool IsDraft { get; set; }
    }
}
