using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietWay.API.Management.RequestModel
{
    public class CreateTourTemplateRequest
    {
        public string? Code { get; set; }
        public string? TourName { get; set; }
        public string? Description { get; set; }
        [Required]
        public required string DurationId { get; set; }
        [Required]
        public required string TourCategoryId { get; set; }
        public string? Policy { get; set; }
        public string? Note { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public List<string>? ProvinceIds { get; set; }
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
