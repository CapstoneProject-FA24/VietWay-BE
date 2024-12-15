using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VietWay.API.Management.RequestModel.CustomValidation;

namespace VietWay.API.Management.RequestModel
{
    public class CreateTourTemplateRequest
    {
        [RequiredIf(nameof(IsDraft), false)]
        public string? Code { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public string? TourName { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public string? Description { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public string? DurationId { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public string? TourCategoryId { get; set; }
        public string? Note { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public decimal? MinPrice { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public decimal? MaxPrice { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public List<string>? ProvinceIds { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public string? StartingProvinceId { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public string? Transportation { get; set; }
        [RequiredIf(nameof(IsDraft), false)]
        public List<TemplateSchedule>? Schedules { get; set; }
        [Required]
        public required bool IsDraft { get; set; }
    }
    public class TemplateSchedule
    {
        [Required]
        public int DayNumber { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<string>? AttractionIds { get; set; }
    }
}
